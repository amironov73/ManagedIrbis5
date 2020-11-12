// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* MarcRecord.cs -- библиографическая запись
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Библиографическая запись. Состоит из произвольного количества полей.
    /// </summary>
    public class Record
    {
        #region Properties

        /// <summary>
        /// Имя базы данных, в которой хранится запись.
        /// </summary>
        public string? Database { get; set; }

        /// <summary>
        /// MFN записи.
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
        /// Список полей.
        /// </summary>
        public List<Field> Fields { get; } = new List<Field>();

        /// <summary>
        /// Описание в произвольной форме (опциональное).
        /// </summary>
        public string? Description { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Добавление поля в запись.
        /// </summary>
        /// <returns>
        /// Свежедобавленное поле.
        /// </returns>
        public Field Add
            (
                int tag,
                string? value = null
            )
        {
            var result = new Field { Tag = tag, Value = value };
            Fields.Add(result);

            return result;
        }

        /// <summary>
        /// Очистка записи (удаление всех полей).
        /// </summary>
        /// <returns>
        /// Очищенную запись.
        /// </returns>
        public Record Clear()
        {
            Fields.Clear();

            return this;
        }

        #endregion
    }
}
