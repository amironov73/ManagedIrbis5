// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* EqualsPropertyOperation.cs --
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
public class EqualsPropertyOperation
    : BinaryComparisonPropertyOperation
{
    #region Properties

    /// <inheritdoc cref="BinaryComparisonPropertyOperation.OperationName"/>
    public override string OperationName => "Is";

    /// <inheritdoc cref="BinaryComparisonPropertyOperation.Text"/>
    public override string Text => "is";

    #endregion

    #region Construction

    /// <summary>
    ///
    /// </summary>
    public EqualsPropertyOperation()
        : base (ExpressionType.Equal)
    {
        // пустое тело конструктора
    }

    #endregion
}
