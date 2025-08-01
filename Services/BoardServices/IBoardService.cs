using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.DTOs.Board.Request;
using BusinessObjects.DTOs.Board.Response;
using BusinessObjects.DTOs.User.Response;
using BusinessObjects.Models;

namespace Services.BoardServices
{
    public interface IBoardService
    {
        Task<ICollection<BoardPreviewResponse>> GetBoardsByUserId(string? userId);
        Task<BoardDetailResponse> GetBoardDetailById(string id, string? userId);
        Task<ICollection<BoardPreviewResponse>> GetBoardTemplatesPreview();
        Task<ICollection<BoardTemplateResponse>> GetBoardTemplatesForSetup();
        Task AddBoardTemplate(BoardTemplateAddRequest request, string? userId);        

        Task AddBoard(BoardAddRequest request, string? userId);
        Task UpdateBoard(string boardId, BoardUpdateRequest request, string? userId);
        Task DeleteBoard(string boardId, string? userId);
    }
}
