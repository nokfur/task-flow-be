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
        Task<ColumnDetailResponseModel> AddColumn(ColumnAddRequestModel request, string boardId);
        Task UpdateColumn(string columnId, ColumnUpdateRequestModel request);
        Task UpdateColumnPositions(List<ColumnPositionUpdateRequestModel> request);
        Task DeleteColumn(string columnId);
    }
}
