// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* InMemoryDatabase.cs -- база данных, расположенная в оперативной памяти
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;

#endregion

#nullable enable

namespace ManagedIrbis.InMemory
{
    /// <summary>
    /// База данных, расположенная в оперативной памяти.
    /// </summary>
    public class InMemoryDatabase
    {
        #region Properties

        /// <summary>
        /// Имя базы данных. Нечувствительно к регистру.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// База заблокирована?
        /// </summary>
        public bool Locked { get; set; }

        /// <summary>
        /// Мастер-файл.
        /// </summary>
        public InMemoryMaster Master { get; }

        /// <summary>
        /// Инвертированный файл.
        /// </summary>
        public InMemoryInverted Inverted { get; }

        /// <summary>
        /// Запрещено вносить изменения в базу?
        /// </summary>
        public bool ReadOnly { get; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public InMemoryDatabase
            (
                string name,
                bool readOnly = false
            )
        {
            Name = name;
            ReadOnly = readOnly;
            Master = new InMemoryMaster();
            Inverted = new InMemoryInverted();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Дамп базы данных.
        /// </summary>
        public void Dump
            (
                TextWriter output
            )
        {
            Master.Dump(output);
            Inverted.Dump(output);
        }

        /// <summary>
        /// Чтение записи.
        /// </summary>
        public Record? ReadRecord (int mfn) => Master.ReadRecord(mfn);

        /// <summary>
        /// Сохранение/обновление записи.
        /// </summary>
        public bool WriteRecord(Record record) => Master.WriteRecord(record);

        #endregion

    } // class InMemoryDatabase

} // namespace ManagedIrbis.InMemory
