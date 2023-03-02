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
    /// Индекс группы.
    /// </summary>
    public int Index { get; set; }

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

        var parameters = ParameterUtility.ParseString (text);
        var result = new NumberPart();
        foreach (var parameter in parameters)
        {
            parameter.Verify (true);
            switch (parameter.Name)
            {
                case "index":
                    result.Index = parameter.Value!.ParseInt32();
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

        Match match;
        if (Index > 0)
        {
            var matches = _regex.Matches (fileInfo.Name);
            if (matches.IsNullOrEmpty())
            {
                return string.Empty;
            }

            var temp = matches!.SafeAt (Index);
            if (temp is null)
            {
                return string.Empty;
            }

            match = temp;
        }
        else
        {
            match = _regex.Match (fileInfo.Name);
        }

        if (!match.Success)
        {
            return string.Empty;
        }

        if (Width > 0)
        {
            var value = match.Value.ParseInt32();
            var format = new string ('0', Width);
            return value.ToInvariantString (format);
        }

        return match.Value;
    }

    #endregion
}
