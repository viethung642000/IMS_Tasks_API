using BE.Data.Contexts;
using BE.Data.Models;
using BE.Services.Repository;
using EF.Core.Repository.Interface.Manager;
using EF.Core.Repository.Manager;

namespace BE.Services.Managers
{
    public interface ITasksManager : ICommonManager<Tasks>
    {
        Task<ICollection<Tasks>> GetAllTasksAsync();
        Task<Tasks?> getTaskByIdAsync(int id);
        Task<ICollection<Tasks>> getTaskChild(int id);
    }

    public class TasksManager : CommonManager<Tasks>, ITasksManager
    {
        public TasksManager(AppDbContext appDbContext) : base(new TasksRepository(appDbContext))
        {
        }

        public async Task<ICollection<Tasks>> GetAllTasksAsync()
        {
            return await GetAllAsync();
        }

        public async Task<Tasks?> getTaskByIdAsync(int id)
        {
            return await GetFirstOrDefaultAsync(t => t.idTask == id);
        }
        public async Task<ICollection<Tasks>> getTaskChild(int id)
        {
            return await GetAsync(t => t.idParent == id);
        }
    }

}
