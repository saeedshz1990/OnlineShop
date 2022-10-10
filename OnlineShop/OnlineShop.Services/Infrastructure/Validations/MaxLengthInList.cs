using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Services.Infrastructure.Validations;

[AttributeUsage(AttributeTargets.Property |
                AttributeTargets.Field)]
public class MaxLengthInListAttribute : ValidationAttribute
{
    private readonly int _length;

    public MaxLengthInListAttribute(int maximumLength)
    {
        _length = maximumLength;
    }

    public override bool IsValid(object? value)
    {
        if (value == null) return false;

        var isList = value is IEnumerable<string>;

        if (!isList) return false;

        var castedList = (IEnumerable<string>)value;
        var stringList = castedList.ToList();

        foreach (var item in stringList)
        {
            if (item.Length > _length)
                return false;
        }

        return true;
    }

    public override string FormatErrorMessage(string name)
    {
        return $"The field {name} must be a string or " +
            $"array type with a maximum length of '{_length}'";
    }
}