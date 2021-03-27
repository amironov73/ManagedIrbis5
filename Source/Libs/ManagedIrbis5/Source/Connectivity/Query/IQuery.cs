// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* IQuery.cs -- интерфейс клиентского запроса к серверу ИРБИС64
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Infrastructure
{
    /// <summary>
    /// Интерфейс клиентского запроса к серверу ИРБИС64.
    /// </summary>
    public interface IQuery
    {
        /// <summary>
        /// Добавление строки с целым числом (плюс перевод строки).
        /// </summary>
        void Add (int value);

        /// <summary>
        /// Добавление строки в кодировке ANSI (плюс перевод строки).
        /// </summary>
        void AddAnsi (string? value);

        /// <summary>
        /// Добавление строки в кодировке UTF-8 (плюс перевод строки).
        /// </summary>
        void AddUtf (string? value);

        /// <summary>
        /// Добавление формата.
        /// </summary>
        void AddFormat (string? format);

        /// <summary>
        /// Отладочная печать.
        /// </summary>
        void Debug (TextWriter writer);

        /// <summary>
        /// Получение массива фрагментов, из которых состоит
        /// клиентский запрос.
        /// </summary>
        byte[] GetBody();

        /// <summary>
        /// Подсчет общей длины запроса (в байтах).
        /// </summary>
        int GetLength();

        /// <summary>
        /// Добавление одного перевода строки.
        /// </summary>
        void NewLine();
    } // interface IQuery
} // namespace ManagedIrbis.Infrastructrue
