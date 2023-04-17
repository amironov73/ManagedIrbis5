// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable UnusedMember.Global

/* NewNode.cs -- оператор new
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;

using AM.Kotik.Ast;

#endregion

#nullable enable

namespace AM.Kotik.Barsik.Ast;

/// <summary>
/// Оператор `new`.
/// </summary>
internal sealed class NewNode
    : AtomNode
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public NewNode
        (
            string typeName,
            IList<string>? typeArguments,
            IList<AtomNode> constructorArguments,
            StatementBase? initialization
        )
    {
        _typeName = typeName;
        _typeArguments = typeArguments;
        _constructorArguments = constructorArguments;
        _initialization = initialization;
    }

    #endregion

    #region Private members

    private readonly string _typeName;
    private readonly IList<string>? _typeArguments;
    private readonly IList<AtomNode> _constructorArguments;
    private readonly StatementBase? _initialization;

    #endregion

    #region AtomNode members

    /// <inheritdoc cref="Compute"/>
    public override dynamic? Compute
        (
            Context context
        )
    {
        if (!context.Commmon.Settings.AllowNewOperator)
        {
            throw new BarsikException ("Operator NEW is not allowed");
        }

        var type = context.ResolveType (_typeName, _typeArguments);
        if (type is null)
        {
            context.Commmon.Error?.WriteLine($"Type '{_typeName}' not found");
            return null;
        }

        object? result;
        if (_constructorArguments.Count == 0)
        {
            result = Activator.CreateInstance (type);
        }
        else
        {
            var parameters = new List<object?>();
            foreach (var argument in _constructorArguments)
            {
                var parameter = (object?) argument.Compute (context);
                parameters.Add (parameter);
            }

            result = Activator.CreateInstance (type, parameters.ToArray());
        }

        if (_initialization is not null)
        {
            var previousCenter = context.With;
            var centerName = "$" + Guid.NewGuid().ToString ("N");
            context = context.CreateChildContext();
            context.SetVariable (centerName, result);
            var center = new VariableNode (centerName);
            context.With = center;

            try
            {
                _initialization.Execute (context);
            }
            finally
            {
                context.With = previousCenter;
                context.RemoveVariable (centerName);
            }
        }

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
        
        DumpHierarchyItem ("Type", level + 1, writer, _typeName);
        if (_typeArguments is not null)
        {
            foreach (var typeArgument in _typeArguments)
            {
                DumpHierarchyItem ("TypeArg: " + typeArgument, level + 1, writer);
            }
        }
        foreach (var argument in _constructorArguments)
        {
            argument.DumpHierarchyItem ("Arg", level + 1, writer);
        }
    }

    #endregion
}
