// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* AsnHelper.cs -- полезные методы
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text;

#endregion

#nullable enable

namespace AM.Asn1;

/// <summary>
/// Полезные методы.
/// </summary>
/// <remarks>
/// Тип <see cref="AsnUtility"/> уже есть в сборке <c>AM.Core</c>.
/// </remarks>
public static class AsnHelper
{
    #region Public methods

    //=================================================

    private static readonly string[] _reservedWords =
    {
        "ABSENT",
        "ABSTRACT",
        "ALL",
        "APPLICATION",
        "AUTOMATIC",
        "BEGIN",
        "BIT",
        "BOOLEAN",
        "BY",
        "CHARACTER",
        "CHOICE",
        "CLASS",
        "COMPONENT",
        "COMPONENTS",
        "CONSTRAINED",
        "CONTAINING",
        "DEFAULT",
        "DEFINITIONS",
        "EMBEDDED",
        "ENCODED",
        "END",
        "ENUMERATED",
        "EXCEPT",
        "EXPLICIT",
        "EXTENSIBILITY",
        "EXTERNAL",
        "FALSE",
        "false",
        "FROM",
        "IDENTIFIER",
        "IMPLIED",
        "IMPLICIT",
        "IMPORTS",
        "INCLUDES",
        "INFINITY",
        "INSTANCE",
        "INTERSECTION",
        "MAX",
        "MIN",
        "MINUS",
        "NULL",
        "OBJECT",
        "OCTET",
        "OID",
        "OF",
        "OPTIONAL",
        "PATTERN",
        "PDV",
        "PLUS",
        "PRESENT",
        "PRIVATE",
        "REAL",
        "RELATIVE",
        "SET",
        "SEQUENCE",
        "SIZE",
        "STRING",
        "TAGS",
        "TRUE",
        "true",
        "UNION",
        "UNIQUE",
        "WITH",
    };

    /// <summary>
    /// Get array of reserved words.
    /// </summary>
    public static string[] GetReservedWords()
    {
        return _reservedWords;
    }

    //=================================================

    /// <summary>
    /// Преобразование коллекции узлов в текст.
    /// </summary>
    public static void NodesToText
        (
            StringBuilder builder,
            AsnNodeCollection collection
        )
    {
        Sure.NotNull (builder);
        Sure.NotNull (collection);

        foreach (var node in collection)
        {
            builder.Append (node);
        }
    }

    #endregion
}
