// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* FunctionDefinitionNode.cs -- псевдо-узел: определение функции в скрипте
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

using System.IO;

namespace AM.Kotik;

/// <summary>
/// Псевдо-узел AST: определение функции в скрипте
/// </summary>
public sealed class FunctionDefinitionNode
    : PseudoNode
{
    #region Properties

    /// <summary>
    /// Имя функции.
    /// </summary>
    public string Name { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public FunctionDefinitionNode
        (
            int line,
            string functionName,
            string[] argumentNames,
            StatementBase body
        )
        : base (line)
    {
        Sure.NotNullNorEmpty (functionName);
        Sure.NotNull (argumentNames);
        Sure.NotNull (body);

        Name = functionName;
        _argumentNames = argumentNames;
        _body = body;
    }

    #endregion

    #region Private members

    internal readonly string[] _argumentNames;
    internal readonly StatementBase _body;

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
        foreach (var argumentName in _argumentNames)
        {
            DumpHierarchyItem ("Arg", level + 1, writer, argumentName);
        }
        
        _body.DumpHierarchyItem ("Body", level + 1, writer);
    }

    #endregion
    
    #region Object members

    /// <inheritdoc cref="AstNode.ToString"/>
    public override string ToString() => $"Function Definition at line {Line}";

    #endregion
    
}
