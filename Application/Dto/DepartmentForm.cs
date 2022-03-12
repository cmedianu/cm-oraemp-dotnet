using FluentValidation;

namespace OraEmp.Application.Dto;

public class DepartmentForm
{
    public decimal DepartmentId { get; set; }
    public string? DepartmentName { get; set; }
}

public class DepartmentValidator :
    AbstractValidator<DepartmentForm>
{
    public DepartmentValidator()
    {
        RuleFor(x => x.DepartmentName)
            .NotEmpty().WithMessage("Please enter a name")
            .MaximumLength(4).WithMessage("Max 4 chars");

    }
}