using System.Text.Json;
using TaskManagementApi.Models;
using TaskManagementApi.Interfaces;

namespace TaskManagementApi
{
    public class TaskService : ITaskService
    {
        private readonly List<Models.Task> _tasks;
        private const string FilePath = @"D:\task.json";

        public TaskService()
        {
            _tasks = LoadTasksFromFile().Result;
        }

        public List<Models.Task> GetAllTasks() => _tasks;

        public Models.Task? GetTaskById(int id) => _tasks.FirstOrDefault(t => t.Id == id);

        public void AddTask(Models.Task task)
        {
            task.Id = _tasks.Count + 1;
            _tasks.Add(task);
            SaveTasksToFile();
        }

        public void UpdateTask(Models.Task task)
        {
            var existingTask = GetTaskById(task.Id);
            if (existingTask != null)
            {
                existingTask.Title = task.Title;
                //existingTask.Description = task.Completed;
                //Sai loai
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

        private async Task<List<Models.Task>> LoadTasksFromFile()
        {
            if (!File.Exists(FilePath)) return new List<Models.Task>();
            string json = await File.ReadAllTextAsync(FilePath);
            return JsonSerializer.Deserialize<List<Models.Task>>(json) ?? new List<Models.Task>();
        }
    }
}