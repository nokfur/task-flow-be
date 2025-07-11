using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BusinessObjects;
using BusinessObjects.DTOs.Column.Request;
using BusinessObjects.DTOs.WorkTask.Request;
using BusinessObjects.DTOs.WorkTask.Response;
using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Repositories.Repositories;
using Services.Utils;

namespace Services.TaskServices
{
    public class TaskService : ITaskService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TaskService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<TaskDetailResponseModel> AddTask(string columnId, TaskAddRequestModel request)
        {
            if (!await _unitOfWork.Columns.IsExistAsync(c => c.Id.Equals(columnId)))
                throw new CustomException("Column Id not found");

            var existingTitles = (await _unitOfWork.Tasks.GetAsync(t => t.ColumnId.Equals(columnId) && t.Title.StartsWith(request.Title)))
                    .Select(t => t.Title).ToHashSet();
            request.Title = Util.GenerateUniqueTitle(request.Title, existingTitles);

            var tasks = await _unitOfWork.Tasks.GetAsync(t => t.ColumnId.Equals(columnId));
            int lastPostion = tasks.Count() - 1;

            var newTask = _mapper.Map<WorkTask>(request);
            newTask.ColumnId = columnId;
            newTask.Position = lastPostion + 1;

            await _unitOfWork.Tasks.AddAsync(newTask);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<TaskDetailResponseModel>(newTask);
        }

        public async Task UpdateTask(string taskId, TaskUpdateRequestModel request)
        {
            var task = await _unitOfWork.Tasks.SingleOrDefaultAsync(c => c.Id.Equals(taskId));

            if (task == null) throw new CustomException("Task Id not found");
            if (await _unitOfWork.Tasks.IsExistAsync(x => x.ColumnId.Equals(task.ColumnId) && x.Title.Equals(request.Title) && !x.Id.Equals(taskId)))
                throw new CustomException("Task name has existed in this Column");

            _mapper.Map(request, task);

            _unitOfWork.Tasks.Update(task);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteTask(string taskId)
        {
            var task = await _unitOfWork.Tasks.SingleOrDefaultAsync(c => c.Id.Equals(taskId));

            if (task == null) throw new CustomException("Task Id not found");

            var taskLabels = await _unitOfWork.TaskLabels.GetAsync(tl => tl.TaskId.Equals(taskId));
            var tasks = await _unitOfWork.Tasks.GetAsync(t => t.ColumnId.Equals(task.ColumnId) && !t.Id.Equals(taskId));
            foreach (var t in tasks)
            {
                if (t.Position > task.Position)
                {
                    t.Position--;
                }
            }

            _unitOfWork.TaskLabels.DeleteRange(taskLabels);
            _unitOfWork.Tasks.Delete(task);
            _unitOfWork.Tasks.UpdateRange(tasks);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAllTasks(string columnId)
        {
            var tasks = await _unitOfWork.Tasks.GetAsync(t => t.ColumnId.Equals(columnId));
            
            var taskIds = tasks.Select(t => t.Id).ToHashSet();
            var taskLabels = await _unitOfWork.TaskLabels.GetAsync(tl => taskIds.Contains(tl.TaskId));

            _unitOfWork.TaskLabels.DeleteRange(taskLabels);
            _unitOfWork.Tasks.DeleteRange(tasks);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task ToggleLabel(string taskId, string labelId)
        {
            var taskLabel = await _unitOfWork.TaskLabels.SingleOrDefaultAsync(tl => tl.TaskId.Equals(taskId) && tl.LabelId.Equals(labelId));

            if (taskLabel == null)
            {
                await _unitOfWork.TaskLabels.AddAsync(new TaskLabel
                {
                    TaskId = taskId,
                    LabelId = labelId
                });
            }
            else
            {
                _unitOfWork.TaskLabels.Delete(taskLabel);
            }
                
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task Reorder(TaskReorderRequestModel request)
        {
            var task = await _unitOfWork.Tasks.SingleOrDefaultAsync(t => t.Id.Equals(request.TaskId));
            if (task == null) throw new CustomException("Task not found");

            var sourceTasks = (await _unitOfWork.Tasks
                .GetAsync(t => t.ColumnId.Equals(request.SourceColumnId) && !t.Id.Equals(request.TaskId)))
                .OrderBy(t => t.Position)
                .ToList();

            var targetTasks = (await _unitOfWork.Tasks
                .GetAsync(t => t.ColumnId.Equals(request.TargetColumnId) && !t.Id.Equals(request.TaskId)))
                .OrderBy(t => t.Position)
                .ToList();

            targetTasks.Insert(request.TargetIndex, task);

            // Update columnId if moved across columns
            if (!request.SourceColumnId.Equals(request.TargetColumnId))
            {
                task.ColumnId = request.TargetColumnId;
            }

            // Re-assign positions
            for (int i = 0; i < sourceTasks.Count; i++)
                sourceTasks[i].Position = i;

            for (int i = 0; i < targetTasks.Count; i++)
                targetTasks[i].Position = i;

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
