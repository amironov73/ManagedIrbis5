// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ConstantNode.cs -- константное значение
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;

#endregion

#nullable enable

namespace AM.Kotik.Barsik;

/// <summary>
/// Хранимое константное значение.
/// </summary>
public sealed class ConstantNode
    : AtomNode
{
    #region Properties

    /// <summary>
    /// Собственно хранимое значение.
    /// </summary>
    public object? Value { get; private set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ConstantNode
        (
            object? value
        )
    {
        Value = value;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Изменение значения константы.
    /// </summary>
    public ConstantNode ChangeValue
        (
            object? newValue
        )
    {
        Value = newValue;

        return this;
    }

    #endregion

    #region AtomNode members

    /// <inheritdoc cref="AtomNode.Compute"/>
    public override dynamic? Compute
        (
            Context context
        )
    {
        return Value;
    }

    #endregion

    #region AtomNode members

    /// <inheritdoc cref="AstNode.DumpHierarchyItem(string?,int,System.IO.TextWriter)"/>
    internal override void DumpHierarchyItem
        (
            string? name,
            int level,
            TextWriter writer
        )
    {
        base.DumpHierarchyItem (name, level, writer, ToString());
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return $"ConstantNode '{Value}'";
    }

    #endregion
}
