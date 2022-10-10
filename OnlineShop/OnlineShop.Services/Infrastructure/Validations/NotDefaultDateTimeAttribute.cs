#region

using System.ComponentModel.DataAnnotations;

#endregion

namespace OnlineShop.Services.Infrastructure.Validations;

[AttributeUsage(
    AttributeTargets.Property | AttributeTargets.Field |
    AttributeTargets.Parameter)]
public class NotDefaultDateTimeAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        if (value is DateTime dateTime) return dateTime != default;

        return true;
    }
}