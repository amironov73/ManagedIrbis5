// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftIteration.cs -- итерация для for/foreach и т. п.
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Threading.Tasks;

using AM;

using ManagedIrbis.Pft.Infrastructure.Ast;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure;

/// <summary>
/// Итерация для <see cref="PftParallelFor"/>,
/// <see cref="PftParallelForEach"/>, <see cref="PftParallelGroup"/>
/// и <see cref="PftParallelWith"/>.
/// </summary>
internal sealed class PftIteration //-V3073
    : IDisposable
{
    #region Properties

    public PftGroup? Group { get; }

    public object Data { get; }

    public PftNodeCollection? Nodes { get; set; }

    public PftContext Context { get; set; }

    public int Index { get; }

    public Task Task { get; }

    public Exception? Exception { get; private set; }

    public string Result => Context.Text;

    private Action<PftIteration, object>? TheAction { get; set; }

    #endregion

    #region Construction

    public PftIteration
        (
            PftContext context,
            PftNodeCollection nodes,
            int index,
            Action<PftIteration, object> theAction,
            object data,
            bool withGroup
        )
    {
        Sure.NotNull (context);
        Sure.NotNull (nodes);
        Sure.NonNegative (index);

        Context = context.Push();
        Nodes = nodes.CloneNodes (nodes.Parent);
        Index = index;
        if (withGroup)
        {
            Group = new PftGroup();
            Context.CurrentGroup = Group;
        }

        Context.Index = Index;
        Exception = null;
        TheAction = theAction;
        Data = data;

        Task = new Task (_Run);
        Task.Start();
    }

    #endregion

    #region Private members

    private void _Run()
    {
        try
        {
            TheAction?.Invoke (this, Data);
        }
        catch (Exception exception)
        {
            Magna.Logger.LogError
                (
                    exception,
                    nameof (PftIteration) + "::" + nameof (_Run)
                );

            Exception = exception;
        }
    }

    #endregion

    #region IDisposable members

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose()
    {
        Context.Pop();
    }

    #endregion

    #region Object members

    /// <see cref="object.ToString"/>
    public override string ToString()
    {
        return Index.ToString();
    }

    #endregion
}
