using BE.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace BE.Data.Models
{
    public class Tasks { 
        public int idTask { get; set; }
        public int idParent { get; set; }
        public string taskName { get; set; }
        public string? description { get; set; }
        public bool isDeleted { get; set; }
        public Status status { get; set; }
        public Tags tag { get; set; }
        public int assignee { get; set; }
        public string? milestone { get; set; }
        public DateTime? startTaskDate { get; set; }
        public DateTime? endTaskDate { get; set; }
        public DateTime createTaskDate { get; set; }
        public int createUser { get; set; }
        public int idProject { get; set; }
    }
}
