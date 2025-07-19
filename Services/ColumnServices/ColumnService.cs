using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Azure.Core;
using BusinessObjects;
using BusinessObjects.DTOs.Column.Request;
using BusinessObjects.DTOs.Column.Response;
using BusinessObjects.Models;
using Repositories.Repositories;
using Services.Utils;

namespace Services.ColumnServices
{
    public class ColumnService : IColumnService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ColumnService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ColumnDetailResponse> AddColumn(ColumnAddRequest request, string boardId)
        {
            if (!await _unitOfWork.Boards.IsExistAsync(c => c.Id.Equals(boardId)))
                throw new CustomException("Board Id not found");

            var existingTitles = (await _unitOfWork.Columns.GetAsync(t => t.BoardId == boardId && t.Title.StartsWith(request.Title)))
                    .Select(t => t.Title).ToHashSet();
            request.Title = Util.GenerateUniqueTitle(request.Title, existingTitles);

            var columns = await _unitOfWork.Columns.GetAsync(c => c.BoardId.Equals(boardId));
            int lastPostion = columns.Count() - 1;

            var newColumn = _mapper.Map<Column>(request);
            newColumn.BoardId = boardId;
            newColumn.Position = lastPostion + 1;

            await _unitOfWork.Columns.AddAsync(newColumn);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ColumnDetailResponse>(newColumn);
        }

        public async Task UpdateColumn(string columnId, ColumnUpdateRequest request)
        {
            var column = await _unitOfWork.Columns.SingleOrDefaultAsync(c => c.Id.Equals(columnId));

            if (column == null) throw new CustomException("Column Id not found");
            if (await _unitOfWork.Columns.IsExistAsync(x => x.BoardId.Equals(column.BoardId) && x.Title.Equals(request.Title) && !x.Id.Equals(columnId)))
                throw new CustomException("Column name has existed in this board");

            _mapper.Map(request, column);

            _unitOfWork.Columns.Update(column);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateColumnPositions(List<ColumnPositionUpdateRequest> request)
        {
            var requestIds = request.Select(c => c.Id).ToHashSet();

            var columns = await _unitOfWork.Columns.GetAsync(c => requestIds.Contains(c.Id));

            foreach (var column in columns)
            {
                var newPos = request.FirstOrDefault(c => c.Id.Equals(column.Id))?.Position;
                if (newPos.HasValue)
                {
                    column.Position = newPos.Value;
                }
            }

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteColumn(string columnId)
        {
            var column = await _unitOfWork.Columns.SingleOrDefaultAsync(c => c.Id.Equals(columnId));

            if (column == null) throw new CustomException("Column Id not found");

            var columns = await _unitOfWork.Columns.GetAsync(c => c.BoardId.Equals(column.BoardId) && !c.Id.Equals(columnId));
            foreach (var col in columns)
            {
                if (col.Position > column.Position)
                {
                    col.Position--;
                }
            }

            _unitOfWork.Columns.Delete(column);
            _unitOfWork.Columns.UpdateRange(columns);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
