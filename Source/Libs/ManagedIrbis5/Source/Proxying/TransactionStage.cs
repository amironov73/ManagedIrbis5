// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* TransactionStage.cs -- стадии транзакции при проксировании
 * Ars Magna project, http://arsmagna.ru
 */

namespace ManagedIrbis.Proxying;

/// <summary>
/// Стадии транзакции при проксировании ИРБИС64.
/// </summary>
public enum TransactionStage
{
    /// <summary>
    /// Начальная стадия: произошло подключение клиента
    /// к прослушивающему сокету.
    /// </summary>
    Initial = 1,

    /// <summary>
    /// Успешно прочитаны первые байты запроса, содержащие длину
    /// клиентского пакета.
    /// </summary>
    HeaderFetched = 2,

    /// <summary>
    /// В прочитанных байтах обнаружен ограничитель строки.
    /// </summary>
    DelimiterFound = 3,

    /// <summary>
    /// Из клиентского сокета прочитан весь пакет с запросом
    /// (согласно длине, указанной в первой строке).
    /// </summary>
    RequestFetched = 4,

    /// <summary>
    /// Полученный пакет прошел проверку.
    /// </summary>
    RequestValidated = 5,

    /// <summary>
    /// Установлено соединение с сервером.
    /// </summary>
    ConnectedToServer = 6,

    /// <summary>
    /// Клиенский запрос полностью переслан на сервер.
    /// </summary>
    RequestForwarded = 7,

    /// <summary>
    /// Получен ответ сервера.
    /// </summary>
    GotReply = 8,

    /// <summary>
    /// Пакет с ответом прошел проверку.
    /// </summary>
    ReplyValidated = 9,

    /// <summary>
    /// Пакет с ответом отослан клиенту.
    /// </summary>
    ReplySent = 10,

    /// <summary>
    /// "Нижний" сокет заглушен.
    /// </summary>
    BottomShutdown = 11,

    /// <summary>
    /// "Верхний" сокет закрыт.
    /// </summary>
    UpperClosed = 12,

    /// <summary>
    /// "Нижний" сокет закрыт. Обработка успешно завершена.
    /// </summary>
    AllDone = 13
}
