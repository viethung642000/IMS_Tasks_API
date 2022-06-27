using BE.Data.Enums;

namespace BE.Data.Dtos
{
    public class TaskEditDto
    {
        public int idParent { get; set; }
        public string taskName { get; set; }
        public string? description { get; set; }
        public Status status { get; set; }
        public Tags tags { get; set; }
        public int assignee { get; set; }
        public string? milestone { get; set; }
        public DateTime? startTaskDate { get; set; }
        public DateTime? endTaskDate { get; set; }
        public int idProject { get; set; }
    }
}
