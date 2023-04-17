// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* IsNode.cs -- проверка значения на принадлежность указанному типу
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Kotik.Barsik.Ast;

/// <summary>
/// Проверка значения на принадлежность указанному типу.
/// </summary>
public sealed class IsNode
    : PostfixNode
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public IsNode
        (
            AtomNode obj,
            string typeName
        )
    {
        Sure.NotNull (obj);
        Sure.NotNullNorEmpty (typeName);
        
        _obj = obj;
        _typeName = typeName;
        _other = null;
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public IsNode
        (
            AtomNode obj,
            AtomNode other
        )
    {
        Sure.NotNull (obj);
        Sure.NotNull (other);
        
        _obj = obj;
        _typeName = null;
        _other = other;
    }

    #endregion
    
    #region Private members

    private readonly AtomNode _obj;
    private readonly string? _typeName;
    private readonly AtomNode? _other;

    #endregion

    #region AtomNode members

    /// <inheritdoc cref="AtomNode.Compute"/>
    public override dynamic? Compute 
        (
            Context context
        )
    {
        var value = _obj.Compute (context);

        if (value is null)
        {
            return _typeName == "null";
        }

        if (!string.IsNullOrEmpty (_typeName))
        {
            var leftType = ((object) value).GetType();
            var rightType = context.ResolveType (_typeName);

            if (leftType == rightType)
            {
                return true;
            }

            return leftType.IsAssignableTo (rightType);
        }

        if (_other is not null)
        {
            var other = _other.Compute (context);

            return OmnipotentComparer.Default.Compare (value, other) == 0;
        }

        return null;
    }

    #endregion
}
