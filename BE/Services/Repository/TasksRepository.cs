using BE.Data.Contexts;
using BE.Data.Models;
using EF.Core.Repository.Interface.Repository;
using EF.Core.Repository.Repository;

namespace BE.Services.Repository
{
    public interface ITasksRepository : ICommonRepository<Tasks>
    {
    }

    public class TasksRepository : CommonRepository<Tasks>, ITasksRepository
    {
        public TasksRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }
    }
}
