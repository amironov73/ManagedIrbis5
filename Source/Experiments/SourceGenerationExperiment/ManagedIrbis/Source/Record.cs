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

    public string[] FMA (int tag, char code)
    {
        var result = new List<string>();
        foreach (var field in Fields)
        {
            if (field.Tag == tag)
            {
                var value = field.GetFirstSubFieldValue (code);
                if (!string.IsNullOrEmpty (value))
                {
                    result.Add (value);
                }
            }
        }

        return result.ToArray();
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

    /// <summary>
    /// Получение заданного повторения поля с указанной меткой.
    /// </summary>
    public Field? GetField
        (
            int tag,
            int occurrence = 0
        )
    {
        foreach (var field in Fields)
        {
            if (field.Tag == tag)
            {
                if (occurrence == 0)
                {
                    return field;
                }

                --occurrence;
            }
        }

        return null;
    }

    /// <summary>
    /// Получение заданного повторения поля с указанной меткой.
    /// </summary>
    public List<Field> GetFields
        (
            int tag
        )
    {
        var result = new List<Field>();
        foreach (var field in Fields)
        {
            if (field.Tag == tag)
            {
                result.Add (field);
            }
        }

        return result;
    }

    public Record SetField
        (
            int tag,
            string? newValue
        )
    {
        // TODO implement

        return this;
    }

    public Record SetField
        (
            int tag,
            Field? newValue
        )
    {
        // TODO implement

        return this;
    }

    public Record SetField
        (
            int tag,
            IList<Field> newValues
        )
    {
        // TODO implement

        return this;
    }

    public Record SetField
        (
            int tag,
            IList<string> newValues
        )
    {
        // TODO implement

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
