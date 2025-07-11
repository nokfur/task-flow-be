using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.DTOs.WorkTask.Request;
using BusinessObjects.Models;
using BusinessObjects;
using Repositories.Repositories;
using BusinessObjects.DTOs.Label.Request;
using AutoMapper;
using BusinessObjects.DTOs.Label.Response;

namespace Services.LabelServices
{
    public class LabelService : ILabelService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LabelService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<LabelDetailResponseModel> AddLabel(string boardId, LabelAddRequestModel request)
        {
            if (!await _unitOfWork.Boards.IsExistAsync(c => c.Id.Equals(boardId)))
                throw new CustomException("Board Id not found");
            if (await _unitOfWork.Labels.IsExistAsync(c => c.Name.Equals(request.Name) && c.BoardId.Equals(boardId)))
                throw new CustomException("Label name has existed in this Board");

            var newLabel = _mapper.Map<Label>(request);
            newLabel.BoardId = boardId;

            await _unitOfWork.Labels.AddAsync(newLabel);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<LabelDetailResponseModel>(newLabel);
        }

        public async Task UpdateLabel(string labelId, LabelUpdateRequestModel request)
        {
            var label = await _unitOfWork.Labels.SingleOrDefaultAsync(c => c.Id.Equals(labelId));

            if (label == null) throw new CustomException("Label Id not found");
            if (await _unitOfWork.Labels.IsExistAsync(x => x.BoardId.Equals(label.BoardId) && x.Name.Equals(request.Name) && !x.Id.Equals(labelId)))
                throw new CustomException("Label name has existed in this Board");

            _mapper.Map(request, label);

            _unitOfWork.Labels.Update(label);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteLabel(string labelId)
        {
            var label = await _unitOfWork.Labels.SingleOrDefaultAsync(c => c.Id.Equals(labelId));

            if (label == null) throw new CustomException("Label Id not found");

            var taskLabels = await _unitOfWork.TaskLabels.GetAsync(tl => tl.LabelId.Equals(labelId));

            _unitOfWork.TaskLabels.DeleteRange(taskLabels);
            _unitOfWork.Labels.Delete(label);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
