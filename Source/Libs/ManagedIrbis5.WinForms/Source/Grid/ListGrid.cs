// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedType.Global

/* ListGrid.cs -- грид в виртуальном режиме, демонстрирующий IList
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using System.Collections.Generic;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms.Grid
{
    /// <summary>
    /// Грид в виртуальном режиме, демонстрирующий IList.
    /// </summary>
    public class ListGrid<T>
        : VirtualGrid<T>
    {
        #region Nested classes

        /// <summary>
        /// Адаптер, умеющий забирать данные из IList.
        /// </summary>
        class ListAdapter
            : IVirtualAdapter<T>
        {
            #region Properties

            /// <summary>
            /// Список-поставщик данных.
            /// </summary>
            private IList<T>? List { get; }

            #endregion

            #region Construction

            /// <summary>
            /// Конструктор.
            /// </summary>
            public ListAdapter (IList<T>? list) => List = list;

            #endregion

            #region IVirtualAdapter members

            public int TotalLength => List?.Count ?? 0;

            public VirtualData<T>? ReadData
                (
                    int firstLine,
                    int lineCount
                )
            {
                if (List is not { } list)
                {
                    return null;
                }

                if (firstLine >= list.Count || lineCount <= 0)
                {
                    return null;
                }

                lineCount = Math.Min(lineCount, list.Count - firstLine);
                var data = new T[lineCount];
                for (var i = 0; i < lineCount; i++)
                {
                    data[i] = list[firstLine + i];
                }

                var result = new VirtualData<T>
                {
                    Length = lineCount,
                    FirstLine = firstLine,
                    Data = data
                };

                return result;

            } // method ReadData

            #endregion
        }

        #endregion

        #region Properties

        /// <summary>
        /// Список отображаемых объектов.
        /// </summary>
        public IList<T>? List { get; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public ListGrid
            (
                IList<T>? list
            )
            : base (new ListAdapter(list))
        {
            List = list;
        }

        #endregion

    } // class ListGrid

} // namespace ManagedIrbis.WinForms.Grid
