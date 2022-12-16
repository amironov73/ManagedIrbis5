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

using System.Linq.Expressions;

#endregion

#nullable enable

namespace AM.HtmlTags.Reflection.Expressions;

public class EqualsPropertyOperation : BinaryComparisonPropertyOperation
{
    public EqualsPropertyOperation()
        : base (ExpressionType.Equal)
    {
    }

    public override string OperationName => "Is";

    public override string Text => "is";
}
