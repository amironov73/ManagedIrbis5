// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* PostfixNode.cs -- постфиксная операция
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Scripting.Barsik;

/// <summary>
/// Постфиксная операция, например, "<c>++</c>".
/// </summary>
sealed class PostfixNode
    : UnaryNode
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PostfixNode
        (
            string type,
            AtomNode inner
        )
    {
        _type = type;
        _inner = inner;
    }

    #endregion

    #region Private members

    private readonly string _type;
    private readonly AtomNode _inner;

    #endregion

    #region AtomNode members

    /// <inheritdoc cref="AtomNode.Compute"/>
    public override dynamic? Compute
        (
            Context context
        )
    {
        if (_inner is VariableNode variable)
        {
            var name = variable.Name;
            if (!context.TryGetVariable (name, out var oldValue))
            {
                context.Error.WriteLine ($"Variable {name} not found");

                return null;
            }

            var newValue = _type switch
            {
                "++" => Increment (oldValue),
                "--" => Decrement (oldValue),
                _ => throw new BarsikException ($"Unknown operation {_type}")
            };

            context.SetVariable (name, newValue);

            return oldValue;
        }

        throw new BarsikException ($"Bad inner node {_inner}");
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return $"postfix ({_inner} {_type})";
    }

    #endregion
}
