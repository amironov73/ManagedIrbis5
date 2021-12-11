// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo

/* LogClientManager.cs -- работа с базой данных LOGC
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

using AM;

using ManagedIrbis.Batch;

#endregion

#nullable enable

namespace ManagedIrbis.LogClientDb
{
    /*
        Начиная с версии 2021.1 разработана система протоколирования (логирования)
        действий клиента в АРМе Каталогизатор.

        Протоколируются (фиксируются) следующие действия клиента:
        - Регистрация (начало сеанса)
        - Разрегистрация (завершение сеанса)
        - Корректировка текущей записи
        - Импорт/Заимствование
        - Копирование
        - Экспорт
        - Глобальная корректировка (включая технологии на ее основе:
          корректировка по словарю, оперативные режимы, формирование подшивки)
        - Выполнение пакетного задания
        - Печать списочная
        - Печать табличная
        - Корректировка справочника
        - Печать стат.формы
        - Печать статистики

        Протоколирование ведется в специальной БД LOGC.

        Каждое действие фиксируется в виде одной записи БД, содержащей следующие данные:
        - Дата/Время
        - Логин клиента
        - IP-адрес клиента
        - БД
        - Код действия
        - Содержание действия

        Для наглядного представления протокола предлагается выходная табличная форма (LOGC0).

        Для "чистки" БД LOGC по диапазону дат предлагается задание на глобальную корректировку DelDate.

        Протоколирование управляется параметром в секции [MAIN] профиля клиента
        LOGC=
        который принимает значения:
        - 0 - протоколирование отключено (по умолчанию)
        - 1 - протоколирование включено

        БД LOGC ведется АВТОМАТИЧЕСКИ. Предполагается, что доступ к ней (для анализа протокола)
        имеют администратор или иное контролирующее лицо.

     */

    /// <summary>
    /// Работа с базой данных LOGC.
    /// </summary>
    public sealed class LogClientManager
    {
        #region Constants

        /// <summary>
        /// Имя базы данных.
        /// </summary>
        public const string DatabaseName = "LOGC";

        #endregion

        #region Properties

        /// <summary>
        /// Синхронный провайдер.
        /// </summary>
        public ISyncProvider Provider { get; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public LogClientManager
            (
                ISyncProvider provider
            )
        {
            Sure.NotNull (provider);

            Provider = provider;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Получение массива всех (не удаленных) записей из базы.
        /// </summary>
        public LogClientRecord[] GetAllRecords()
        {
            var result = new List<LogClientRecord>
                (
                    Provider.GetMaxMfn() + 1
                );

            var batch = BatchRecordReader.WholeDatabase
                (
                    Provider,
                    DatabaseName
                );

            foreach (var record in batch)
            {
                var log = LogClientRecord.ParseRecord (record);
                result.Add (log);
            }

            return result.ToArray();
        }

        #endregion
    }
}
