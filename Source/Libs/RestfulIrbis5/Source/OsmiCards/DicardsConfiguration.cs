﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UseNameofExpression

/* DicardsConfiguration.cs -- common DICARDS related configuration
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Text;
using System.Text.Json.Serialization;

using AM;
using AM.Json;

#endregion

#nullable enable

namespace RestfulIrbis.OsmiCards
{
    /// <summary>
    /// Общие настройки, связанные с DICARDS.
    /// </summary>
    public sealed class DicardsConfiguration
        : IVerifiable
    {
        #region Constants

        private const string IrbisConnection = "ИРБИС64: подключение к серверу";

        private const string IrbisDatabase = "ИРБИС64: база данных";

        private const string DicardsConnection = "DICARDS: подключение к API";

        private const string DicardsTemplate = "DICARDS: шаблон читательского билета";

        #endregion

        #region Properties

        /// <summary>
        /// IP-адрес сервера.
        /// </summary>
        [JsonIgnore]
        [Category(IrbisConnection)]
        [DisplayName("IP-адрес сервера")]
        [Description("IP-адрес хоста, на котором запущен сервер ИРБИС64.")]
        public string? Host { get; set; }

        /// <summary>
        /// Номер порта.
        /// </summary>
        [JsonIgnore]
        [Category(IrbisConnection)]
        [DisplayName("Номер порта на сервере")]
        [Description("Номер порта, на котором сервер ИРБИС64 "
            + "ожидает подключения клиентов. Как правило, 6666.")]
        public int Port { get; set; }

        /// <summary>
        /// Логин пользователя.
        /// </summary>
        [JsonIgnore]
        [Category(IrbisConnection)]
        [DisplayName("Логин пользователя")]
        [Description("Логин пользователя в системе ИРБИС64. "
            + "Не забудьте переключить раскладку клавиатуры, "
            + "если это необходимо!")]
        public string? Login { get; set; }

        /// <summary>
        /// Пароль.
        /// </summary>
        [JsonIgnore]
        [Category(IrbisConnection)]
        [DisplayName("Пароль")]
        [PasswordPropertyText(true)]
        [Description("Пароль чувствителен к регистру символов! "
            + "Не забудьте переключить раскладку клавиатуры, "
            + "если это необходимо!")]
        public string? Password { get; set; }

        /// <summary>
        /// База данных.
        /// </summary>
        [JsonIgnore]
        [Category(IrbisDatabase)]
        [DisplayName("Имя базы данных")]
        [Description("База данных читателей в ИРБИС64. "
            + "Как правило, RDR.")]
        public string? Database { get; set; }

        /// <summary>
        /// Строка подключения к серверу ИРБИС64.
        /// </summary>
        [Browsable(false)]
        [JsonPropertyName("connectionString")]
        public string? ConnectionString { get; set; }

        /// <summary>
        /// Идентификатор пользователя
        /// для подключения к DICARDS API.
        /// </summary>
        [JsonPropertyName("apiID")]
        [Category(DicardsConnection)]
        [DisplayName("ID пользователя")]
        [Description("Идентификатор пользователя системы DiCARDS.")]
        public string? ApiId { get; set; }

        /// <summary>
        /// Ключ для подключания к DICARDS API.
        /// </summary>
        [JsonPropertyName("apiKey")]
        [DisplayName("Ключ API")]
        [Category(DicardsConnection)]
        [Description("Ключ для доступа к API DiCARDS.")]
        public string? ApiKey { get; set; }

        /// <summary>
        /// Базовый URL для подключения к DICARDS API.
        /// </summary>
        [JsonPropertyName("baseUri")]
        [DisplayName("Базовый URL")]
        [Category(DicardsConnection)]
        [Description("URL точки подключения к API DiCARDS.")]
        public string? BaseUri { get; set; }

        /// <summary>
        /// Группа для извлечения читателей
        /// из репозитория DICARDS (опционально).
        /// </summary>
        [Category(DicardsConnection)]
        [JsonPropertyName("group")]
        [DisplayName("Группа в репозитории")]
        [Description("Группа регистрации новых пользователей "
            + "(задает DiCARDS).")]
        public string? Group { get; set; }

        /// <summary>
        /// Префикс для читательских билетов
        /// для читателей, извлеченных их репозитория DICARDS.
        /// </summary>
        [JsonPropertyName("prefix")]
        [Category(IrbisDatabase)]
        [DisplayName("Префикс идентификатора")]
        [Description("Префикс, присваиваемый номеру читательского "
            + "билета для импортированного читателя, может быть "
            + "пустой строкой.")]
        public string? Prefix { get; set; }

        /// <summary>
        /// Категория для читателей, извлеченных
        /// из репозитория DICARDS.
        /// </summary>
        [JsonPropertyName("category")]
        [Category(IrbisDatabase)]
        [DisplayName("Категория читателей")]
        [Description("Категория, присваиваемая импортируемому "
            + "читателю (задается самой библиотекой, исходя "
            + "из её задач).")]
        public string? Category { get; set; }

        /// <summary>
        /// Поле записи в БД RDR, используемое как идентификатор читателя.
        /// В дистрибутиве это поле 30.
        /// </summary>
        [JsonPropertyName("readerID")]
        [Category(IrbisDatabase)]
        [DisplayName("Поле с идентификатором читателя")]
        [Description("Поле записи в БД RDR, используемое в качестве "
            + "идентификатора читателя. Как правило, это поле 30.")]
        public string? ReaderId { get; set; }

        /// <summary>
        /// Поле записи в БД RDR, используемое для хранения
        /// номера пропуска в библиотеку (например, RFID-метка).
        /// В дистрибутиве это поле 22.
        /// </summary>
        [JsonPropertyName("ticket")]
        [Category(IrbisDatabase)]
        [DisplayName("Поле с номером пропуска")]
        [Description("Поле записи в БД RDR, используемое для "
            + "хранения номера пропуска в библиотеку (например, "
            + "RFID-метка). Как правило, это поле 22.")]
        public string? Ticket { get; set; }

        /// <summary>
        /// Имя шаблона для карт DICARDS.
        /// </summary>
        [JsonPropertyName("template")]
        [Category(DicardsTemplate)]
        [DisplayName("Имя шаблона читательского билета")]
        [Description("Имя шаблона карты (читательского билета), "
                     + "которая будет послана на устройство читателя. "
                     + "Эту карту должен создать администратор.")]
        public string? Template { get; set; }

        /// <summary>
        /// Имя поля в шаблоне карточки читателя,
        /// в которое будет помещено ФИО читателя.
        /// </summary>
        [JsonPropertyName("fioField")]
        [Category(DicardsTemplate)]
        [DisplayName("Поле для ФИО")]
        [Description("Имя поля в шаблоне карточки читателя, "
            + "в которое будет помещено ФИО читателя.")]
        public string? FioField { get; set; }

        /// <summary>
        /// Имя поля в шаблоне карточки читателя,
        /// в которое будет помещен штрих-код.
        /// </summary>
        [JsonPropertyName("barcodeField")]
        [Category(DicardsTemplate)]
        [DisplayName("Поле для штрих-кода")]
        [Description("Имя поля в шаблоне карточки читателя, "
            + "в которое будет помещен штрих-код.")]
        public string? BarcodeField { get; set; }

        /// <summary>
        /// Имя поля в шаблоне карточки читателя
        /// для ссылки на личный кабинет.
        /// </summary>
        [JsonPropertyName("cabinetField")]
        [Category(DicardsTemplate)]
        [DisplayName("Поле для ссылки на личный кабинет")]
        [Description("Имя поля в шаблоне карточки читателя, "
            + "в которое будет помещена ссылка на личный кабинет.")]
        public string? CabinetField { get; set; }

        /// <summary>
        /// URL - ссылка на личный кабинет.
        /// </summary>
        [JsonPropertyName("cabinetUrl")]
        [Category(DicardsTemplate)]
        [DisplayName("URL - ссылка на личный кабинет")]
        [Description("URL - ссылка на личный кабинет.")]
        public string? CabinetUrl { get; set; }

        /// <summary>
        /// Имя поля в шаблоне карточки читателя
        /// для ссылки на электронный каталог библиотеки
        /// с возможностью заказа книг.
        /// </summary>
        [JsonPropertyName("catalogField")]
        [Category(DicardsTemplate)]
        [DisplayName("Поле для ссылки на электронный каталог")]
        [Description("Имя поля в шаблоне карточки читателя, "
            + "в которое будет помещена ссылка на электронный "
            + "каталог библиотеки.")]
        public string? CatalogField { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonPropertyName("catalogUrl")]
        [Category(DicardsTemplate)]
        [DisplayName("URL - ссылка на электронный каталог")]
        [Description("URL - ссылка на электронный каталог библиотеки.")]
        public string? CatalogUrl { get; set; }

        /// <summary>
        /// Имя поля в шаблоне карточки читателя,
        /// в которое будет помещено напоминание о необходимости
        /// сдать книги в библиотеку.
        /// </summary>
        [JsonPropertyName("reminderField")]
        [Category(DicardsTemplate)]
        [DisplayName("Поле для напоминания о задолженности")]
        [Description("Имя поля в шаблоне карточки читателя, "
            + "в которое будет помещено напоминание о необходимости"
            + "сдать книги в библиотеку")]
        public string? ReminderField { get; set; }

        /// <summary>
        /// Сообщение о необходимости возвращать книги.
        /// </summary>
        [JsonPropertyName("reminderMessage")]
        [Category(DicardsTemplate)]
        [DisplayName("Текст сообщения о необходимости возвращать книги")]
        [Description("Сообщение отправляемое задачей Pusher читателям, "
            + "имеющим просроченную задолженность (одно сообщение "
            + "на всех читателей).")]
        public string? ReminderMessage { get; set; }

        /// <summary>
        /// Имя поля в шаблоне карточки читателя,
        /// в которое будет помещено общее количество
        /// документов, числящихся за читателем.
        /// Имя поля задает сама библиотека,
        /// например «ВСЕГО», оно будет видно в приложении.
        /// </summary>
        [JsonPropertyName("totalCountField")]
        [Category(DicardsTemplate)]
        [DisplayName("Поле для количества книг")]
        [Description("Имя поля в шаблоне карточки читателя, "
            + "в которое будет помещено общее количество "
            + "книг, числящихся за читателем.")]
        public string? TotalCountField { get; set; }

        /// <summary>
        /// Имя файла формата, используемого для формирования
        /// значения в поле totalCountField. По умолчанию
        /// это “|total_count.pft”. Здесь символ “|” означает,
        /// что файл находится в локальной файловой системе
        /// (рядом с программой Back Office), а не на сервере ИРБИС64.
        /// </summary>
        [JsonPropertyName("totalCountFormat")]
        [Category(DicardsTemplate)]
        [DisplayName("Файл формата для количества книг")]
        [Description("Имя файла формата, используемого "
            + "для подсчета общего количества книг на руках "
            + "у читателя.")]
        public string? TotalCountFormat { get; set; }

        /// <summary>
        /// Имя поля в шаблоне карточки читателя, в которое будет
        /// помещено количество документов, просроченных читателем.
        /// Имя поля задает сама библиотека, например «ДОЛГ»,
        /// оно будет видно в приложении.
        /// </summary>
        [JsonPropertyName("expiredCountField")]
        [Category(DicardsTemplate)]
        [DisplayName("Поле для величины долга")]
        [Description("Имя поля в шаблоне карточки читателя, "
            + "в которое будет помещено количество просроченных "
            + "читателем книг.")]
        public string? ExpirecCountField { get; set; }

        /// <summary>
        /// Имя файла формата, используемого для формирования
        /// значения в поле expiredCountField. По умолчанию
        /// это “|expired_count.pft”. Здесь символ “|” означает,
        /// что файл находится в локальной файловой системе.
        /// </summary>
        [JsonPropertyName("expiredCountFormat")]
        [Category(DicardsTemplate)]
        [DisplayName("Файл формата для количества задолженных книг")]
        [Description("Имя файла формата, используемого для "
            + "подсчета количества задолженных читателем книг.")]
        public string? ExpiredCountFormat { get; set; }

        /// <summary>
        /// Имя поля в шаблоне карточки читателя, в которое
        /// будет помещен общий список документов, числящихся
        /// за читателем. Имя поля задает сама библиотека,
        /// например «КНИГИ», оно будет видно в приложении.
        /// </summary>
        [JsonPropertyName("totalListField")]
        [Category(DicardsTemplate)]
        [DisplayName("Поле для списка книг")]
        [Description("Имя поле в шаблоне карточки читателя, "
            + "в которое будет помещен список всех книг, "
            + "числящихся за данным читателем.")]
        public string? TotalListField { get; set; }

        /// <summary>
        /// Имя файла формата, используемого для формирования
        /// значения в поле totalListField. По умолчанию
        /// это “|total_list.pft”. Здесь символ “|” означает,
        /// что файл находится в локальной файловой системе.
        /// </summary>
        [JsonPropertyName("totalListFormat")]
        [Category(DicardsTemplate)]
        [DisplayName("Формат для списка книг")]
        [Description("Имя файла формата, используемого для "
            + "формирования списка книг на руках у данного "
            + "читателя.")]
        public string? TotalListFormat { get; set; }

        /// <summary>
        /// Имя поля в шаблоне карточки читателя, в которое
        /// будет помещен список документов, просроченных читателем.
        /// Имя поля задает сама библиотека.
        /// </summary>
        [JsonPropertyName("expiredListField")]
        [Category(DicardsTemplate)]
        [DisplayName("Поле для списка задолженных книг")]
        [Description("Имя поля в шаблоне карточки читателя, "
            + "в которое будет помещен список задолженных "
            + "читателем книг.")]
        public string? ExpiredListField { get; set; }

        /// <summary>
        /// Имя файла формата, используемого для формирования
        /// значения в поле expiredListField. По умолчанию
        /// это “|expired_list.pft”. Здесь символ “|” означает,
        /// что файл находится в локальной файловой системе.
        /// </summary>
        [JsonPropertyName("expiredListFormat")]
        [Category(DicardsTemplate)]
        [DisplayName("Формат для списка задолженных книг")]
        [Description("Имя файла формата, используемого для "
            + "формирования списка задолженных читателем книг.")]
        public string? ExpiredListFormat { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Чтение конфигурации из указанного файла.
        /// </summary>
        public static DicardsConfiguration LoadConfiguration
            (
                string fileName
            )
        {
            var result = JsonUtility.ReadObjectFromFile<DicardsConfiguration>(fileName);
            result.ApiId = Unprotect(result.ApiId);
            result.ApiKey = Unprotect(result.ApiKey);
            result.ConnectionString = Unprotect(result.ConnectionString);

            return result;
        }

        /// <summary>
        /// Запись конфигурации в указанный файл.
        /// </summary>
        public void SaveConfiguration
            (
                string fileName
            )
        {
            var clone = (DicardsConfiguration) MemberwiseClone();
            clone.ApiId = Protect(ApiId);
            clone.ApiKey = Protect(ApiKey);
            clone.ConnectionString = Protect(ConnectionString);

            JsonUtility.SaveObjectToFile(clone, fileName);
        }

        /// <summary>
        /// Примитивная защита от подглядывания паролей и прочего.
        /// Работает только против совсем неопытных пользователей.
        /// </summary>
        public static string? Protect
            (
                string? value
            )
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            var bytes = Encoding.UTF8.GetBytes(value);
            var result = "!" + Convert.ToBase64String(bytes);

            return result;
        }

        /// <summary>
        /// Раскодирование (при необходимости) строкового значения,
        /// закодированного методом <see cref="Protect"/>.
        /// </summary>
        public static string? Unprotect
            (
                string? value
            )
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            if (value.FirstChar() != '!')
            {
                return value;
            }

            var bytes = Convert.FromBase64String(value.Substring(1));
            var result = Encoding.UTF8.GetString(bytes);

            return result;
        }

        #endregion

        #region IVerifiable members

        private bool CheckString
            (
                string name,
                string? value
            )
        {
            if (string.IsNullOrEmpty(value))
            {
                Magna.Error($"Не задано значение для {name}");
                return false;
            }

            return true;
        }

        /// <inheritdoc cref="IVerifiable.Verify"/>
        public bool Verify
            (
                bool throwOnError
            )
        {
            bool result = CheckString(nameof(BaseUri), BaseUri)
                && CheckString(nameof(ApiId), ApiId)
                && CheckString(nameof(ApiKey), ApiKey)
                && CheckString(nameof(ConnectionString), ConnectionString)
                && CheckString(nameof(ReaderId), ReaderId)
                && CheckString(nameof(Ticket), Ticket);

            if (throwOnError && !result)
            {
                throw new VerificationException();
            }

            return result;
        }

        #endregion
    }
}
