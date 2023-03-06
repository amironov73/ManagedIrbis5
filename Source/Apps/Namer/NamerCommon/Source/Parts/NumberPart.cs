// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* NumberPart.cs -- число в составе имени файла
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text.RegularExpressions;

using AM;
using AM.Collections;
using AM.Parameters;
using AM.Text;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace NamerCommon;

/// <summary>
/// Число в составе имени файла.
/// </summary>
[PublicAPI]
public sealed class NumberPart
    : NamePart
{
    #region Properties

    /// <inheritdoc cref="NamePart.Designation"/>
    public override string Designation => "number";

    /// <inheritdoc cref="NamePart.Title"/>
    public override string Title => "Число";

    /// <summary>
    /// Выводить текст до совпадения.
    /// </summary>
    public bool Before { get; set; }

    /// <summary>
    /// Выводить текст после совпадения.
    /// </summary>
    public bool After { get; set; }

    /// <summary>
    /// Добавка.
    /// </summary>
    public int Delta { get; set; }

    /// <summary>
    /// Индекс группы.
    /// </summary>
    public int Index { get; set; }

    /// <summary>
    /// Обязательно?
    /// </summary>
    public bool Required { get; set; }

    /// <summary>
    /// Ширина группы.
    /// </summary>
    public int Width { get; set; }

    #endregion

    #region Private members

    private readonly Regex _regex = new (@"\d+");

    #endregion

    #region NamePart members

    /// <inheritdoc cref="NamePart.Parse"/>
    public override NamePart Parse
        (
            string text
        )
    {
        Sure.NotNull (text);

        var parameters = ParameterUtility.SimpleParseString (text);
        var result = new NumberPart();
        foreach (var parameter in parameters)
        {
            switch (parameter.Name)
            {
                case "after":
                    result.After = true;
                    break;

                case "before":
                    result.Before = true;
                    break;

                case "delta":
                    result.Delta = parameter.Value!.ParseInt32();
                    break;

                case "index":
                    result.Index = parameter.Value!.ParseInt32();
                    break;

                case "require":
                case "required":
                    result.Required = true;
                    break;

                case "width":
                    result.Width = parameter.Value!.ParseInt32();
                    break;

                default:
                    throw new ApplicationException();
            }
        }

        return result;
    }

    /// <inheritdoc cref="NamePart.Render"/>
    public override string Render
        (
            NamingContext context,
            FileInfo fileInfo
        )
    {
        Sure.NotNull (context);
        Sure.NotNull (fileInfo);

        var matches = _regex.Matches (fileInfo.Name);
        if (matches.IsNullOrEmpty())
        {
            if (Required)
            {
                throw new ApplicationException();
            }

            return EmptyResult();
        }

        Match match;
        if (Index > 0)
        {
            var temp = matches!.SafeAt (Index);
            if (temp is null)
            {
                return EmptyResult();
            }

            match = temp;
        }
        else
        {
            match = matches.Last();
        }

        if (!match.Success)
        {
            return EmptyResult();
        }

        var value = match.Value.ParseInt32() + Delta;
        var result = StringBuilderPool.Shared.Get();
        if (Before)
        {
            result.Append (fileInfo.Name.AsSpan (0, match.Index));
        }

        if (Width > 0)
        {
            var format = new string ('0', Width);
            result.Append (value.ToInvariantString (format));
        }
        else
        {
            result.Append (value.ToInvariantString());
        }

        if (After)
        {
            result.Append (fileInfo.Name[(match.Index + match.Length)..]);
        }

        return result.ReturnShared();

        string EmptyResult() => Before || After ? fileInfo.Name : string.Empty;
    }

    #endregion
}
