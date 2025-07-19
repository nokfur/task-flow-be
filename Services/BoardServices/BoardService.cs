using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BusinessObjects;
using BusinessObjects.Constants;
using BusinessObjects.DTOs.Board.Request;
using BusinessObjects.DTOs.Board.Response;
using BusinessObjects.Models;
using Microsoft.AspNetCore.Http;
using Repositories.Repositories;

namespace Services.BoardServices
{
    public class BoardService : IBoardService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BoardService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ICollection<BoardPreviewResponse>> GetBoardsByUserId(string? userId)
        {
            var boards = await _unitOfWork.Boards.GetAsync(b => b.OwnerId.Equals(userId) ||
                b.Members.Any(m => m.Id.Equals(userId)),
                "Members, Labels, Columns, Columns.Tasks, Columns.Tasks.Labels");

            var response = _mapper.Map<ICollection<BoardPreviewResponse>>(boards, opt => { opt.Items["UserId"] = userId; });

            return response;
        }

        public async Task<BoardDetailResponse> GetBoardDetailById(string id, string? userId)
        {
            var board = await _unitOfWork.Boards.SingleOrDefaultAsync(b => b.Id.Equals(id), 
                "Members, Labels, Columns, Columns.Tasks, Columns.Tasks.Labels");

            if (board == null) throw new CustomException("Board Id not found");
            if (!board.IsTemplate && !board.OwnerId.Equals(userId) && !board.Members.Any(m => m.Id.Equals(userId))) 
                throw new CustomException("You are not permit to access this board", 403);

            return _mapper.Map<BoardDetailResponse>(board);
        }

        public async Task<ICollection<BoardPreviewResponse>> GetBoardTemplatesPreview()
        {
            var boards = await _unitOfWork.Boards.GetAsync(b => b.IsTemplate, 
                "Labels, Columns, Columns.Tasks, Columns.Tasks.Labels");
            return _mapper.Map<ICollection<BoardPreviewResponse>>(boards);
        }

        public async Task<ICollection<BoardTemplateResponse>> GetBoardTemplatesForSetup()
        {
            var boards = await _unitOfWork.Boards.GetAsync(b => b.IsTemplate,
                "Columns");
            return _mapper.Map<ICollection<BoardTemplateResponse>>(boards);
        }        

        public async Task AddBoardTemplate(BoardTemplateAddRequest request, string? userId)
        {
            var currentUser = await _unitOfWork.Users.SingleOrDefaultAsync(x => x.Id.Equals(userId));
            if (currentUser == null) throw new CustomException("User not exist");

            if (await _unitOfWork.Boards.IsExistAsync(b => b.Title.Equals(request.Title) && b.IsTemplate))
                throw new CustomException("Template with this name already exists");

            var newBoard = _mapper.Map<Board>(request);
            newBoard.OwnerId = userId;

            // Map and assign new IDs to labels with tracking
            var labelMap = new Dictionary<string, Label>();

            foreach (var label in newBoard.Labels)
            {
                var clientId = label.Id;
                label.Id = Guid.NewGuid().ToString();
                label.BoardId = newBoard.Id;

                labelMap[clientId] = label; // track by original ID from client
            }
            newBoard.Labels = labelMap.Values.ToList();

            foreach (var column in newBoard.Columns)
            {
                column.BoardId = newBoard.Id;
                
                foreach(var task in column.Tasks)
                {
                    task.ColumnId = column.Id;

                    // assigning labels with their corresponding ones from client
                    task.Labels = task.Labels
                        .Where(label => labelMap.ContainsKey(label.Id))
                        .Select(label => labelMap[label.Id])
                        .ToList();
                }
            }

            await _unitOfWork.Boards.AddAsync(newBoard);
            await _unitOfWork.SaveChangesAsync();
        }

