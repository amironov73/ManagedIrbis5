// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* LiteRecord.cs -- облегченная библиографическая запись
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using AM;
using AM.Collections;

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis
{

    /// <summary>
    /// Облегченная библиографическая запись.
    /// </summary>
    public sealed class LiteRecord
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
        public List<LiteField> Fields { get; } = new ();

        /// <summary>
        /// Описание в произвольной форме (опциональное).
        /// </summary>
        public string? Description { get; set; }

        #endregion
    }
}
