using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Services.Infrastructure.Validations;

[AttributeUsage(AttributeTargets.Property |
                AttributeTargets.Field)]
public class RequiredListValue : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value == null) return false;
        var isList = value is IEnumerable<object>;
        if (!isList) return false;
        var castedList = (IEnumerable<object>)value;
        var iEnumerable = castedList.ToList();
        return iEnumerable.Any();
    }

    public override string FormatErrorMessage(string name)
    {
        return "List is either null or empty";
    }
}