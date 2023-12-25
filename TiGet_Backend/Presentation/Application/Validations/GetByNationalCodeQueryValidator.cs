using FluentValidation;
using Rhazes.Services.Identity.API.Application.Queries;

namespace Rhazes.Services.Identity.API.Application.Validations
{
    public class GetByNationalCodeQueryValidator : AbstractValidator<GetByNationalCodeQuery>
    {
        public GetByNationalCodeQueryValidator()
        {
            RuleFor(c => c.NationalCode).NotEmpty().WithMessage("NATIONAL_CODE_REQUIRED")
                //.Must(x => ValidationHelper.IsValidNationalCode(x)).When(x => x.NationalCode.Length == 10).WithMessage("NATIONAL_CODE_IS_INVALID")
                ;
        }

        
    }
}