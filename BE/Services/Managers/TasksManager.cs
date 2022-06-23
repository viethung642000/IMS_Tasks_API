using BE.Data.Contexts;
using BE.Data.Dtos;
using BE.Data.Models;
using BE.Services.Repository;
using EF.Core.Repository.Interface.Manager;
using EF.Core.Repository.Manager;

namespace BE.Services.Managers
{
    public interface ITasksManager : ICommonManager<Tasks>
    {
        Task<ICollection<Tasks>> GetAllTasksAsync();
        Task AddTask(TaskDto task); 
    }

    public class TasksManager : CommonManager<Tasks>, ITasksManager
    {
        public TasksManager(AppDbContext appDbContext) : base(new TasksRepository(appDbContext))
        {
        }

        public async Task AddTask(TaskDto task)
        {
            var result = new Tasks
            {
                idParent = task.idParent,
                taskName = task.taskName,
                description = task.description,
                assignee = task.assignee,
                milestone  = task.milestone,
                startTaskDate = task.startTaskDate,
                endTaskDate = task.endTaskDate,
                createUser = task.createUser,
                idProject = task.idProject
            };
            await AddAsync(result);
        }

        public async Task<ICollection<Tasks>> GetAllTasksAsync()
        {
            return await GetAllAsync();
        }
    }

}
