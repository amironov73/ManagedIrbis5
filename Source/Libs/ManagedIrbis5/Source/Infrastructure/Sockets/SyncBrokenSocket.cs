// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedParameter.Local

/* SyncBrokenSocket.cs -- синхронный "сбойный" сокет для целей тестирования
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Infrastructure.Sockets;

/// <summary>
/// Синхронный "сбойный" сокет для целей тестирования.
/// </summary>
public sealed class SyncBrokenSocket
    : SyncNestedSocket
{
    #region Constants

    /// <summary>
    /// Вероятность сбоя по умолчанию.
    /// </summary>
    public const double DefaultProbability = 0.07;

    #endregion

    #region Properties

    /// <summary>
    /// Вероятность сбоя.
    /// </summary>
    public double Probability { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public SyncBrokenSocket
        (
            ISyncClientSocket innerSocket,
            double probability = DefaultProbability
        )
        : base (innerSocket)
    {
        _random = new Random();
    }

    #endregion

    #region Private members

    private readonly Random _random;

    #endregion

    #region ISyncClientSocket members

    /// <inheritdoc cref="ISyncClientSocket.TransactSync"/>
    public override Response? TransactSync
        (
            SyncQuery query
        )
    {
        if (Probability is > 0.0 and < 1.0)
        {
            var value = _random.NextDouble();
            if (value < Probability)
            {
                Magna.Trace (nameof (SyncBrokenSocket) + "::" + nameof (TransactSync)
                             + ": simulate broken network");
                return null;
            }
        }

        return base.TransactSync (query);
    }

    #endregion
}
