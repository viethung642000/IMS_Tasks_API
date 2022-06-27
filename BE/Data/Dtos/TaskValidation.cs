using FluentValidation;
namespace BE.Data.Dtos
{
    public class TaskValidation : AbstractValidator<TaskDto>
    {
        public TaskValidation()
        {
            RuleFor(t => t.taskName).NotEmpty().NotNull().WithMessage("Task name not empty!!!");
            RuleFor(d => d.description).MaximumLength(500).WithMessage("Description don't more than 500 characters!!!");
            RuleFor(s => s.status).IsInEnum().WithMessage("Value status don't valid");
            RuleFor(t => t.tags).IsInEnum().WithMessage("Value tag don't valid");
            RuleFor(a => a.assignee).NotEmpty().NotNull().WithMessage("Assignee don't empty!!!");
            RuleFor(s => s.startTaskDate).NotEmpty().NotNull().WithMessage("Date task start don't empty!!!");
            RuleFor(s => s.endTaskDate).NotEmpty().NotNull().WithMessage("Date task end don't empty!!!");
            RuleFor(i => i.idProject).NotEmpty().NotNull().WithMessage("Project don't empty!!!");
        }

    }
}
