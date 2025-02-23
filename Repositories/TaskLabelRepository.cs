﻿using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TaskManagementApi.Models;

namespace TaskManagementApi.Repositories
{
    public class TaskLabelRepository : GenericRepository<TaskLabel>
    {
        public TaskLabelRepository(TaskManagementDbContext context) : base(context) { }
        public bool Exists(int taskId, int labelId)
        {
            return _dbSet.Any(tl => tl.TaskId == taskId && tl.LabelId == labelId);
        }
        public void RemoveLabel(int taskId, int labelId)
        {
            var taskLabel = _dbSet.FirstOrDefault(tl => tl.TaskId == taskId && tl.LabelId == labelId);
            if (taskLabel != null)
            {
                _dbSet.Remove(taskLabel);
                _context.SaveChanges();
            }
        }
    }
}
