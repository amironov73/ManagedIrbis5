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

using AM;

using JetBrains.Annotations;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

using Spectre.Console;

#endregion

#nullable enable

namespace NamerCommon;

/// <summary>
/// Пара имен для файла: старое и новое.
/// </summary>
[PublicAPI]
public sealed class NamePair
    : ReactiveObject
{
    #region Properties

    /// <summary>
    /// Элемент отмечен?
    /// </summary>
    [Reactive]
    public bool IsChecked { get; set; }

    /// <summary>
    /// Ошибка?
    /// </summary>
    [Reactive]
    public string? ErrorMessage { get; set; }
    
    /// <summary>
    /// Старое имя файла.
    /// </summary>
    [Reactive]
    public required string Old { get; init; }

    /// <summary>
    /// Новое имя файла.
    /// </summary>
    [Reactive]
    public required string New { get; init; }

    /// <summary>
    /// Совпадают ли новое и старое имена?
    /// </summary>
    public bool IsSame => OperatingSystem.IsWindows()
        ? Old.SameString (New)
        : Old.SameStringSensitive (New);

    #endregion

    #region Public methods

    /// <summary>
    /// Рендеринг пар имен файлов.
    /// </summary>
    public void Render()
    {
        var same = Old.SameStringSensitive (New);
        var color = IsChecked
            ? same
                ? Color.Grey : Color.Green
            : Color.Grey;
        var style = new Style (color);

        AnsiConsole.Write (new Text (Old, style));
        AnsiConsole.Write (" => ");
        AnsiConsole.Write (new Text (New, style));
        if (!string.IsNullOrEmpty (ErrorMessage))
        {
            AnsiConsole.Write (' ');
            AnsiConsole.Write (new Text (ErrorMessage, new Style (Color.Red)));
        }

        AnsiConsole.WriteLine();
    }

    #endregion
}
