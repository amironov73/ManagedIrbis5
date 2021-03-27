// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedParameter.Local

/* IAsyncConnection.cs -- интерфейс асинхронного подключения
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.IO;
using AM.PlatformAbstraction;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Menus;
using ManagedIrbis.Pft;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Интерфейс асинхронного подключения.
    /// </summary>
    public interface IAsyncConnection
        : IAsyncIrbisProvider,
        IIrbisConnectionSettings
    {

    }
}
