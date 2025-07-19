using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.DTOs.Label.Request;
using BusinessObjects.DTOs.Label.Response;

namespace Services.LabelServices
{
    public interface ILabelService
    {
        Task<LabelDetailResponse> AddLabel(string boardId, LabelAddRequest request);
        Task UpdateLabel(string labelId, LabelUpdateRequest request);
        Task DeleteLabel(string labelId);
    }
}
