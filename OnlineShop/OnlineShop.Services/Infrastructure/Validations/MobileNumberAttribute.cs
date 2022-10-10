using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace OnlineShop.Services.Infrastructure.Validations;

public class MobileNumberAttribute : ValidationAttribute
{
    public override bool IsValid(object? mobileNumberObject)
    {
        if (mobileNumberObject == null) return true;
        var mobileNumber = mobileNumberObject.ToString()?.TrimStart('0');
        var number = $"0{mobileNumber}";
        var pattern =
            new Regex(@"^(09)([0|1|2|3][1-9]{1}[0-9]{3}[0-9]{4})$");
        return pattern.IsMatch(number);
    }
}