// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ChimeraUtility.cs -- полезные методы для Chimera
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;

using AM;
using AM.Collections;
using AM.Text;

using JetBrains.Annotations;

#endregion

namespace ManagedIrbis.Scripting.Barsik;

/// <summary>
/// Полезные методы для Chimera.
/// </summary>
[PublicAPI]
public static class ChimeraUtility
{
    #region Public methods

    /// <summary>
    /// Формирование строки с ФИО автора.
    /// </summary>
    public static string GetAuthor
        (
            Field field
        )
    {
        Sure.NotNull (field);

        Span<char> characters = stackalloc char[256];
        var builder = new ValueStringBuilder (characters);
        builder.Append (field.GetSubFieldValue('a'));
        if (field.HaveSubField ('1'))
        {
            builder.Append ($" ({field.GetSubFieldValue('1')})");
        }

        if (field.HaveSubField ('g'))
        {
            builder.Append (',');
            builder.Append (' ');
            builder.Append (field.GetSubFieldValue('g'));
        }
        else if (field.HaveSubField ('b'))
        {
            builder.Append (',');
            builder.Append (' ');
            builder.Append (field.GetSubFieldValue('b'));
        }

        return builder.ToString();
    }

    /// <summary>
    /// Получение массива ФИО авторов.
    /// </summary>
    public static string[] GetAuthors
        (
            IEnumerable<Field> fields
        )
    {
        Sure.NotNull (fields);

        var list = new ValueList<string>();
        foreach (var field in fields)
        {
            list.Append (GetAuthor (field));
        }

        return list.ToArray();
    }

    /// <summary>
    /// Получение массива ФИО авторов.
    /// </summary>
    public static string[] GetAuthors
        (
            Record record,
            int tag
        )
    {
        Sure.NotNull (record);
        Sure.Positive (tag);

        var fields = record.EnumerateField (tag);
        var result = GetAuthors (fields);

        return result;
    }

    /// <summary>
    /// Получение массива ФИО авторов.
    /// </summary>
    public static string[] GetAuthors
        (
            Record record
        )
    {
        Sure.NotNull (record);

        var fields = record.EnumerateField (700)
            .Concat (record.EnumerateField (701))
            .Concat (record.EnumerateField (961));
        var result = GetAuthors (fields);

        return result;
    }

    #endregion
}
