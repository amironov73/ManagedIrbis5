// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global

/* TagComparer.cs -- умеет сравнивать теги
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

namespace AM.StableDiffusion.PromptEngineering;

/// <summary>
/// Умеет сравнивать теги.
/// </summary>
[PublicAPI]
public static class TagComparer
{
    #region Properties

    /// <summary>
    /// Общий экземпляр (с возможностью замены).
    /// </summary>
    public static StringComparer Instance { get; set; }
        = StringComparer.InvariantCultureIgnoreCase;

    #endregion

    #region Public methods

    /// <summary>
    /// Сравнение двух строк.
    /// </summary>
    public static int Compare (string? left, string? right)
        => Instance.Compare (left, right);

    /// <summary>
    /// Сравнение двух тегов.
    /// </summary>
    public static int Compare (TagInfo left, TagInfo right)
        => Instance.Compare (left.Title, right.Title);

    /// <summary>
    /// Две строки равны?
    /// </summary>
    public static bool IsSame (string? left, string? right)
        => Compare (left, right) == 0;

    /// <summary>
    /// Два тега равны?
    /// </summary>
    public static bool IsSame (TagInfo left, TagInfo right)
        => Compare (left, right) == 0;

    #endregion
}
