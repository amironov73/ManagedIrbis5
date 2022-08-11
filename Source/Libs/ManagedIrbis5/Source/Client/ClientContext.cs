// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* ClientContext.cs -- клиентский контекст
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using ManagedIrbis.Direct;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

#endregion

#nullable enable

namespace ManagedIrbis.Client;

/// <summary>
/// Клиентский контекст.
/// </summary>
public class ClientContext
    : ReactiveObject
{
    #region Properties

    /// <summary>
    /// Текущая запись.
    /// </summary>
    [Reactive]
    public Record? CurrentRecord { get; set; }

    /// <summary>
    /// Найденные записи.
    /// </summary>
    [Reactive]
    public FoundRecord[]? FoundRecords { get; set; }

    #endregion
}
