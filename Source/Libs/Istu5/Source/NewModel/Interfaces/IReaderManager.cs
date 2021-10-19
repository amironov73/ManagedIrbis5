// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UseNameofExpression

/* IReaderManager.cs -- интерфейс менеджера базы читателей
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace Istu.NewModel.Interfaces
{
    /// <summary>
    /// Интерфейс менеджера базы читателей.
    /// По факту обертка над таблицей <c>readers</c>
    /// </summary>
    public interface IReaderManager
        : IDisposable
    {
        /// <summary>
        /// Создание читателя.
        /// </summary>
        int CreateReader (Reader reader);

        /// <summary>
        /// Получение информации о читателе по читательскому билету.
        /// </summary>
        Reader? GetReaderByTicket (string ticket);

        /// <summary>
        /// Получение информации о читателе по читательскому билету и паролю.
        /// </summary>
        Reader? GetReaderByTicketAndPassword (string ticket, string password);

        /// <summary>
        /// Поиск читателя по штрих-коду.
        /// </summary>
        Reader? GetReaderByBarcode (string barcode);

        /// <summary>
        /// Поиск читателя по идентификатору в MIRA.
        /// </summary>
        Reader? GetReaderByIstuID (int id);

        /// <summary>
        /// Поиск читателя по RFID.
        /// </summary>
        Reader? GetReaderByRfid (string rfid);

        /// <summary>
        /// Обновление данных читателя.
        /// </summary>
        void UpdateReaderInfo (Reader reader);

        /// <summary>
        /// Перерегистрация читателя в текущем году.
        /// </summary>
        void Reregister (string ticket);

        /// <summary>
        /// Удаление читателя из базы.
        /// </summary>
        void DeleteReader (string ticket);

        /// <summary>
        /// Проверка существования читателя с указанным номером билета.
        /// </summary>
        bool CheckExistence (string ticket);

        /// <summary>
        /// Проверка, что строка имеет верный синтаксис.
        /// </summary>
        bool ValidateTicketString (string ticket);

        /// <summary>
        /// Проверка, что строка имеет верный синтаксис.
        /// </summary>
        bool ValidateNameString (string name);

        /// <summary>
        /// Проверка пароля (правильно введен или нет?).
        /// </summary>
        bool VerifyPassword (string ticket, string password);

        /// <summary>
        /// Поиск читателей с похожими фамилиями.
        /// </summary>
        Reader[] FindReaders
            (
                ReaderSearchCriteria criteria,
                string mask,
                int max
            );

        /// <summary>
        /// Произвольный поиск.
        /// </summary>
        /// <param name="expression">SQL-выражение.</param>
        Reader[] Search (string expression);

        /// <summary>
        /// Получение фотографии читателя.
        /// </summary>
        byte[]? GetPhoto (string ticket);

        /// <summary>
        /// Установка фотографии для читателя.
        /// </summary>
        void SetPhoto (string ticket, byte[]? photo);

        /// <summary>
        /// Экспорт всех фото в указанную папку.
        /// </summary>
        void ExportPhoto (string path);

        /// <summary>
        /// Получение списка двойников.
        /// </summary>
        string[] GetDopplers();

    } // interface IReaderManager

} // namespace Istu.NewModel.Interfaces
