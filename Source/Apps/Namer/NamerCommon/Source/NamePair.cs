// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement

/* NamePair.cs -- пара имен для файла: старое и новое
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using JetBrains.Annotations;

#endregion

#nullable enable

namespace NamerCommon;

/// <summary>
/// Пара имен для файла: старое и новое.
/// </summary>
[PublicAPI]
public sealed class NamePair
{
    #region Properties

    /// <summary>
    /// Старое имя файла.
    /// </summary>
    public required string Old { get; init; }

    /// <summary>
    /// Новое имя файла.
    /// </summary>
    public required string New { get; init; }

    #endregion

    #region Public methods

    /// <summary>
    /// Рендеринг пар имен файлов.
    /// </summary>
    public static void Render
        (
            IEnumerable<NamePair> pairs
        )
    {
        foreach (var pair in pairs)
        {
            Console.WriteLine ($"{pair.Old} => {pair.New}");
        }
    }

    #endregion
}
