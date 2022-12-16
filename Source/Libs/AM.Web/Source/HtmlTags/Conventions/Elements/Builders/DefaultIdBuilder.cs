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

using System.Text.RegularExpressions;

#endregion

#nullable enable

namespace AM.HtmlTags.Conventions.Elements.Builders;

public class DefaultIdBuilder
{
    private static readonly Regex IdRegex = new (@"[\.\[\]]");

    public static string Build (ElementRequest request)
        => IdRegex.Replace (request.ElementId, "_");
}
