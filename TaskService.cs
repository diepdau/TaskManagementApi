using System.Text.Json;
using TaskManagementApi.Models;
using TaskManagementApi.Services;

namespace TaskManagementApi
{
    public class TaskService : ITaskService
    {
        private readonly List<TaskItem> _tasks;
        private const string FilePath = @"D:\task.json";

        public TaskService()
        {
            _tasks = LoadTasksFromFile().Result;
        }

        public List<TaskItem> GetAllTasks() => _tasks;

        public TaskItem GetTaskById(int id) => _tasks.FirstOrDefault(t => t.Id == id);

        public void AddTask(TaskItem task)
        {
            task.Id = _tasks.Count + 1;
            _tasks.Add(task);
            SaveTasksToFile();
        }

        public void UpdateTask(TaskItem task)
        {
            var existingTask = GetTaskById(task.Id);
            if (existingTask != null)
            {
                existingTask.Title = task.Title;
                existingTask.Completed = task.Completed;
                SaveTasksToFile();
            }
        }

        public void DeleteTask(int id)
        {
            _tasks.RemoveAll(t => t.Id == id);
            SaveTasksToFile();
        }

        private async void SaveTasksToFile()
        {
            string json = JsonSerializer.Serialize(_tasks, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(FilePath, json);
        }

        private async Task<List<TaskItem>> LoadTasksFromFile()
        {
            if (!File.Exists(FilePath)) return new List<TaskItem>();
            string json = await File.ReadAllTextAsync(FilePath);
            return JsonSerializer.Deserialize<List<TaskItem>>(json) ?? new List<TaskItem>();
        }
    }
}