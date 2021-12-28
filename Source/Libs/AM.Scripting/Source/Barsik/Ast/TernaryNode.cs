// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* TernaryNode.cs -- тернарный оператор
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Text;

#endregion

#nullable enable

namespace AM.Scripting.Barsik;

/// <summary>
/// Тернарный оператор "условие ? истина : ложь".
/// </summary>
sealed class TernaryNode
    : AtomNode
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public TernaryNode
        (
            AtomNode condition,
            AtomNode trueValue,
            AtomNode falseValue
        )
    {
        Sure.NotNull (condition);
        Sure.NotNull (trueValue);
        Sure.NotNull (falseValue);

        _condition = condition;
        _trueValue = trueValue;
        _falseValue = falseValue;
    }

    #endregion

    #region Private members

    private readonly AtomNode _condition;
    private readonly AtomNode _trueValue;
    private readonly AtomNode _falseValue;

    #endregion

    #region AtomNode members

    /// <inheritdoc cref="AtomNode.Compute"/>
    public override dynamic? Compute
        (
            Context context
        )
    {
        var value = _condition.Compute (context);
        var result = value
            ? _trueValue.Compute (context)
            : _falseValue.Compute (context);

        return result;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        var builder = StringBuilderPool.Shared.Get();
        builder.AppendLine ($"ternary condition: {_condition}");
        builder.AppendLine ($"true: {_trueValue}");
        builder.AppendLine ($"false: {_falseValue}");

        var result = builder.ToString();
        StringBuilderPool.Shared.Return (builder);

        return result;
    }

    #endregion
}
