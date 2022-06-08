// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

/* ConstantNode.cs -- константное значение
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Scripting;

/// <summary>
/// Константное значение.
/// </summary>
internal sealed class ConstantNode
    : AtomNode
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ConstantNode
        (
            object? value
        )
    {
        _value = value;
    }

    #endregion

    #region Private members

    // собственно хранимое значение
    private readonly object? _value;

    #endregion

    #region AtomNode members

    /// <inheritdoc cref="AtomNode.Compute"/>
    public override dynamic? Compute
        (
            Context context
        )
    {
        return _value;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return $"constant '{_value}'";
    }

    #endregion
}
