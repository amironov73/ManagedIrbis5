// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* ReturnMnu.cs -- обертка над файлом RETURN.MNU
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

using AM;

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis.Menus
{
    /// <summary>
    /// Обертка над файлом RETURN.MNU.
    /// </summary>
    public sealed class ReturnMnu
    {
        #region Constants

        /// <summary>
        /// Default file name.
        /// </summary>
        public const string DefaultFileName = "return.mnu";

        #endregion

        #region Nested classes

        /// <summary>
        /// Item.
        /// </summary>
        public sealed class Item
        {
            #region Properties

            /// <summary>
            /// Date.
            /// </summary>
            public DateTime Date { get; set; }

            /// <summary>
            /// Comment.
            /// </summary>
            public string? Comment { get; set; }

            #endregion

            #region Construction

            /// <summary>
            /// Constructor.
            /// </summary>
            public Item()
            {
            }

            /// <summary>
            /// Constructor.
            /// </summary>
            public Item
                (
                    MenuEntry entry
                )
            {
                Sure.NotNull(entry, nameof(entry));

                string code = entry.Code.ThrowIfNull(nameof(entry.Code));
                Comment = entry.Comment;
                if (code.StartsWith("@"))
                {
                    Date = DateTime.ParseExact
                        (
                            code.Substring(1),
                            "dd.MM.yyyy",
                            CultureInfo.InvariantCulture
                        );
                }
                else
                {
                    Date = DateTime.Today.AddDays(int.Parse(code));
                }
            }

            #endregion

            #region Object members

            /// <inheritdoc cref="object.ToString" />
            public override string ToString()
            {
                return $"{Date.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture)} {Comment}";
            }

            #endregion
        }

        #endregion

        #region Properties

        /// <summary>
        /// Items.
        /// </summary>
        public List<Item> Items { get; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ReturnMnu
            (
                MenuFile menu
            )
        {
            Sure.NotNull(menu, nameof(menu));

            Items = new List<Item>();
            foreach (MenuEntry entry in menu.Entries)
            {
                Items.Add(new Item(entry));
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Read RETURN.MNU from server connection.
        /// </summary>
        public static ReturnMnu FromConnection
            (
                ISyncIrbisProvider connection,
                string fileName = DefaultFileName
            )
        {
            Sure.NotNullNorEmpty(fileName, nameof(fileName));

            var specification = new FileSpecification
                {
                    Path = IrbisPath.MasterFile,
                    Database = StandardDatabases.Readers,
                    FileName = DefaultFileName
                };
            var menu = MenuFile.ReadFromServer(connection, specification);
            menu = menu.ThrowIfNull(nameof(menu));
            var result = new ReturnMnu(menu!);

            return result;
        }

        /// <summary>
        /// Read RETURN.MNU from the local file.
        /// </summary>
        public static ReturnMnu FromFile
            (
                string fileName
            )
        {
            Sure.NotNullNorEmpty(fileName, nameof(fileName));

            var menu = MenuFile.ParseLocalFile(fileName);
            var result = new ReturnMnu(menu);

            return result;
        }

//        /// <summary>
//        /// Read RETURN.MNU from the provider.
//        /// </summary>
////        public static ReturnMnu FromProvider
//            (
//                IrbisProvider provider,
//                string fileName = DefaultFileName
//            )
//        {
//            Sure.NotNull(provider, nameof(provider));
//            Sure.NotNullNorEmpty(fileName, nameof(fileName));
//
//            FileSpecification specification = new FileSpecification
//                (
//                    IrbisPath.MasterFile,
//                    StandardDatabases.Readers,
//                    fileName
//                );
//            MenuFile menu = provider.ReadMenuFile(specification)
//                .ThrowIfNull(nameof(menu));
//            ReturnMnu result = new ReturnMnu(menu);
//
//            return result;
//        }

        #endregion
    }
}
