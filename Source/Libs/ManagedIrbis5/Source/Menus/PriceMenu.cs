// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* PriceMenu.cs -- обертка над файлом IZC.MNU
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;

using AM;

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis.Menus
{
    //
    // Выдержка из официальной документации:
    //
    // При выполнении пополнения записи КСУ, при формировании выходных форм,
    // а также в задаче «Проверка фонда», подсчет цены выбывшего
    // (или проверенного) экземпляра книги и итоговых сумм выбытия
    // осуществляется с учетом индекса изменения цен текущего года
    // относительно цен года приобретения экземпляра.
    //
    // Индексы изменения цен или коэффициенты пересчета цен (КПЦ)
    // по годам представлены в справочнике IZC.mnu, находящемся в БД ЭК.
    // В исходном состоянии IZC.mnu содержит КПЦ, начиная с 1980 года,
    // по состоянию на 2007 год.

    /// <summary>
    /// Обертка над файлом IZC.MNU.
    /// </summary>
    public sealed class PriceMenu
    {
        #region Constants

        /// <summary>
        /// Default file name.
        /// </summary>
        public const string DefaultFileName = "IZC.MNU";

        #endregion

        #region Nested classes

        /// <summary>
        /// Item.
        /// </summary>
        public sealed class Item
            : IComparable<Item>
        {
            #region Properties

            /// <summary>
            /// Date.
            /// </summary>
            public string? Date { get; set; }

            /// <summary>
            /// Coefficient.
            /// </summary>
            public decimal? Coefficient { get; set; }

            #endregion

            #region Public methods

            /// <summary>
            /// Parse the menu entry.
            /// </summary>
            public static Item Parse
                (
                    MenuEntry entry
                )
            {
                var date = entry.Code;
                if (string.IsNullOrEmpty(date)
                    || ((date.Length != 4) && (date.Length != 6)))
                {
                    throw new IrbisException();
                }

                var comment = entry.Comment;
                if (string.IsNullOrEmpty(comment))
                {
                    throw new IrbisException();
                }

                var coefficient = comment.ParseDecimal();

                var result = new Item
                {
                    Date = date,
                    Coefficient = coefficient
                };

                return result;
            }

            #endregion

            #region IComparable<Item> members

            /// <inheritdoc cref="IComparable{T}.CompareTo"/>
            public int CompareTo
                (
                    Item? other
                )
            {
                return _Compare(Date, other.Date);
            }

            #endregion
        }

        #endregion

        #region Properties

        /// <summary>
        /// List of items.
        /// </summary>
        public List<Item> Items { get; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PriceMenu
            (
                MenuFile menu
            )
        {
            Items = new List<Item>();
            foreach (var entry in menu.Entries)
            {
                var item = Item.Parse(entry);
                Items.Add(item);
            }

            Items.Sort
                (
                    (left, right) => _Compare(left.Date, right.Date)
                );
        }

        #endregion

        #region Private members

        private static int _Compare(string left, string right)
        {
            return string.Compare
                (
                    left,
                    right,
                    StringComparison.Ordinal
                );
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Read IZC.MNU from the local file.
        /// </summary>
        public static PriceMenu FromFile
            (
                string fileName
            )
        {
            var menu = MenuFile.ParseLocalFile(fileName);
            var result = new PriceMenu(menu);

            return result;
        }

        /// <summary>
        /// Read IZC.MNU from the provider.
        /// </summary>
        public static PriceMenu ReadPrices
            (
                ISyncIrbisProvider provider,
                string fileName
            )
        {
            var specification = new FileSpecification
                {
                    Path = IrbisPath.MasterFile,
                    Database = provider.Database,
                    FileName = fileName
                };
            var menu = provider.RequireMenu(specification);
            var result = new PriceMenu(menu);

            return result;
        }

        /// <summary>
        /// Получение коэффициента пересчета для указанной даты.
        /// </summary>
        public decimal GetCoefficient
            (
                string? date
            )
        {
            if (string.IsNullOrEmpty(date))
            {
                return 1.0m;
            }

            var result = 1.0m;
            if (Items.Count != 0)
            {
                result = Items.Min().Coefficient.Value;
            }

            foreach (var item in Items)
            {
                if (_Compare(date, item.Date) >= 0)
                {
                    result = item.Coefficient.Value;
                }
            }

            return result;
        }

        /// <summary>
        /// Есть ли у нас коэффициент пересчета для указанной даты?
        /// </summary>
        public bool HaveCoefficient
            (
                string? date
            )
        {
            if (string.IsNullOrEmpty(date))
            {
                return true;
            }

            foreach (var item in Items)
            {
                if (item.Date.StartsWith(date))
                {
                    return true;
                }
            }

            return false;
        }

        #endregion

    } // class PriceMenu

} // namespace ManagedIrbis.Menus
