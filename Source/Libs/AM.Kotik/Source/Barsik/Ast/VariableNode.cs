// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* VariableNode.cs -- ссылка на переменную
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;

using AM.Kotik.Ast;

#endregion

#nullable enable

namespace AM.Kotik.Barsik.Ast;

/// <summary>
/// Ссылка на переменную.
/// На самом деле может оказаться также ссылкой на тип или функцию.
/// </summary>
internal sealed class VariableNode
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
    public override dynamic? Compute
        (
            Context context
        )
    {
        if (context.TryGetVariable (Name, out var value))
        {
            return value;
        }

        var type = context.ResolveType (Name);
        if (type is not null)
        {
            return type;
        }

        if (context.FindFunction (Name, out var descriptor))
        {
            // это может быть именем функции
            return descriptor;
        }

        context.Commmon.Error?.WriteLine ($"Variable or type '{Name}' not defined");

        return null;
    }

    /// <inheritdoc cref="AtomNode.Assign"/>
    public override dynamic? Assign
        (
            Context context,
            string? operation,
            dynamic? value
        )
    {
        dynamic? variableValue = null;

        if (operation != "=")
        {
            if (!context.TryGetVariable (Name, out variableValue))
            {
                context.Commmon.Error?.WriteLine ($"Variable {Name} not found");
            }
        }

        // context.Output.WriteLine ($"AssignVariable {Name} ({variableValue}) {operation} {value}");

        value = operation switch
        {
            "=" => value,
            "+=" => variableValue + value,
            "-=" => variableValue - value,
            "*=" => variableValue * value,
            "/=" => variableValue / value,
            "%=" => variableValue % value,
            "&=" => variableValue & value,
            "|=" => variableValue | value,
            "^=" => variableValue ^ value,
            "<<=" => variableValue << value,
            ">>=" => variableValue >> value,
            _ => throw new InvalidOperationException()
        };

        context.SetVariable (Name, value);

        return value;
    }

    #endregion

    #region AstNode members

    /// <inheritdoc cref="AstNode.DumpHierarchyItem(string?,int,System.IO.TextWriter,string?)"/>
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
        return $"VariableNode '{Name}'";
    }

    #endregion
}
