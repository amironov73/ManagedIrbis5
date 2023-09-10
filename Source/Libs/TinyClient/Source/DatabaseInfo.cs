// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

/* DatabaseInfo.cs -- информация о базе данных
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;

#endregion

namespace ManagedIrbis
{
    /// <summary>
    /// Информация о базе данных ИРБИС.
    /// </summary>
    public sealed class DatabaseInfo
    {
        #region Constants

        /// <summary>
        /// Разделитель элементов
        /// </summary>
        public const char ItemDelimiter = (char)0x1E;

        #endregion

        #region Properties

        /// <summary>
        /// Имя базы данных.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Описание базы данных
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Максимальный MFN.
        /// </summary>
        public int MaxMfn { get; set; }

        /// <summary>
        /// Список логически удаленных записей.
        /// </summary>
        public int[]? LogicallyDeletedRecords { get; set; }

        /// <summary>
        /// Список физически удаленных записей.
        /// </summary>
        public int[]? PhysicallyDeletedRecords { get; set; }

        /// <summary>
        /// Список неактуализированных записей.
        /// </summary>
        public int[]? NonActualizedRecords { get; set; }

        /// <summary>
        /// Список заблокированных записей.
        /// </summary>
        public int[]? LockedRecords { get; set; }

        /// <summary>
        /// Флаг монопольной блокировки базы данных.
        /// </summary>
        public bool DatabaseLocked { get; set; }

        /// <summary>
        /// База данных только для чтения.
        /// </summary>
        public bool ReadOnly { get; set; }

        #endregion

        #region Private members

        private static void _Write
            (
                TextWriter writer,
                string name,
                int[]? mfns
            )
        {
            writer.Write ($"{name}: ");
            writer.WriteLine
                (
                    mfns is null or { Length: 0 }
                        ? "None"
                        : string.Join (", ", mfns)
                );
        }

        private static int[] _ParseLine
            (
                string? text
            )
        {
            if (ReferenceEquals (text, null) || text.Length == 0)
            {
                return Array.Empty<int>();
            }

            var items = text.Split (ItemDelimiter);
            var result = new int[items.Length];
            for (var i = 0; i < items.Length; i++)
            {
                result[i] = int.Parse (items[i]);
            }

            Array.Sort (result);

            return result;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Разбор ответа сервера.
        /// </summary>
        public static DatabaseInfo Parse
            (
                string? name,
                Response response
            )
        {
            var result = new DatabaseInfo
            {
                Name = name,
                LogicallyDeletedRecords = _ParseLine (response.ReadAnsi()),
                PhysicallyDeletedRecords = _ParseLine (response.ReadAnsi()),
                NonActualizedRecords = _ParseLine (response.ReadAnsi()),
                LockedRecords = _ParseLine (response.ReadAnsi()),
                MaxMfn = _ParseLine (response.ReadAnsi())[0],
                DatabaseLocked = _ParseLine (response.ReadAnsi())[0] != 0
            };

            return result;
        }

        /// <summary>
        /// Разбор меню со списком баз данных.
        /// </summary>
        public static DatabaseInfo[] ParseMenu
            (
                MenuFile menu
            )
        {
            var result = new List<DatabaseInfo>();

            foreach (var entry in menu.Entries)
            {
                var readOnly = false;
                var name = entry.Code;
                if (!ReferenceEquals (name, null) && name.Length != 0)
                {
                    if (name.FirstChar() == '-')
                    {
                        readOnly = true;
                        name = name.Substring (1);
                    }

                    var database = new DatabaseInfo
                    {
                        Name = name,
                        Description = entry.Comment,
                        ReadOnly = readOnly
                    };
                    result.Add (database);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Вывод сведений о базе данных.
        /// </summary>
        public void Write
            (
                TextWriter writer
            )
        {
            writer.WriteLine ($"Database: {Name}");
            writer.WriteLine ($"Max MFN={MaxMfn}");
            writer.WriteLine ($"Locked={DatabaseLocked}");
            _Write (writer, "Logically deleted records", LogicallyDeletedRecords);
            _Write (writer, "Physically deleted records", PhysicallyDeletedRecords);
            _Write (writer, "Non-actualized records", NonActualizedRecords);
            _Write (writer, "Locked records", LockedRecords);
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            if (string.IsNullOrEmpty (Description))
            {
                return Name.ToVisibleString();
            }

            return $"{Name} - {Description}";
        }

        #endregion
    }
}
