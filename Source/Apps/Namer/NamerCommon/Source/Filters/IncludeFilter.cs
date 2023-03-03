// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* IncludeFilter.cs -- включение файла при соответствии регулярному выражению
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text.RegularExpressions;

using AM;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace NamerCommon;

/// <summary>
/// Включение файла в обработку при соответствии
/// регулярному выражению.
/// </summary>
[PublicAPI]
public sealed class IncludeFilter
    : RegexFilter
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public IncludeFilter
        (
            string specification
        )
        : base (specification)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public IncludeFilter
        (
            Regex regex
        )
        : base (regex)
    {
        // пустое тело конструктора
    }

    #endregion

    #region FileFilter members

    /// <inheritdoc cref="FileFilter.CanPass"/>
    public override bool CanPass
        (
            FileInfo fileInfo
        )
    {
        Sure.NotNull (fileInfo);

        return IsMatch (fileInfo.Name);
    }

    #endregion
}
