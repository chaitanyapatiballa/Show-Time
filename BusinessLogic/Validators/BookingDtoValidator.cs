using Booking_Service.Controllers;
using FluentValidation;

namespace BusinessLogic.Validators
{
    public class BookingDtoValidator : AbstractValidator<BookingDto>
    {
        public BookingDtoValidator()
        {
            RuleFor(x => x.Showinstanceid).GreaterThan(0);
            RuleFor(x => x.Seatid).GreaterThan(0);
        }
    }
}
