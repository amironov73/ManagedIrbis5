// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* SyncQuery.cs -- клиентский запрос к серверу ИРБИС64 (для синхронного сценария)
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;

#endregion

#nullable enable

namespace ManagedIrbis.Infrastructure
{
    //
    // В синхронном сценарии у нас есть возможность
    // использовать структуру, чтобы избежать трафика памяти
    //
    // Может показаться, что мы допускаем ошибку, т. к.
    // в принимающем методе будет использоваться копия структуры,
    // но на самом деле все ОК, т. к. единственное поле
    // нашей структуры - указатель на поток, в который
    // и происходит добавление данных. Так что все добавленные
    // данные разделяются между всеми копиями структуры.
    //
    // Оверхеда на копирование нет, т. к. копирование этой
    // структуры равноценно передаче простого указателя
    // на поток.
    //
    // Единственная беда -- вынужденное дублирование кода.
    // Его, насколько смог, победил с помощью класса
    // QueryStream.
    //

    /// <summary>
    /// Клиентский запрос к серверу ИРБИС64
    /// (для синхронного сценария).
    /// </summary>
    public readonly struct SyncQuery
        : IQuery
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SyncQuery
            (
                IConnectionSettings connection,
                string commandCode
            )
            : this()
        {
            _stream = new QueryStream(1024);
            _stream.AddHeader(connection, commandCode);
        } // constructor

        #endregion

        #region Private members

        private readonly QueryStream _stream;

        #endregion

        #region Public methods

        /// <summary>
        /// Добавление строки с целым числом (плюс перевод строки).
        /// </summary>
        public void Add (int value) => _stream.Add(value);

        /// <summary>
        /// Добавление строки в кодировке ANSI (плюс перевод строки).
        /// </summary>
        public void AddAnsi (string? value) => _stream.AddAnsi(value);

        /// <summary>
        /// Добавление строки в кодировке UTF-8 (плюс перевод строки).
        /// </summary>
        public void AddUtf (string? value) => _stream.AddUtf(value);

        /// <summary>
        /// Добавление формата.
        /// </summary>
        public void AddFormat (string? format) => _stream.AddFormat(format);

        /// <summary>
        /// Отладочная печать.
        /// </summary>
        public void Debug (TextWriter writer) => _stream.Debug(writer);

        /// <summary>
        /// Отладочная печать.
        /// </summary>
        public void DebugUtf (TextWriter writer) => _stream.DebugUtf(writer);

        /// <summary>
        /// Получение массива фрагментов, из которых состоит
        /// клиентский запрос.
        /// </summary>
        public byte[] GetBody() => _stream.GetBody();

        /// <summary>
        /// Подсчет общей длины запроса (в байтах).
        /// </summary>
        public int GetLength() => _stream.GetLength();

        /// <summary>
        /// Добавление одного перевода строки.
        /// </summary>
        public void NewLine() => _stream.NewLine();

        #endregion

    } // struct SyncQuery

} // namespace ManagedIrbis.Infrastructure
