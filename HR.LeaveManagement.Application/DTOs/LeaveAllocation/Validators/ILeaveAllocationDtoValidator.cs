using FluentValidation;
using HR.LeaveManagement.Application.Contracts.Persistence;

namespace HR.LeaveManagement.Application.DTOs.LeaveAllocation.Validators
{
    public class ILeaveAllocationDtoValidator : AbstractValidator<ILeaveAllocationDto>
    {
        public ILeaveAllocationDtoValidator(ILeaveTypeRepository leaveTypeRepository)
        {
            RuleFor(p => p.NumberOfDays)
                .GreaterThan(0).WithMessage("{PropertyName} should be greater than {ComparisonValue}");

            RuleFor(p => p.LeaveTypeId)
               .GreaterThan(0).WithMessage("{PropertyName} should be greater than {ComparisonValue}")
               .MustAsync(async (id, token) =>
               {
                   var leaveTypeExists = await leaveTypeRepository.Exists(id);
                   return !leaveTypeExists;
               }).WithMessage("{PropertyName} does not exist");

            RuleFor(p => p.Period)
                .GreaterThan(0).WithMessage("{PropertyName} should be greater than {ComparisonValue}");
        }
    }
}
