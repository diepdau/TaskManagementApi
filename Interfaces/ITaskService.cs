using TaskManagementApi.Models;

namespace TaskManagementApi.Interfaces
{
    public interface ITaskService
    {
        List<Models.Task> GetAllTasks();
        Models.Task GetTaskById(int id);
        void AddTask(Models.Task task);
        void UpdateTask(Models.Task task);
        void DeleteTask(int id);
    }
}
