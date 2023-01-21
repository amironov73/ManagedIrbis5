// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* CallNode.cs -- вызов свободной функции
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.IO;

#endregion

#nullable enable

namespace AM.Kotik.Barsik;

/// <summary>
/// Вызов свободной функции, например, `println`.
/// </summary>
public sealed class CallNode
    : AtomNode
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="name">Имя функции.</param>
    /// <param name="arguments">Аргументы (если есть).</param>
    public CallNode
        (
            string name,
            AtomNode[] arguments
        )
    {
        Sure.NotNullNorEmpty (name);

        _name = name;
        _arguments = arguments;
    }

    #endregion

    #region Private members

    private readonly string _name;
    private readonly AtomNode[] _arguments;
    private FunctionDescriptor? _function;

    #endregion

    #region AtomNode members

    /// <inheritdoc cref="AtomNode.Compute"/>
    public override dynamic? Compute
        (
            Context context
        )
    {
        // // это может быть вызов лямбды
        // if (context.TryGetVariable (_name, out var value))
        // {
        //     if (value is LambdaNode lambda)
        //     {
        //         return lambda.Execute (context, _arguments);
        //     }
        //
        //     throw new BarsikException ($"Unexpected variable {_name}");
        // }

        _function ??= context.GetFunction (_name).ThrowIfNull ();

        var args = new List<dynamic?>();
        foreach (var node in _arguments)
        {
            var arg = _function.ComputeArguments
                ? node.Compute (context)
                : node;
            args.Add (arg);
        }

        var result = _function.CallPoint (context, args.ToArray());

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

        DumpHierarchyItem ("Name", level + 1, writer, _name);
        foreach (var argument in _arguments)
        {
            argument.DumpHierarchyItem ("Arg", level + 1, writer);
        }
    }

    #endregion
}
