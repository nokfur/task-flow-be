using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.DTOs.WorkTask.Request;
using BusinessObjects.DTOs.WorkTask.Response;

namespace Services.TaskServices
{
    public interface ITaskService
    {
        Task<TaskDetailResponse> AddTask(string columnId, TaskAddRequest request);
        Task UpdateTask(string taskId, TaskUpdateRequest request);
        Task DeleteTask(string taskId);
        Task DeleteAllTasks(string columnId);
        Task ToggleLabel(string taskId, string labelId);
        Task Reorder(TaskReorderRequest request);
    }
}
