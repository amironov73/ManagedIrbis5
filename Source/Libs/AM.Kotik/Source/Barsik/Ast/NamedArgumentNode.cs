// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* NamedParameterNode.cs -- именованный аргумент функции
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;

#endregion

#nullable enable

namespace AM.Kotik.Barsik.Ast;

/// <summary>
/// Именованный аргумент функции.
/// </summary>
internal sealed class NamedArgumentNode
    : AtomNode
{
    #region Properties

    /// <summary>
    /// Имя параметра.
    /// </summary>
    public string Name { get; }

    #endregion
    
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public NamedArgumentNode
        (
            string name, 
            AtomNode innerNode
        )
    {
        Sure.NotNullNorEmpty (name);
        Sure.NotNull (innerNode);
        
        _innerNode = innerNode;
        Name = name;
    }

    #endregion

    #region Private members

    private readonly AtomNode _innerNode;

    #endregion

    #region Public methods

    /// <summary>
    /// Реальное вычисление значения аргумента.
    /// </summary>
    public dynamic? RealCompute
        (
            Context context
        )
    {
        return _innerNode.Compute (context);
    }

    #endregion

    #region AtomNode members

    /// <inheritdoc cref="AtomNode.Compute"/>
    public override dynamic Compute 
        (
            Context context
        )
    {
        // фокус, чтобы сохранить имя аргумента
        
        return this;
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
        
        DumpHierarchyItem ("Name", level + 1, writer, Name);
        _innerNode.DumpHierarchyItem ("Value", level + 1, writer);
    }

    #endregion
}
