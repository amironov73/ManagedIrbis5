// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* Grammar.cs -- грамматика языка
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Kotik;

/// <summary>
/// Грамматика языка.
/// </summary>
public static class Grammar
{
    #region Public methods and properties

    /// <summary>
    /// Разбор литералов.
    /// </summary>
    public static readonly LiteralParser Literal = new ();

    /// <summary>
    /// Разбор перечисленных терминов.
    /// </summary>
    public static TermParser Term (params string[] terms) => new (terms);

    /// <summary>
    /// Разбор зарезервированного слова.
    /// </summary>
    public static ReservedWordParser Reserved (string word) => new (word);

    #endregion
}
