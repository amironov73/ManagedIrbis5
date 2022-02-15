// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* IRecord.cs -- общий интерфейс для библиографической записи
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using ManagedIrbis.Direct;
using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis.Records;

/// <summary>
/// Общий интерфейс для библиографической записи.
/// </summary>
public interface IRecord
{
    /// <summary>
    /// База данных, в которой хранится запись.
    /// </summary>
    public string? Database { get; set; }

    /// <summary>
    /// MFN (порядковый номер в базе данных) записи.
    /// </summary>
    public int Mfn { get; set; }

    /// <summary>
    /// Версия записи.
    /// </summary>
    public int Version { get; set; }

    /// <summary>
    /// Статус записи.
    /// </summary>
    public RecordStatus Status { get; set; }

    /// <summary>
    /// Декодирование ответа сервера.
    /// </summary>
    void Decode (Response response);

    /// <summary>
    /// Декодирование записи, считанной из базы.
    /// </summary>
    void Decode (MstRecord64 record);

    /// <summary>
    /// Кодирование записи в текст.
    /// </summary>
    string Encode (string? delimiter = IrbisText.IrbisDelimiter);

    /// <summary>
    /// Кодирование записи для базы данных.
    /// </summary>
    void Encode (MstRecord64 record);

    /// <summary>
    /// Получить текст поля до разделителей подполей
    /// первого повторения поля с указанной меткой.
    /// </summary>
    /// <param name="tag">Метка поля.</param>
    /// <returns>Значение поля или <c>null</c>.</returns>
    string? FM (int tag);
}
