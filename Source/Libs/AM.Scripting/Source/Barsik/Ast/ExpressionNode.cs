// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ExpressionNode.cs -- стейтмент, в котором вычисляется значение выражения
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Scripting.Barsik;

/// <summary>
/// Стейтмент, в котором вычисляется значение выражения.
/// Например, происходит присваивание переменной вычисленного значения.
/// </summary>
internal sealed class ExpressionNode
    : StatementNode
{
    #region Properties

    /// <summary>
    /// Вычисляемое выражение.
    /// </summary>
    public AtomNode Expression { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="expression">Вычисляемое выражение.</param>
    public ExpressionNode
        (
            AtomNode expression
        )
    {
        Sure.NotNull (expression);

        Expression = expression;
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="startPosition">Позиция в исходном коде.</param>
    /// <param name="expression">Вычисляемое выражение.</param>
    public ExpressionNode
        (
            SourcePosition startPosition,
            AtomNode expression
        )
        : base (startPosition)
    {
        Sure.NotNull (expression);

        Expression = expression;
    }

    #endregion

    #region StatementNode members

    /// <inheritdoc cref="StatementNode.Execute"/>
    public override void Execute
        (
            Context context
        )
    {
        PreExecute (context);

        // вычисленное значение игнорируем
        Expression.Compute (context);
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return $"expression ({StartPosition}): {Expression}";
    }

    #endregion
}
