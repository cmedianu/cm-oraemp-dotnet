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
            .MaximumLength(30).WithMessage("Maximum length is 30")
            .MinimumLength(2).WithMessage("Minimum Legth is 2");
    }
}