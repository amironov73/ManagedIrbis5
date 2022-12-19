// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* LessThanPropertyOperation.cs --
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
public class LessThanPropertyOperation
    : BinaryComparisonPropertyOperation
{
    #region Properties

    /// <inheritdoc cref="BinaryComparisonPropertyOperation.OperationName"/>
    public override string OperationName => "LessThan";

    /// <inheritdoc cref="BinaryComparisonPropertyOperation.Text"/>
    public override string Text => "less than";

    #endregion

    #region Construction

    /// <summary>
    ///
    /// </summary>
    public LessThanPropertyOperation()
        : base (ExpressionType.LessThan)
    {
        // пустое тело конструктора
    }

    #endregion
}
