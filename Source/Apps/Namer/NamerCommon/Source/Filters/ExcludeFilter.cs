// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ExcludeFilter.cs -- исключение файла при соответствии регулярному выражению
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
/// Исключение файла из обработки при соответствии регулярному выражению.
/// </summary>
[PublicAPI]
public sealed class ExcludeFilter
    : RegexFilter
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ExcludeFilter
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
    public ExcludeFilter
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

        return !IsMatch (fileInfo.Name);
    }

    #endregion
}
