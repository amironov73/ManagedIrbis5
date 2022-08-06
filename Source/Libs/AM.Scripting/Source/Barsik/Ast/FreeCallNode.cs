// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* FreeCallNode.cs -- вызов свободной функции
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

using AM.Text;

#endregion

#nullable enable

namespace AM.Scripting.Barsik;

/// <summary>
/// Вызов свободной функции.
/// </summary>
sealed class FreeCallNode
    : AtomNode
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="name">Имя функции.</param>
    /// <param name="arguments">Аргументы (если есть).</param>
    public FreeCallNode
        (
            string name,
            IEnumerable<AtomNode>? arguments
        )
    {
        Sure.NotNullNorEmpty (name);

        _name = name;
        _arguments = new ();
        if (arguments is not null)
        {
            _arguments.AddRange (arguments);
        }
    }

    #endregion

    #region Private members

    private readonly string _name;
    private readonly List<AtomNode> _arguments;
    private FunctionDescriptor? _function;

    #endregion

    #region AtomNode members

    /// <inheritdoc cref="AtomNode.Compute"/>
    public override dynamic? Compute
        (
            Context context
        )
    {
        // это может быть вызов лямбды
        if (context.TryGetVariable (_name, out var value))
        {
            if (value is LambdaNode lambda)
            {
                return lambda.Execute (context, _arguments);
            }

            throw new BarsikException ($"Unexpected variable {_name}");
        }

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

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        var builder = StringBuilderPool.Shared.Get();
        builder.Append ($"function '{_name}' (");
        var first = true;
        foreach (var node in _arguments)
        {
            if (!first)
            {
                builder.Append (", ");
            }

            builder.Append (node);

            first = false;
        }
        builder.Append (')');

        return builder.ReturnShared();
    }

    #endregion
}
