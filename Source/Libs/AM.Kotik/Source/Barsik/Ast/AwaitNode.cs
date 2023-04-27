// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* AwaitNode.cs -- оператор await
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using AM.Kotik.Ast;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace AM.Kotik.Barsik.Ast;

/// <summary>
/// Оператор await.
/// </summary>
internal sealed class AwaitNode
    : AtomNode
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public AwaitNode
        (
            AtomNode inner
        )
    {
        Sure.NotNull (inner);

        _inner = inner;
    }

    #endregion

    #region Private members

    private readonly AtomNode _inner;

    private void DoAwait
        (
            Context context,
            Task task
        )
    {
        try
        {
            // await task;
            var awaiter = task.GetAwaiter ();
            while (!awaiter.IsCompleted)
            {
                Thread.Yield();
            }
        }
        catch (Exception exception)
        {
            Magna.Logger.LogError (exception, "Error during task execution");
            context.Commmon.Error?.WriteLine("Error during task execution");
        }
    }

    #endregion

    #region AtomNode members

    /// <inheritdoc cref="AtomNode.Compute"/>
    public override dynamic? Compute
        (
            Context context
        )
    {
        Sure.NotNull (context);

        // TODO реализовать асинхронно

        var value = _inner.Compute (context);
        if (value is null)
        {
            context.Commmon.Error?.WriteLine ("Null task detected");
            Magna.Logger.LogError ("Null task Detected");

            return value;
        }

        var type = ((object) value).GetType ();
        if (type.IsGenericType && type.IsSubclassOf (typeof (Task)))
        {
            var genericTask = (Task) value;
            DoAwait (context, genericTask);
            var property = type.GetProperty ("Value").ThrowIfNull();
            value = property.GetValue (genericTask);

            return value;
        }

        if (value is Task task)
        {
            DoAwait (context, task);
        }

        return value;
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

        _inner.DumpHierarchyItem ("Inner", level + 1, writer);
    }

    /// <inheritdoc cref="AstNode.GetNodeInfo"/>
    public override AstNodeInfo GetNodeInfo() => new (this)
        {
            Name = "await",
            Children =
            {
                _inner.GetNodeInfo()
            }
        };

    #endregion
}
