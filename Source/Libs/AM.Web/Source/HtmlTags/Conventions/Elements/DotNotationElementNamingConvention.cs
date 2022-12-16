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

namespace AM.HtmlTags.Conventions.Elements;

using System;
using System.Linq;

using Reflection;

public class DotNotationElementNamingConvention : IElementNamingConvention
{
    public static Func<string, bool> IsCollectionIndexer = x => x.StartsWith ("[") && x.EndsWith ("]");

    public string GetName (Type modelType, Accessor accessor)
    {
        return accessor.PropertyNames
            .Aggregate ((x, y) =>
            {
                var formatString = IsCollectionIndexer (y)
                    ? "{0}{1}"
                    : "{0}.{1}";
                return string.Format (formatString, x, y);
            });
    }
}
