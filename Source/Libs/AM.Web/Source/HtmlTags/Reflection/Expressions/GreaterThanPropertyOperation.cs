// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* GreaterThanPropertyOperation.cs --
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
public class GreaterThanPropertyOperation
    : BinaryComparisonPropertyOperation
{
    #region Properties

    /// <inheritdoc cref="BinaryComparisonPropertyOperation.OperationName"/>
    public override string OperationName => "GreaterThan";

    /// <inheritdoc cref="BinaryComparisonPropertyOperation.Text"/>
    public override string Text => "greater than";

    #endregion

    #region Construction

    /// <summary>
    ///
    /// </summary>
    public GreaterThanPropertyOperation()
        : base (ExpressionType.GreaterThan)
    {
        // пустое тело конструктора
    }

    #endregion
}
