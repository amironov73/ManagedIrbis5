// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global
// ReSharper disable CommentTypo
// ReSharper disable EventNeverSubscribedTo.Global
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMemberInSuper.Global
// ReSharper disable UnusedParameter.Local
// ReSharper disable VirtualMemberCallInConstructor

/* IMagnaApplication.cs -- интерфейс приложения
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace AM.AppServices;

/// <summary>
/// Интефейс приложения.
/// </summary>
public interface IMagnaApplication
{
    #region Properties

    /// <summary>
    /// Экземпляр инициализирован?
    /// </summary>
    bool IsInitialized { get; }

    /// <summary>
    /// Экземпляр отработал?
    /// </summary>
    bool IsShutdown { get; }

    /// <summary>
    /// Аргументы командной строки.
    /// </summary>
    string[] Args { get; }

    /// <summary>
    /// Конфигурация.
    /// </summary>
    IConfiguration Configuration { get; }

    /// <summary>
    /// Логгер.
    /// </summary>
    ILogger Logger { get; }

    /// <summary>
    /// Пользователь запросил прекращение текущего действия?
    /// </summary>
    bool Stop { get; set; }

    /// <summary>
    /// Хост.
    /// </summary>
    IHost ApplicationHost { get; }

    #endregion

    #region Methods

    /// <summary>
    /// Запуск приложения.
    /// </summary>
    void Run();

    /// <summary>
    /// Завершение приложения.
    /// </summary>
    void Shutdown();

    #endregion
}
