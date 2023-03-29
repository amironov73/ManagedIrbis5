// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* DataAggregator.cs -- блок TPL Dataflow, умеющий собирать данные
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Threading.Tasks;

/// <summary>
/// Блок TPL Dataflow, умеющий собирать данные.
/// </summary>
[PublicAPI]
public sealed class ResultAggregator<TInput>
    : ITargetBlock<TInput>
{
    #region Properties

    /// <summary>
    /// Результат - собранные данные.
    /// </summary>
    public ConcurrentBag<TInput> ResultBag { get; }

    /// <summary>
    /// Пойманные исключения.
    /// </summary>
    public ConcurrentBag<Exception> Exceptions { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public ResultAggregator()
    {
        ResultBag = new ();
        Exceptions = new ConcurrentBag<Exception>();
        _actionBlock = new ActionBlock<TInput> (_ConsumeInput);
    }

    #endregion

    #region Private members

    private readonly ActionBlock<TInput> _actionBlock;
    private void _ConsumeInput (TInput input) => ResultBag.Add (input);

    #endregion

    #region IDataflowBlock members

    /// <inheritdoc cref="IDataflowBlock.Complete"/>
    public void Complete() => _actionBlock.Complete();

    /// <inheritdoc cref="IDataflowBlock.Completion"/>
    public Task Completion => _actionBlock.Completion;

    /// <inheritdoc cref="IDataflowBlock.Fault"/>
    void IDataflowBlock.Fault (Exception exception) => Exceptions.Add (exception);

    #endregion

    #region ITargetBlock members

    /// <inheritdoc cref="ITargetBlock{TInput}.OfferMessage"/>
    DataflowMessageStatus ITargetBlock<TInput>.OfferMessage
        (
            DataflowMessageHeader messageHeader,
            TInput messageValue,
            ISourceBlock<TInput>? source,
            bool consumeToAccept
        )
        => ((ITargetBlock<TInput>) _actionBlock).OfferMessage
            (
                messageHeader,
                messageValue,
                source,
                consumeToAccept
            );

    #endregion
}
