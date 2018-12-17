using FluentValidation;
using Rest.Model.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rest.Models.Validators
{
    /// <summary>
    /// Book Validator using fluent validation
    /// It will Validate book properties where it is called as per the validation rules defined below
    /// </summary>
    public class BookValidator : AbstractValidator<Book>
    {
        /// <summary>
        /// Book Validator constructor
        /// </summary>
        public BookValidator()
        {
            RuleFor(x => x.Name).NotNull().WithMessage("Please provide book name").Length(10, 250).WithMessage("Length of book name should be 10 to 250 characters");
            RuleFor(x => x.NumberOfPages).NotNull().WithMessage("Please provide number of pages").GreaterThan(50).WithMessage("Number Of Pages should be greater than 50");
            RuleFor(x => x.DateOfPublication).NotNull().WithMessage("Please provide publish date").LessThan(DateTime.UtcNow);
            RuleFor(x => x.Authors).NotEmpty().WithMessage("Please provide atleast one author");
        }
    }
}
