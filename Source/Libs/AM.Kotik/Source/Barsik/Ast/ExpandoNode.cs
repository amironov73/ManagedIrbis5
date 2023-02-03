// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

/* ExpandoNode.cs -- создание объекта анонимного типа
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Dynamic;
using System.IO;

using AM.Kotik.Ast;

#endregion

#nullable enable

namespace AM.Kotik.Barsik.Ast;

/// <summary>
/// Создание объекта анонимного типа.
/// </summary>
public sealed class ExpandoNode
    : AtomNode
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ExpandoNode
        (
            StatementBase initialization
        )
    {
        Sure.NotNull (initialization);
        
        _initialization = initialization;
    }

    #endregion
    
    #region Private members

    private readonly StatementBase _initialization;

    #endregion

    #region AtomNode members

    /// <inheritdoc cref="AtomNode.Compute"/>
    public override dynamic Compute 
        (
            Context context
        )
    {
        const string variableName = "$object";
        var result = new ExpandoObject();

        var initializationContext = context.CreateChildContext();
        initializationContext.Variables[variableName] = result;
        var variableNode = new VariableNode (variableName);
        initializationContext.With = variableNode;
        _initialization.Execute (initializationContext);
        
        return result;
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

        _initialization.DumpHierarchyItem ("Initialization", level + 1, writer);
    }

    #endregion
}
