// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local
// ReSharper disable UnusedType.Global

/* IrbisSystemEvent.cs -- системные события, используемые сервером ИРБИС64
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Threading;

using AM;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Server;

//
// При старте сервера
//
// var systemEvent = new IrbisSystemEvent();
// if (!systemEvent.CheckOtherServerRunning())
// {
//   Облом
// }
// systemEvent.SayIamRunning();
//
// В конце
//
// systemEvent.Dispose();
//

/// <summary>
/// Системные события, используемые сервером ИРБИС64
/// для координации экземпляров сервера.
/// </summary>
public sealed class IrbisSystemEvent
    : IDisposable
{
    #region Constants

    /// <summary>
    /// Name of the event.
    /// </summary>
    public const string StartedName = "IRBIS64_STARTED";

    /// <summary>
    /// Name of the event.
    /// </summary>
    public const string StopName = "IRBIS64_STOP_";

    #endregion

    #region Properties

    /// <summary>
    /// IRBIS64 server started.
    /// </summary>
    public EventWaitHandle? StartedEvent { get; }

    /// <summary>
    /// Stop the IRBIS64 server.
    /// </summary>
    public EventWaitHandle? StopEvent { get; }

    /// <summary>
    /// Флаг, означающий, что создано новое событие
    /// (а не использовано ранее созданное).
    /// </summary>
    public bool Created { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public IrbisSystemEvent()
    {
        StartedEvent = new EventWaitHandle
            (
                false,
                EventResetMode.ManualReset,
                StartedName,
                out var created
            );
        Created = created;

        Magna.Logger.LogTrace
            (
                nameof (IrbisSystemEvent) + "::Constructor"
                + ": created={Time}",
                created
            );

        StopEvent = new EventWaitHandle
            (
                false,
                EventResetMode.ManualReset,
                StopName
            );
    }

    /// <summary>
    /// Финализатор.
    /// </summary>
    ~IrbisSystemEvent()
    {
        Magna.Logger.LogTrace (nameof (IrbisSystemEvent) + "::Destructor");

        Dispose();
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Проверяет, не запущен ли другой экземпляр сервера ИРБИС64.
    /// </summary>
    /// <returns><c>true</c>, если другого экземпляра нет.</returns>
    public bool CheckOtherServerRunning()
    {
        return !StartedEvent.ThrowIfNull().WaitOne (1);
    }

    /// <summary>
    /// Проверяет, не запрошен ли останов сервера ИРБИС64.
    /// </summary>
    /// <returns><c>true</c>, если запрошен останов сервера.</returns>
    public bool CheckStopRequested()
    {
        return StopEvent.ThrowIfNull().WaitOne (1);
    }

    /// <summary>
    /// Запрашивает остановку ранее запущенного экземпляра
    /// сервера ИРБИС64.
    /// </summary>
    public void RequestStop()
    {
        StopEvent.ThrowIfNull().Set();
    }

    /// <summary>
    /// Заявляет "Я, сервер ИРБИС64, не потерплю других серверов
    /// пред ликом моим".
    /// </summary>
    public void SayIamRunning()
    {
        StartedEvent.ThrowIfNull().Set();
    }

    #endregion

    #region IDisposable members

    /// <inheritdoc cref="IDisposable.Dispose" />
    public void Dispose()
    {
        Magna.Logger.LogTrace (nameof (IrbisSystemEvent) + "::" + nameof (Dispose));

        GC.SuppressFinalize (this);
        StartedEvent?.Dispose();
        StopEvent?.Dispose();
    }

    #endregion
}
