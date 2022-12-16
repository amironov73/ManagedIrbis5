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

#region Using directives

using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

#nullable enable

namespace AM.HtmlTags;

public class TagList : ITagSource
{
    private readonly IEnumerable<HtmlTag> _tags;

    public TagList (IEnumerable<HtmlTag> tags)
    {
        _tags = tags;
    }

    public string ToHtmlString()
    {
        if (_tags.Count() > 5)
        {
            var builder = new StringBuilder();
            _tags.Each (t => builder.AppendLine (t.ToString()));

            return builder.ToString();
        }

        return _tags.Select (x => x.ToString()).Join ("\n");
    }

    public IEnumerable<HtmlTag> AllTags() => _tags;

    public override string ToString() => ToHtmlString();
}
