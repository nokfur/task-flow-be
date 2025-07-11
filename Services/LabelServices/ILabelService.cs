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
        Task<LabelDetailResponseModel> AddLabel(string boardId, LabelAddRequestModel request);
        Task UpdateLabel(string labelId, LabelUpdateRequestModel request);
        Task DeleteLabel(string labelId);
    }
}
