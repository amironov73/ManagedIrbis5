// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* HistoryEntry.cs -- запись в истории входа-выхода
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text.Json.Serialization;

using AM.Json;

using JetBrains.Annotations;

#endregion

namespace Gatekeeper2024;

/// <summary>
/// Запись в истории входа-выхода.
/// </summary>
[PublicAPI]
internal sealed class HistoryEntry
{
    #region Properties

    /// <summary>
    /// Момент входа.
    /// </summary>
    [JsonPropertyName ("in")]
    public string? TimeIn { get; set; }

    /// <summary>
    /// Момент выхода.
    /// </summary>
    [JsonPropertyName ("out")]
    public string? TimeOut { get; set; }

    /// <summary>
    /// Номер читательского билета.
    /// </summary>
    [JsonPropertyName ("ticket")]
    public string? Ticket { get; set; }

    /// <summary>
    /// ФИО.
    /// </summary>
    [JsonPropertyName ("name")]
    public string? Name { get; set; }

    #endregion

    #region Public methods

    /// <summary>
    /// Прямое упорядочение по хронологии.
    /// </summary>
    public static int CompareForward (HistoryEntry first, HistoryEntry second) =>
        StringComparer.Ordinal.Compare (first.TimeIn, second.TimeIn);

    /// <summary>
    /// Обратное упорядочение по хронологии.
    /// </summary>
    public static int CompareReverse (HistoryEntry first, HistoryEntry second) =>
        - StringComparer.Ordinal.Compare (first.TimeIn, second.TimeIn);

    /// <summary>
    /// Данные для тестирования.
    /// </summary>
    public static HistoryEntry[] FakeEntries() => new HistoryEntry[]
    {
        new () { TimeIn = "10:11", TimeOut = "11:11", Ticket = "12345", Name = "Билибин, Иван Яковлевич" },
        new () { TimeIn = "10:10", TimeOut = "11:10", Ticket = "12346", Name = "Вальк, Генрих Оскарович" },
        new () { TimeIn = "10:09", TimeOut = "11:09", Ticket = "12347", Name = "Васнецов, Юрий Алексеевич" },
        new () { TimeIn = "10:08", TimeOut = "11:08", Ticket = "12348", Name = "Верейский, Орест Георгиевич" },
        new () { TimeIn = "10:07", TimeOut = "11:07", Ticket = "12349", Name = "Герасимов, Александр Михайлович" },
        new () { TimeIn = "10:06", TimeOut = "11:06", Ticket = "12350", Name = "Дейнека, Александр Александрович" },
        new () { TimeIn = "10:05", TimeOut = "11:05", Ticket = "12351", Name = "Ефимов, Борис Ефимович" },
        new () { TimeIn = "10:04", TimeOut = "11:04", Ticket = "12352", Name = "Жаба, Альфонс Константинович" },
        new () { TimeIn = "10:03", TimeOut = "11:03", Ticket = "12353", Name = "Иогансон, Борис Владимирович" },
        new () { TimeIn = "10:02", TimeOut = "11:02", Ticket = "12354", Name = "Калита, Николай Иванович" },
        new () { TimeIn = "10:01", TimeOut = "11:01", Ticket = "12355", Name = "Карлов, Георгий Николаевич" },
        new () { TimeIn = "10:00", TimeOut = "11:00", Ticket = "12356", Name = "Кокорин, Анатолий Владимирович" },
        new () { TimeIn = "09:59", TimeOut = "10:59", Ticket = "12357", Name = "Гудиашвили, Ладо Давидович" },
        new () { TimeIn = "09:58", TimeOut = "10:58", Ticket = "12358", Name = "Копелян, Исаак Залманович" },
    };

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => JsonUtility.SerializeWithReadableCyrillic (this);

    #endregion
}