        private async Task CloneBoardFromTemplate(string templateId, Board board)
        {
            var template = await _unitOfWork.Boards.SingleOrDefaultAsync(b => b.Id.Equals(templateId) && b.IsTemplate,
                "Labels, Columns, Columns.Tasks, Columns.Tasks.Labels");

            if (template == null) throw new CustomException("Template not found");

            // 1. Clone labels
            var labelMap = new Dictionary<string, Label>();
            foreach (var label in template.Labels)
            {
                var newLabel = _mapper.Map<Label>(label);
                newLabel.Id = Guid.NewGuid().ToString();
                newLabel.BoardId = board.Id;

                labelMap[label.Id] = newLabel;
                board.Labels.Add(newLabel);
            }

            // 2. Clone columns and tasks
            foreach (var col in template.Columns)
            {
                var newColumn = _mapper.Map<Column>(col);
                newColumn.Id = Guid.NewGuid().ToString();
                newColumn.BoardId = board.Id;

                foreach (var task in col.Tasks)
                {
                    var newTask = _mapper.Map<WorkTask>(task);
                    newTask.Id = Guid.NewGuid().ToString();
                    newTask.CreatedAt = DateTime.Now;
                    newTask.UpdatedAt = DateTime.Now;
                    newTask.ColumnId = newColumn.Id;

                    // Re-create TaskLabels using new label IDs
                    newTask.TaskLabels = task.Labels
                                        .Where(l => labelMap.ContainsKey(l.Id))
                                        .Select(l => new TaskLabel
                                        {
                                            LabelId = labelMap[l.Id].Id,
                                            TaskId = newTask.Id
                                        })
                                        .ToList();

                    newColumn.Tasks.Add(newTask);
                }

                board.Columns.Add(newColumn);
            }
        }

        public async Task AddBoard(BoardAddRequest request, string? userId)
        {
            var currentUser = await _unitOfWork.Users.SingleOrDefaultAsync(x => x.Id.Equals(userId));
            if (currentUser == null) throw new CustomException("User not exist");

            if (await _unitOfWork.Boards.IsExistAsync(b => b.Title.Equals(request.Title) && b.OwnerId.Equals(userId)))
                throw new CustomException("You have already had a Board with this name");

            var newBoard = _mapper.Map<Board>(request);
            newBoard.OwnerId = userId;
                        
            var boardMembers = _mapper.Map<ICollection<BoardMember>>(request.BoardMembers, opt => opt.Items["BoardId"] = newBoard.Id);
            newBoard.BoardMembers = boardMembers;

            if (!string.IsNullOrEmpty(request.TemplateId))
            {
                await CloneBoardFromTemplate(request.TemplateId, newBoard);
            }

            await _unitOfWork.Boards.AddAsync(newBoard);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateBoard(string boardId, BoardUpdateRequest request, string? userId)
        {
            var board = await _unitOfWork.Boards.SingleOrDefaultAsync(b => b.Id.Equals(boardId));
            var user = await _unitOfWork.Users.SingleOrDefaultAsync(u => u.Id.Equals(userId));

            if (board == null) throw new CustomException("Board not found", 404);
            if (user == null) throw new CustomException("User not found", 404);

            if ((board.IsTemplate && !user.Role.Equals(UserRoles.Admin)) || 
                !board.OwnerId.Equals(userId)) 
                throw new CustomException("You are not allowed to perform this action", StatusCodes.Status403Forbidden);
            
            if (await _unitOfWork.Boards.IsExistAsync(b => !b.Id.Equals(boardId) && b.Title.Equals(request.Title) && (b.IsTemplate || b.OwnerId.Equals(userId))))
                throw new CustomException("You have already had a Board with this name");

            _mapper.Map(request, board);

            _unitOfWork.Boards.Update(board);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteBoard(string boardId, string? userId)
        {
            var board = await _unitOfWork.Boards.SingleOrDefaultAsync(b => b.Id.Equals(boardId), "Members, Labels");
            var user = await _unitOfWork.Users.SingleOrDefaultAsync(u => u.Id.Equals(userId));

            if (board == null) throw new CustomException("Board not found", 404);
            if (user == null) throw new CustomException("User not found", 404);

            if ((board.IsTemplate && !user.Role.Equals(UserRoles.Admin)) ||
                !board.OwnerId.Equals(userId))
                throw new CustomException("You are not allowed to perform this action", StatusCodes.Status403Forbidden);

            var LabelIds = board.Labels.Select(l => l.Id).ToHashSet();
            var taskLabels = await _unitOfWork.TaskLabels.GetAsync(tl => LabelIds.Contains(tl.LabelId));

            var memberIds = board.Members.Select(m => m.Id).ToHashSet();
            var boardMembers = await _unitOfWork.BoardMembers.GetAsync(bm => memberIds.Contains(bm.MemberId));

            _unitOfWork.TaskLabels.DeleteRange(taskLabels);
            _unitOfWork.BoardMembers.DeleteRange(boardMembers);
            _unitOfWork.Boards.Delete(board);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
