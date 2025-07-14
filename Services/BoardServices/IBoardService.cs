using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.DTOs.Board.Request;
using BusinessObjects.DTOs.Board.Response;
using BusinessObjects.Models;

namespace Services.BoardServices
{
    public interface IBoardService
    {
        Task<ICollection<BoardResponseModel>> GetBoardsByUserId(string? userId);
        Task<BoardDetailResponseModel> GetBoardDetailById(string id, string? userId);
        Task<ICollection<BoardResponseModel>> GetBoardTemplates();
        Task<ICollection<BoardTemplateResponseModel>> GetBoardTemplatesForSetup();
        Task AddBoardTemplate(BoardTemplateAddRequestModel request, string? userId);

        Task AddBoard(BoardAddRequestModel request, string? userId);
        Task UpdateBoard(string boardId, BoardUpdateRequestModel request, string? userId);
        Task DeleteBoard(string boardId, string? userId);
    }
}
