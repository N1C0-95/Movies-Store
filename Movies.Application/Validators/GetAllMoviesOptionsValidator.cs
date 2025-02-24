using FluentValidation;
using Movies.Application.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Application.Validators
{
    public class GetAllMoviesOptionsValidator:AbstractValidator<GetAllMoviesOptions>
    {
        public GetAllMoviesOptionsValidator()
        {
            RuleFor(x => x.YearOfRelease).LessThanOrEqualTo(DateTime.UtcNow.Year);

            RuleFor(x => x.Page)
                .GreaterThanOrEqualTo(1);


            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 25)
                .WithMessage("you can get between 1 and 25 movies per page");
        }
    }
}
