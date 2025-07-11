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
        Task<TaskDetailResponseModel> AddTask(string columnId, TaskAddRequestModel request);
        Task UpdateTask(string taskId, TaskUpdateRequestModel request);
        Task DeleteTask(string taskId);
        Task DeleteAllTasks(string columnId);
        Task ToggleLabel(string taskId, string labelId);
        Task Reorder(TaskReorderRequestModel request);
    }
}
