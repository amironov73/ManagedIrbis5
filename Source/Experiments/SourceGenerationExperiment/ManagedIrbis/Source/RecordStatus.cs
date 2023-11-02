// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace ManagedIrbis;

public enum RecordStatus
{
    /// <summary>
    /// Нет статуса -- запись только что создана.
    /// </summary>
    None = 0,

    /// <summary>
    /// Запись логически удалена.
    /// </summary>
    LogicallyDeleted = 1,

    /// <summary>
    /// Запись физически удалена.
    /// </summary>
    PhysicallyDeleted = 2,

    /// <summary>
    /// Запись отсутствует.
    /// </summary>
    Absent = 4,

    /// <summary>
    /// Запись не актуализирована.
    /// </summary>
    NonActualized = 8,

    /// <summary>
    /// Первый экземпляр записи.
    /// </summary>
    NewRecord = 16,

    /// <summary>
    /// Последний экземпляр записи.
    /// </summary>
    Last = 32,

    /// <summary>
    /// Запись заблокирована.
    /// </summary>
    Locked = 64,

    /// <summary>
    /// Ошибка в Autoin.gbl.
    /// </summary>
    AutoinError = 128,

    /// <summary>
    /// Полный текст не актуализирован.
    /// </summary>
    FullTextNotActualized = 256
}
