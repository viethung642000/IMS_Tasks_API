using BE.Data.Contexts;
using BE.Data.Models;
using BE.Services.Repository;
using EF.Core.Repository.Interface.Manager;
using EF.Core.Repository.Manager;

namespace BE.Services.Managers
{
    public interface ITasksManager : ICommonManager<Tasks>
    {
    }

    public class TasksManager : CommonManager<Tasks>, ITasksManager
    {
        public TasksManager(AppDbContext appDbContext) : base(new TasksRepository(appDbContext))
        {
        }
    }
}
