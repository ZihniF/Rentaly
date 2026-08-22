using FluentValidation.TestHelper;
using Rentaly.BusinessLayer.ValidationRules;
using Rentaly.DtoLayer.RentalDtos;
using Rentaly.EntityLayer.Entities;

namespace Rentaly.Tests;

public class ValidationRulesTests
{
    [Fact]
    public void CarValidator_AcceptsValidCar()
    {
        var result = new CarValidator().TestValidate(CreateValidCar());

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void CarValidator_RejectsInvalidVehicleFields()
    {
        var car = CreateValidCar();
        car.VIN = "SHORT";
        car.DailyPrice = 0;
        car.Kilometer = -1;
        car.ModelId = 0;

        var result = new CarValidator().TestValidate(car);

        result.ShouldHaveValidationErrorFor(x => x.VIN);
        result.ShouldHaveValidationErrorFor(x => x.DailyPrice);
        result.ShouldHaveValidationErrorFor(x => x.Kilometer);
        result.ShouldHaveValidationErrorFor(x => x.ModelId);
    }

    [Fact]
    public void CreateBookingValidator_AcceptsFutureBookingWithoutIdentityNumber()
    {
        var booking = CreateValidBooking();
        booking.IdentityNumber = null;

        var result = new CreateBookingValidator().TestValidate(booking);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void CreateBookingValidator_RejectsReturnBeforePickupAndInvalidIdentityNumber()
    {
        var booking = CreateValidBooking();
        booking.ReturnDate = booking.PickupDate.AddMinutes(-1);
        booking.IdentityNumber = "123";

        var result = new CreateBookingValidator().TestValidate(booking);

        result.ShouldHaveValidationErrorFor(x => x.ReturnDate);
        result.ShouldHaveValidationErrorFor(x => x.IdentityNumber);
    }

    private static Car CreateValidCar() => new()
    {
        PlateNumber = "34ABC123",
        VIN = "WVWZZZ1JZXW000001",
        BrandId = 1,
        ModelId = 1,
        CategoryId = 1,
        BranchId = 1,
        Year = DateTime.Now.Year,
        Kilometer = 15_000,
        DailyPrice = 1_500,
        DepositAmount = 5_000,
        ImageUrl = "/images/car.png",
        SeatCount = 5,
        LuggageCount = 2,
        FuelType = "Benzin"
    };

    private static CreateBookingDto CreateValidBooking() => new()
    {
        CarId = 1,
        PickupBranchId = 1,
        ReturnBranchId = 1,
        PickupDate = DateTime.Now.AddDays(2),
        ReturnDate = DateTime.Now.AddDays(5),
        Name = "Selin",
        Surname = "Yılmaz",
        Email = "selin@example.com",
        Phone = "5551112233"
    };
}
