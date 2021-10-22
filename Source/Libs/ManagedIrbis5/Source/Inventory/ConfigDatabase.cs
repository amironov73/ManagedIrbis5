// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* ConfigDatabase.cs -- обертка над базой данных CONFIG
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

using AM;
using AM.Text;

using ManagedIrbis.Batch;
using ManagedIrbis.Records;

#endregion

#nullable enable

namespace ManagedIrbis.Inventory
{
    /// <summary>
    /// Обертка над базой данных CONFIG.
    /// </summary>
    public sealed class ConfigDatabase
    {
        #region Construction

        /// <summary>
        /// Имя базы данных по умолчанию.
        /// </summary>
        public const string DatabaseName = "CONFIG";

        /// <summary>
        /// Список строк.
        /// </summary>
        public List<ConfigLine> Lines { get; } = new ();

        #endregion

        #region Public methods

        /// <summary>
        /// Проверка, содержит ли база данных указанный номер.
        /// Пустой номер не считается входящим в диапазон.
        /// </summary>
        public bool ContainsNumber (string? text) => ContainsNumber (new NumberText (text));

        /// <summary>
        /// Проверка, содержит ли база данных указанный номер.
        /// Пустой номер не считается входящим в диапазон.
        /// </summary>
        public bool ContainsNumber
            (
                NumberText number
            )
        {
            foreach (var line in Lines)
            {
                if (line.ContainsNumber (number))
                {
                    return true;
                }
            }

            return false;

        } // method ContainsNumber

        /// <summary>
        /// Загрузка строк из синхронного провайдера.
        /// </summary>
        public void LoadLines
            (
                ISyncProvider provider,
                string databaseName = DatabaseName,
                int tag = ConfigLine.Tag,
                RecordConfiguration? configuration = null
            )
        {
            configuration ??= RecordConfiguration.GetDefault();
            var saveDatabase = provider.Database;
            try
            {
                foreach (var record in BatchRecordReader.WholeDatabase (provider, databaseName))
                {
                    var worksheet = configuration.GetIndex (record);
                    if (worksheet.SameString (ConfigLine.ObrabWorksheet))
                    {
                        foreach (var field in record.EnumerateField (tag))
                        {
                            var line = ConfigLine.Parse (field);
                            Lines.Add (line);
                        }

                    } // if

                } // foreach

            } // try

            finally
            {
                provider.Database = saveDatabase;
            }

        } // method LoadLines

        #endregion

    } // class ConfigDatabase

} // namespace ManagedIrbis.Inventory
