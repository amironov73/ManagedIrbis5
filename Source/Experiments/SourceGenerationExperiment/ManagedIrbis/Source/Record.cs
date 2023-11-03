// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global

using System.Text;

namespace ManagedIrbis;

public class Record
{
    public string? Database { get; set; }

    public int Mfn { get; set; }

    public int Version { get; set; }

    public RecordStatus Status { get; set; }

    public MapFieldCollection Fields { get; }

    public Record()
    {
        Fields = new MapFieldCollection { Record = this };
    }

    public string? FM (int tag, char code)
    {
        foreach (var field in Fields)
        {
            if (field.Tag == tag)
            {
                return field.GetFirstSubFieldValue (code);
            }
        }

        return null;
    }

    public Record SetSubFieldValue (int tag, char code, string? value)
    {
        Field? target = null;
        foreach (var field in Fields)
        {
            if (field.Tag == tag)
            {
                target = field;
                break;
            }
        }

        if (target is null)
        {
            target = new Field { Tag = tag };
            Fields.Add (target);
        }

        target.SetSubFieldValue (code, value);

        return this;
    }

    public override string ToString()
    {
        var result = new StringBuilder();
        foreach (var field in Fields)
        {
            result.AppendLine (field.ToString());
        }

        return result.ToString();
    }
}
