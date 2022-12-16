// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UseNameofExpression

/*
 * Ars Magna project, http://arsmagna.ru
 */

namespace AM.HtmlTags.Conventions.Elements.Builders;

using System.Linq;
using System.Text.RegularExpressions;

public class DefaultLabelBuilder : IElementBuilder
{
    private static readonly Regex[] RxPatterns =
    {
        new ("([a-z])([A-Z])", RegexOptions.IgnorePatternWhitespace),
        new ("([0-9])([a-zA-Z])", RegexOptions.IgnorePatternWhitespace),
        new ("([a-zA-Z])([0-9])", RegexOptions.IgnorePatternWhitespace)
    };

    public bool Matches (ElementRequest subject) => true;

    public HtmlTag Build (ElementRequest request)
    {
        return new HtmlTag ("label").Attr ("for", DefaultIdBuilder.Build (request))
            .Text (BreakUpCamelCase (request.Accessor.Name));
    }

    public static string BreakUpCamelCase (string fieldName)
    {
        var output = RxPatterns.Aggregate (fieldName,
            (current, regex) => regex.Replace (current, "$1 $2"));
        return output.Replace ('_', ' ');
    }
}
