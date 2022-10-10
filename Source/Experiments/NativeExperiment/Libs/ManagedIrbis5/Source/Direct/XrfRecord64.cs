// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* XrfRecord64.cs -- запись в XRF-файле
 * Ars Magna project, http://arsmagna.ru
 */

namespace ManagedIrbis.Direct;

//
// Extract from official documentation:
// http://sntnarciss.ru/irbis/spravka/wtcp006002000.htm
//
// Каждая ссылка состоит из 3-х полей:
// Число бит Параметр
// 32        XRF_LOW – младшее слово в 8 байтовом смещении на запись;
// 32        XRF_HIGH– старшее слово в 8 байтовом смещении на запись;
// 32        XRF_FLAGS – Индикатор записи в виде битовых флагов
//           следующего содержания:
//             BIT_LOG_DEL(1)  - логически удаленная запись;
//             BIT_PHYS_DEL(2) - физически удаленная запись;
//             BIT_ABSENT(4)  - несуществующая запись;
//             BIT_NOTACT_REC(8)- неактуализированная запись;
//             BIT_LOCK_REC(64)- заблокированная запись.
//

/// <summary>
/// Запись в XRF-файле. Содержит информацию о смещении
/// и о статусе MST-записи.
/// </summary>
public struct XrfRecord64
{
    #region Constants

    /// <summary>
    /// Фиксированный размер записи = 12 байт.
    /// </summary>
    public const int RecordSize = sizeof (long) + sizeof (int);

    #endregion

    #region Properties

    /// <summary>
    /// 8-байтовое смещение записи в MST-файле.
    /// </summary>
    public long Offset;

    /// <summary>
    /// Статус записи.
    /// </summary>
    public RecordStatus Status;

    /// <summary>
    /// Запись заблокирована?
    /// </summary>
    public bool IsLocked
    {
        get => (Status & RecordStatus.Locked) != 0;
        set
        {
            if (value)
            {
                Status |= RecordStatus.Locked;
            }
            else
            {
                Status &= ~RecordStatus.Locked;
            }
        }
    }

    /// <summary>
    /// Запись удалена (неважно, логически или физически)?
    /// </summary>
    public bool IsDeleted
    {
        get
        {
            const RecordStatus badStatus
                = RecordStatus.LogicallyDeleted
                  | RecordStatus.PhysicallyDeleted;

            return (Status & badStatus) != 0;
        }
    }

    /// <summary>
    /// Запись физически удалена?
    /// </summary>
    public bool IsPhysicallyDeleted =>
        (Status & RecordStatus.PhysicallyDeleted) != 0;

    /// <summary>
    /// Запись логически удалена?
    /// </summary>
    public bool IsLogicallyDeleted =>
        (Status & RecordStatus.LogicallyDeleted) != 0;

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        return $"Offset: {Offset}, Status: {Status}";
    }

    #endregion
}
