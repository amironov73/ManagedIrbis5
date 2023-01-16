// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* VariableNode.cs -- ссылка на переменную
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

using System;

namespace AM.Kotik;

/// <summary>
/// Ссылка на переменную.
/// </summary>
public sealed class VariableNode
    : AtomNode
{
    #region Properties

    /// <summary>
    /// Имя переменной.
    /// </summary>
    public string Name { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public VariableNode
        (
            string name
        )
    {
        Sure.NotNullNorEmpty (name);

        Name = name;
    }

    #endregion

    #region AtomNode members

    /// <inheritdoc cref="AtomNode.Compute"/>
    public override dynamic Compute
        (
            Context context
        )
    {
        throw new NotImplementedException();

    }

    /// <inheritdoc cref="AtomNode.Assign"/>
    public override dynamic Assign
        (
            Context context,
            string operation,
            dynamic? value
        )
    {
        throw new NotImplementedException();
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return $"variable '{Name}'";
    }

    #endregion
}
