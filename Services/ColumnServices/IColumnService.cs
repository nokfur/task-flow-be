using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.DTOs.Board.Request;
using BusinessObjects.DTOs.Column.Request;
using BusinessObjects.DTOs.Column.Response;

namespace Services.ColumnServices
{
    public interface IColumnService
    {
        Task<ColumnDetailResponse> AddColumn(ColumnAddRequest request, string boardId);
        Task UpdateColumn(string columnId, ColumnUpdateRequest request);
        Task UpdateColumnPositions(List<ColumnPositionUpdateRequest> request);
        Task DeleteColumn(string columnId);
    }
}
