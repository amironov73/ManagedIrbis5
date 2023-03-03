// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ReplacePart.cs -- замена части текста в имени файла
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;
using AM.Parameters;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace NamerCommon;

/// <summary>
/// Замена части текста в имени файла.
/// </summary>
[PublicAPI]
public sealed class ReplacePart
    : NamePart
{
    #region Properties

    /// <inheritdoc cref="NamePart.Designation"/>
    public override string Designation => "replace";

    /// <inheritdoc cref="NamePart.Title"/>
    public override string Title => "Замена";

    /// <summary>
    /// Исходный текст.
    /// </summary>
    public string? From { get; set; }

    /// <summary>
    /// Замещающий текст.
    /// </summary>
    public string? To { get; set; }

    /// <summary>
    /// Требуется?
    /// </summary>
    public bool Required { get; set; }

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
        var result = new ReplacePart();
        foreach (var parameter in parameters)
        {
            parameter.Verify (true);
            switch (parameter.Name)
            {
                case "from":
                    result.From = parameter.Value!;
                    break;

                case "to":
                    result.To = parameter.Value!;
                    break;

                case "require":
                case "required":
                    result.Required = true;
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

        var result = fileInfo.Name;
        if (string.IsNullOrEmpty (From) || To is null)
        {
            return result;
        }

        if (Required && !result.Contains (From))
        {
            throw new ApplicationException();
        }

        result = result.Replace (From, To);

        return result;
    }

    #endregion
}
