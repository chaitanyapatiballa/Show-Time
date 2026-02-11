using DBModels.Dto;
using FluentValidation;

namespace BusinessLogic.Validators
{
    public class PaymentDtoValidator : AbstractValidator<PaymentDto>
    {
        public PaymentDtoValidator()
        {
            RuleFor(x => x.Bookingid).GreaterThan(0);
            RuleFor(x => x.Amountpaid).GreaterThan(0);
            RuleFor(x => x.Paymentmethod).NotEmpty();
        }
    }
}
