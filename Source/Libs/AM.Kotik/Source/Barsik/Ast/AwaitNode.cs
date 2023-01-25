// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* AwaitNode.cs -- оператор await
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Threading.Tasks;

#endregion

#nullable enable

namespace AM.Kotik.Barsik;

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
}
