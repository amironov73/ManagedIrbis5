// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable EventNeverSubscribedTo.Global
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* IIrbisProvider.cs -- наиболее общий интерфейс подключения для мокирования
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Threading;
using AM.PlatformAbstraction;
using AM.Threading;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Наиболее общий интерфейс подключения к серверу ИРБИС64
    /// для мокирования
    /// </summary>
    public interface IIrbisProvider
        : IDisposable,
        IAsyncDisposable,
        IServiceProvider,
        ICancellable
    {
        #region Events

        /// <summary>
        /// Событие, возникающее при освобождении провайдера.
        /// </summary>
        event EventHandler? Disposing;

        #endregion

        #region Properties

        /// <summary>
        /// Имя текущей базы данных. Значение по умолчанию <c>"IBIS"</c>,
        /// что соответствует базе данных электронного каталога
        /// в стандартной конфигурации ИРБИС64.
        /// Имя текущей базы можно менять неограниченное число раз.
        /// Имя не должно быть пустым и не должно содержать символов,
        /// не являющихся алфавитно-цифровыми.
        /// Сервер ИРБИС64, работающий на Windows, <b>не различает</b>
        /// регистр символов в имени базы данных.
        /// Сервер ИРБИС64, работающий на Linux, <b>различает</b>
        /// регистр символов в имени базы данных.
        /// </summary>
        string? Database { get; set; }

        /// <summary>
        /// Признак успешного подключения к серверу.
        /// Автоматически устанавливается/сбрасывается клиентом.
        /// </summary>
        bool Connected { get; }

        /// <summary>
        /// Код ошибки последней выполненной операции.
        /// Значение больше или равное 0 означает отсутствие ошибки.
        /// </summary>
        int LastError { get; }

        /// <summary>
        /// Слой абстракции от платформы.
        /// </summary>
        PlatformAbstractionLayer PlatformAbstraction { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Проверка состояния провайдера.
        /// </summary>
        bool CheckProviderState();

        /// <summary>
        /// Конфигурация провайдера.
        /// </summary>
        /// <param name="configurationString">
        /// Строка с параметрами конфигурации.
        /// </param>
        void Configure
            (
                string configurationString
            );

        /// <summary>
        /// Поколение провайдера.
        /// </summary>
        /// <returns>64</returns>
        string GetGeneration();

        /// <summary>
        /// Получение хэндла для ожидания.
        /// </summary>
        public WaitHandle GetWaitHandle();

        #endregion

    } // interface IIrbisProvider

} // namespace ManagedIrbis
