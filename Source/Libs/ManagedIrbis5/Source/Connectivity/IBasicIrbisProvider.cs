// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* IBasicIrbisProvider.cs -- наиболее общий интерфейс подключения для мока
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Threading;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Наиболее общий интерфейс подключения к серверу ИРБИС64
    /// для мокирования
    /// </summary>
    public interface IBasicIrbisProvider
        : IDisposable,
        IAsyncDisposable,
        IServiceProvider
    {
        #region Events

        /// <summary>
        /// Событие, возникающее при изменении состояния
        /// свойства <see cref="Busy"/>.
        /// </summary>
        event EventHandler? BusyChanged;

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
        string Database { get; set; }

        /// <summary>
        /// Признак успешного подключения к серверу.
        /// Автоматически устанавливается/сбрасывается клиентом.
        /// </summary>
        bool Connected { get; }

        /// <summary>
        /// Провайдер занят выполнением операции?
        /// </summary>
        bool Busy { get; }

        /// <summary>
        /// Код ошибки последней выполненной операции.
        /// Значение больше или равное 0 означает отсутствие ошибки.
        /// </summary>
        int LastError { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Отмена текущей операции.
        /// </summary>
        void CancelOperation();

        /// <summary>
        /// Проверка состояния провайдера.
        /// </summary>
        bool CheckProviderState();

        /// <summary>
        /// Получение хэндла для ожидания.
        /// </summary>
        public WaitHandle GetWaitHandle();

        #endregion

    } // interface IBasicIrbisProvider

} // namespace ManagedIrbis
