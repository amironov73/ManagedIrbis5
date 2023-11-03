// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

using System.Text;

namespace ManagedIrbis;

public class Field
{
    public int Tag { get; set; }

    public string? Value { get; set; }

    public SubFieldCollection Subfields { get; }

    public Field()
    {
        Subfields = new SubFieldCollection() { Field = this };
    }

    public Field
        (
            int tag,
            string? value
        )
        : this()
    {
        Tag = tag;
        Value = value;
    }

    public string? GetFirstSubFieldValue
        (
            char code
        )
    {
        foreach (var subField in Subfields)
        {
            if (char.ToUpperInvariant (subField.Code) == char.ToUpperInvariant (code))
            {
                return subField.Value;
            }
        }

        return default;
    }

    public void SetSubFieldValue
        (
            char code,
            string? value
        )
    {
        SubField? target = null;

        foreach (var subField in Subfields)
        {
            if (char.ToUpperInvariant (subField.Code) == char.ToUpperInvariant (code))
            {
                target = subField;
                break;
            }
        }

        if (target is null)
        {
            target = new SubField { Code = code };
            Subfields.Add (target);
        }

        target.Value = value;
    }

    public override string ToString()
    {
        var result = new StringBuilder();
        result.Append ($"{Tag}#");
        foreach (var subfield in Subfields)
        {
            result.Append (subfield);
        }

        return result.ToString();
    }
}
