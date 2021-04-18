// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* IIrbisConnectionSettings.cs -- параметры подключения к серверу ИРБИС64
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Параметры подключения к серверу ИРБИС64
    /// </summary>
    public interface IIrbisConnectionSettings
    {
        /// <summary>
        /// Адрес хоста ИРБИС64. Может задаваться как в числовой форме
        /// (например, <c>"172.27.100.2"</c>), так и в виде имени (например,
        /// <c>"libertine"</c> или <c>"irbis.mylib.com"</c>).
        /// Значение по умолчанию <c>"127.0.0.1"</c>, что соответствует
        /// серверу, расположенному на том же хосте, что и клиент.
        /// </summary>
        string? Host { get; set; }

        /// <summary>
        /// Порт, на котором сервер ИРБИС64 принимает подключения.
        /// Значение по умолчанию <c>6666</c>, что соответствует
        /// стандартной конфигурации сервера.
        /// </summary>
        ushort Port { get; set; }

        /// <summary>
        /// Имя (логин) пользователя АБИС ИРБИС64.
        /// Значение по умолчанию (пустая строка) обязательно
        /// должно быть заменено  на реальное перед подключением.
        /// После подключения к серверу менять логин нельзя.
        /// Сервер ИРБИС64 <b>не различает</b> регистр символов в логине.
        /// </summary>
        string? Username { get; set; }

        /// <summary>
        /// Пароль пользователя АБИС ИРБИС64.
        /// Значение по умолчанию (пустая строка) обязательно
        /// должно быть заменено на реальное перед подключением к серверу.
        /// После подключения к серверу менять пароль нельзя.
        /// Сервер ИРБИС64 <b>различает</b> регистр символов в логине.
        /// </summary>
        string? Password { get; set; }

        /// <summary>
        /// Код АРМ АБИС ИРБИС64. Значение по умолчанию <c>"C"</c>
        /// соответствует АРМ "Каталогизатор".
        /// Код должен быть однобуквенным и соответствовать одному
        /// из АРМ АБИС ИРБИС64 (см. перечисление <see cref="Workstation"/>.
        /// После подключения к серверу менять код АРМ нельзя.
        /// </summary>
        string? Workstation { get; set; }

        /// <summary>
        /// Целое число -- идентификатор клиента.
        /// Выбирается случайно при подключении к серверу
        /// и используется на протяжении всей рабочей сессии.
        /// </summary>
        int ClientId { get; }

        /// <summary>
        /// Целое число -- порядковый номер запроса.
        /// Автоматически инкрементируется клиентом по мере
        /// выполнения новых запросов.
        /// </summary>
        int QueryId { get; }

    } // interface IIrbisConnectionSettings

} // namespace ManagedIrbis
