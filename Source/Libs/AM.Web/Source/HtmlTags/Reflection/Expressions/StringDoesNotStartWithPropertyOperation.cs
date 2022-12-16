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

using System;
using System.Reflection;

#endregion

#nullable enable

namespace AM.HtmlTags.Reflection.Expressions;

public class StringDoesNotStartWithPropertyOperation : CaseInsensitiveStringMethodPropertyOperation
{
    private static readonly MethodInfo _method =
        ReflectionHelper.GetMethod<string> (s => s.StartsWith ("", StringComparison.CurrentCulture));

    public StringDoesNotStartWithPropertyOperation()
        : base (_method, true)
    {
    }

    public override string OperationName => "DoesNotStartWith";

    public override string Text => "does not start with";
}
