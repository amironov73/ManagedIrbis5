// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* AsyncQuery.cs -- асинхронный вариант клиентского запроса к серверу ИРБИС64
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Infrastructure;

//
// async-методы не могут принимать ref-параметры,
// поэтому пришлось сделать отдельный класс
//

/// <summary>
/// Асинхронный вариант клиентского запроса к серверу ИРБИС64.
/// </summary>
public sealed class AsyncQuery
    : IQuery,
    IDisposable
{
    #region Constants

    /// <summary>
    /// Емкость по умолчанию для потока, в который сериализируется
    /// содержимое запроса, байты.
    /// </summary>
    private const int DefaultStreamCapacity = 1024;

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public AsyncQuery
        (
            IConnectionSettings connection,
            string commandCode
        )
    {
        Sure.NotNull (connection);
        Sure.NotNullNorEmpty (commandCode);

        _stream = new QueryStream (DefaultStreamCapacity);
        _stream.AddHeader (connection, commandCode);
    }

    #endregion

    #region Private members

    /// <summary>
    /// Сюда сериализируются запрос.
    /// </summary>
    private readonly QueryStream _stream;

    #endregion

    #region Public methods

    /// <summary>
    /// Добавление строки с целым числом (плюс перевод строки).
    /// </summary>
    public void Add
        (
            int value
        )
    {
        _stream.Add (value);
    }

    /// <summary>
    /// Добавление строки с флагом "да-нет" (плюс перевод строки).
    /// </summary>
    public void Add
        (
            bool value
        )
    {
        Add (value ? 1 : 0);
    }

    /// <summary>
    /// Добавление строки в кодировке ANSI (плюс перевод строки).
    /// </summary>
    public void AddAnsi
        (
            string? value
        )
    {
        _stream.AddAnsi (value);
    }

    /// <summary>
    /// Добавление строки в кодировке UTF-8 (плюс перевод строки).
    /// </summary>
    public void AddUtf (string? value)
    {
        _stream.AddUtf (value);
    }

    /// <summary>
    /// Добавление спецификации формата (плюс перевод строки).
    /// </summary>
    public void AddFormat
        (
            string? format
        )
    {
        _stream.AddFormat (format);
    }

    /// <summary>
    /// Отладочная печать в указанный поток в кодировке ANSI.
    /// </summary>
    public void Debug
        (
            TextWriter writer
        )
    {
        Sure.NotNull (writer);

        _stream.Debug (writer);
    }

    /// <summary>
    /// Отладочная печать в указанный поток в кодировке UTF-8.
    /// </summary>
    public void DebugUtf
        (
            TextWriter writer
        )
    {
        Sure.NotNull (writer);

        _stream.DebugUtf (writer);
    }

    /// <summary>
    /// Получение массива байтов, из которых состоит
    /// клиентский запрос.
    /// </summary>
    public ReadOnlyMemory<byte> GetBody()
    {
        return _stream.GetBody();
    }

    /// <summary>
    /// Подсчет общей длины запроса в байтах.
    /// </summary>
    public int GetLength()
    {
        return _stream.GetLength();
    }

    /// <summary>
    /// Добавление одного перевода строки.
    /// </summary>
    public void NewLine()
    {
        _stream.NewLine();
    }

    #endregion

    #region IDisposable members

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose()
    {
        // пустое тело метода, нечего диспозить
    }

    #endregion
}
