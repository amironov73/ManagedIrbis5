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

using System.Text;

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
    /// Есть ошибка?
    /// </summary>
    public bool HasError => !string.IsNullOrEmpty (ErrorMessage);

    /// <summary>
    /// Совпадают ли новое и старое имена?
    /// </summary>
    public bool IsSame => Old.SameStringSensitive (New);

    #endregion

    #region Private member

    private static bool GoodChar (char chr) => chr is >= '0' and <= '9'
        or >= 'A' and <= 'Z' or >= 'a' and <= 'z' or '=' or '-'
        or '_' or '!' or '(' or ')' or '[' or ']' or '.';

    #endregion

    #region Public methods

    /// <summary>
    /// Валидация имени файла.
    /// </summary>
    public bool ValidateNewName()
    {
        if (string.IsNullOrEmpty (New))
        {
            return false;
        }

        foreach (var chr in New)
        {
            if (!GoodChar (chr))
            {
                return false;
            }
        }
        
        return true;
    }

    /// <summary>
    /// Рендеринг пар имен файлов.
    /// </summary>
    public void Render()
    {
        var color = IsChecked
            ? IsSame
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

    #region Object members

    public override string ToString()
    {
        var builder = new StringBuilder();
        var mark = IsChecked ? "[x]" : "[ ]";
        builder.Append ($"{Old} => {New} {mark}");
        if (!string.IsNullOrEmpty (ErrorMessage))
        {
            builder.Append ($" {ErrorMessage}");
        }

        return builder.ToString();
    }

    #endregion
}
