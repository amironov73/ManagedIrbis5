// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* PrefixNode.cs -- префиксная операция
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Scripting.Barsik;

/// <summary>
/// Префиксная операция, например, "++".
/// </summary>
sealed class PrefixNode
    : AtomNode
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PrefixNode
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
    public override dynamic? Compute (Context context)
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
                "++" => oldValue + 1,
                "--" => oldValue - 1,
                _ => throw new BarsikException ($"Unknown operation {_type}")
            };

            context.SetVariable (name, newValue);

            return newValue;
        }

        var value = _inner.Compute (context);

        value = _type switch
        {
            "!" => ! BarsikUtility.ToBoolean (value),
            "-" => - value,
            _ => throw new BarsikException ($"Unknown operation {_type}")
        };

        return value;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return $"prefix ({_type} {_inner})";
    }

    #endregion
}

