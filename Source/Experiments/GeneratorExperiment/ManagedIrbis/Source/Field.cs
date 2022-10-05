// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Global
// ReSharper disable UnusedParameter.Local
// ReSharper disable UnusedType.Global

#region Using directives

using System.Collections.Generic;
using System.Text;

#endregion

#nullable enable

namespace ManagedIrbis;

public sealed class Field
{
    public int Tag { get; set; }
    public List<SubField> SubFields { get; } = new ();

    public Field()
    {
        // пустое тело конструктора
    }

    public Field(int tag)
    {
        Tag = tag;
    }

    public string? GetFirstSubFieldValue
        (
            char code
        )
    {
        foreach (var subField in SubFields)
        {
            if (subField.Code == code)
            {
                return subField.Value;
            }
        }

        return null;
    }

    public void SetFirstSubFieldValue
        (
            char code,
            string? value
        )
    {
        foreach (var subField in SubFields)
        {
            if (subField.Code == code)
            {
                subField.Value = value;
                return;
            }
        }

        var newSubField = new SubField (code, value);
        SubFields.Add (newSubField);
    }

    public override string ToString()
    {
        var result = new StringBuilder();
        result.Append ($"{Tag}#");
        foreach (var subField in SubFields)
        {
            result.Append(subField);
        }

        return result.ToString();
    }
}

