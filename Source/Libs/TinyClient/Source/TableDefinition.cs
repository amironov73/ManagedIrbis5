// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global

/* TableDefinition.cs -- параметры для команды построения таблицы
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

#endregion

namespace ManagedIrbis
{
    /// <summary>
    /// Параметры для команды построения таблицы.
    /// </summary>
    public sealed class TableDefinition
    {
        #region Properties

        /// <summary>
        /// Имя базы данных.
        /// </summary>
        public string? DatabaseName { get; set; }

        /// <summary>
        /// Имя таблицы.
        /// </summary>
        public string? Table { get; set; }

        /// <summary>
        /// Заголовки таблицы.
        /// </summary>
        public List<string> Headers { get; } = new ();

        /// <summary>
        /// Mode.
        /// </summary>
        public string? Mode { get; set; }

        /// <summary>
        /// Search query.
        /// </summary>
        public string? SearchQuery { get; set; }

        /// <summary>
        /// Minimal MFN.
        /// </summary>
        public int MinMfn { get; set; }

        /// <summary>
        /// Maximal MFN.
        /// </summary>
        public int MaxMfn { get; set; }

        /// <summary>
        /// Optional sequential query.
        /// </summary>
        public string? SequentialQuery { get; set; }

        /// <summary>
        /// List of MFN.
        /// </summary>
        public List<int> MfnList { get; } = new ();

        #endregion

        #region Public methods

        /// <summary>
        /// Кодирование запроса.
        /// </summary>
        public void Encode
            (
                SyncQuery query
            )
        {
            query.AddAnsi (Table);
            query.NewLine(); // вместо заголовков
            query.AddAnsi (Mode);
            query.AddUtf (SearchQuery);
            query.Add (MinMfn);
            query.Add (MaxMfn);
            query.AddUtf (SequentialQuery);
            query.NewLine(); // вместо списка MFN
        }

        #endregion
    }
}
