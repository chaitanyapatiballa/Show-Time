using DBModels.Dto;
using FluentValidation;

namespace BusinessLogic.Validators
{
    public class MovieDtoValidator : AbstractValidator<MovieDto>
    {
        public MovieDtoValidator()
        {
            RuleFor(x => x.Title).NotEmpty();
            RuleFor(x => x.Duration).NotEmpty();
            RuleFor(x => x.Genre).NotEmpty();
        }
    }
}
