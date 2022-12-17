// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* DefaultLabelBuilder.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Linq;
using System.Text.RegularExpressions;

#endregion

namespace AM.HtmlTags.Conventions.Elements.Builders;

/// <summary>
///
/// </summary>
public class DefaultLabelBuilder
    : IElementBuilder
{
    #region Private members

    private static readonly Regex[] RxPatterns =
    {
        new ("([a-z])([A-Z])", RegexOptions.IgnorePatternWhitespace),
        new ("([0-9])([a-zA-Z])", RegexOptions.IgnorePatternWhitespace),
        new ("([a-zA-Z])([0-9])", RegexOptions.IgnorePatternWhitespace)
    };

    #endregion

    #region IElementBuilder members

    /// <inheritdoc cref="ITagBuilder.Build"/>
    public HtmlTag Build
        (
            ElementRequest request
        )
    {
        Sure.NotNull (request);

        return new HtmlTag ("label")
            .Attr ("for", DefaultIdBuilder.Build (request))
            .Text (BreakUpCamelCase (request.Accessor.Name));
    }

    #endregion

    #region Public methods

    /// <inheritdoc cref="Matches"/>
    public bool Matches (ElementRequest subject) => true;

    /// <summary>
    ///
    /// </summary>
    /// <param name="fieldName"></param>
    /// <returns></returns>
    public static string BreakUpCamelCase
        (
            string fieldName
        )
    {
        Sure.NotNullNorEmpty (fieldName);

        var output = RxPatterns.Aggregate
            (
                fieldName,
                (current, regex) => regex.Replace (current, "$1 $2")
            );

        return output.Replace ('_', ' ');
    }

    #endregion
}
