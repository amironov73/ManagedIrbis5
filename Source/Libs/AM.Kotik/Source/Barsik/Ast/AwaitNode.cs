// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* AwaitNode.cs -- оператор await
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;
using System.Threading.Tasks;

using AM.Kotik.Ast;

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
        _inner = inner;
    }

    #endregion

    #region Private members

    private readonly AtomNode _inner;

    private async void _DoAwait
        (
            Task task
        )
    {
        await task;
    }

    #endregion

    #region AtomNode members

    /// <inheritdoc cref="AtomNode.Compute"/>
    public override dynamic? Compute
        (
            Context context
        )
    {
        // TODO реализовать асинхронно

        var value = _inner.Compute (context);
        if (value is Task task)
        {
            _DoAwait (task);

            return null;
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
