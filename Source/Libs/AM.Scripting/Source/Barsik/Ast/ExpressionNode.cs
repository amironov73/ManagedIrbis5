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
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="inner">Вычисляемое выражение.</param>
    public ExpressionNode
        (
            AtomNode inner
        )
    {
        Sure.NotNull (inner);

        _inner = inner;
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="startPosition">Позиция в исходном коде.</param>
    /// <param name="inner">Вычисляемое выражение.</param>
    public ExpressionNode
        (
            SourcePosition startPosition,
            AtomNode inner
        )
        : base (startPosition)
    {
        Sure.NotNull (inner);

        _inner = inner;
    }

    #endregion

    #region Private members

    private readonly AtomNode _inner;

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
        _inner.Compute (context);
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return $"expression ({StartPosition}): {_inner}";
    }

    #endregion
}
