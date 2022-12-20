// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* NotEqualPropertyOperation.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Linq.Expressions;

#endregion

#nullable enable

namespace AM.HtmlTags.Reflection.Expressions;

/// <summary>
///
/// </summary>
public class NotEqualPropertyOperation
    : BinaryComparisonPropertyOperation
{
    #region Properties

    /// <inheritdoc cref="AM.HtmlTags.Reflection.Expressions.BinaryComparisonPropertyOperation.OperationName"/>
    public override string OperationName => "IsNot";

    /// <inheritdoc cref="BinaryComparisonPropertyOperation.Text"/>
    public override string Text => "is not";

    #endregion

    #region Construction

    /// <summary>
    ///
    /// </summary>
    public NotEqualPropertyOperation()
        : base (ExpressionType.NotEqual)
    {
        // пустое тело конструктора
    }

    #endregion
}
