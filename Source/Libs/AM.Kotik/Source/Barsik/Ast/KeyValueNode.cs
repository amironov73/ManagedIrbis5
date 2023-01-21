// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* KeyValueNode.cs -- узел для хранения пары "ключ-значение"
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;

#endregion

#nullable enable

namespace AM.Kotik.Barsik;

/// <summary>
/// Узел AST для хранения пары "ключ-значение".
/// </summary>
public sealed class KeyValueNode
    : AstNode
{
    #region Properties

    /// <summary>
    /// Ключ.
    /// </summary>
    public AtomNode Key { get; }

    /// <summary>
    /// Значение.
    /// </summary>
    public AtomNode Value { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public KeyValueNode
        (
            AtomNode key,
            AtomNode value
        )
    {
        Key = key;
        Value = value;
    }

    #endregion

    #region AstNode members

    /// <inheritdoc cref="AstNode.DumpHierarchyItem(string?,int,System.IO.TextWriter)"/>
    internal override void DumpHierarchyItem 
        (
            string? name, 
            int level, 
            TextWriter writer
        )
    {
        base.DumpHierarchyItem (name, level, writer);
        
        Key.DumpHierarchyItem ("Key", level + 1, writer);
        Value.DumpHierarchyItem ("Value", level + 1, writer);
    }

    #endregion
}
