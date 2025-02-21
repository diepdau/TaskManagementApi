using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TaskManagementApi.Models;

namespace TaskManagementApi.Repositories
{
    public class TaskLabelRepository: GenericRepository<TaskLabel>
    {
       
        public TaskLabelRepository(TaskManagementDbContext context) : base(context)
        {
        }
        //public IEnumerable<TaskLabel> GetAll()
        //{
        //    return _dbSet.Include(tl => tl.Task)  // Include Task data
        //                 .Include(tl => tl.Label)  // Include Label data
        //                 .ToList();
        //}
        

    }
}
