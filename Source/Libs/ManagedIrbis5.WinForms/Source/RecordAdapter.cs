// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* RecordAdapter.cs -- адаптер для отображения записей в гриде
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms
{
    /// <summary>
    /// Адаптер для отображения записей в гриде.
    /// </summary>
    public class RecordAdapter
    {
        #region Properties

        /// <summary>
        /// Binding source.
        /// </summary>
        public BindingSource Source { get; private set; }

        /// <summary>
        /// Current term value.
        /// </summary>
        public int CurrentMfn
        {
            get
            {
                var current = (FoundLine?) Source.Current;
                if (ReferenceEquals(current, null))
                {
                    return 0;
                }

                return current.Mfn;
            }
        }

        /// <summary>
        /// Connection.
        /// </summary>
        public ISyncProvider Connection { get; private set; }

        /// <summary>
        /// First record.
        /// </summary>
        public int First { get;set; }

        /// <summary>
        /// Portion size;
        /// </summary>
        public int Portion { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public RecordAdapter
            (
                ISyncProvider connection
            )
        {
            Source = new BindingSource(Array.Empty<Record>(), null);
            First = 1;
            Portion = 100;
            Connection = connection;

        } // constructor

        #endregion

        #region Private members

        private bool _restricted; // нельзя выходить за пределы загруженных записей?

        #endregion

        #region Public methods

        /// <summary>
        /// Move to next term.
        /// </summary>
        public bool MoveNext()
        {
            var termSource = Source;
            var currencyManager = termSource.CurrencyManager;

            termSource.MoveNext();
            var count = currencyManager.Count;
            if (currencyManager.Position >= count - 1)
            {
                if (!_restricted)
                {
                    return Fill(First + count);
                }
            }

            return true;
            
        } // method MoveNext

        /// <summary>
        /// Move to next term.
        /// </summary>
        public bool MoveNext(int amount)
        {
            while (amount > 0)
            {
                amount--;
                if (!MoveNext())
                {
                    return false;
                }
            }

            return true;

        } // method MoveNext

        /// <summary>
        /// Move to previous term.
        /// </summary>
        public bool MovePrevious()
        {
            var termSource = Source;
            var currencyManager = termSource.CurrencyManager;

            termSource.MovePrevious();
            if (currencyManager.Position < 1)
            {
                if (!_restricted)
                {
                    return Fill(First - Portion, true);
                }
            }

            return true;

        } // method MovePrevious

        /// <summary>
        /// Move to previous term.
        /// </summary>
        public bool MovePrevious
            (
                int amount
            )
        {
            while (amount > 0)
            {
                amount--;
                if (!MovePrevious())
                {
                    return false;
                }
            }

            return true;

        } // method MovePrevious

        /// <summary>
        /// Fill the adapter.
        /// </summary>
        public bool Fill() => Fill(First);

        /// <summary>
        /// Заполнение адаптера найденными записями.
        /// </summary>
        public bool Fill
            (
                IEnumerable<int> found
            )
        {
            Source.DataSource = Array.Empty<FoundLine>();
            _restricted = true;

            var array = found.ToArray();
            if (array.Length == 0)
            {
                return true;
            }

            var records = FoundLine.Read
                (
                    Connection,
                    "@brief",
                    array
                );

            First = array[0];
            Source.DataSource = records;
            Source.Position = 0;

            return true;

        } // method Fill

        /// <summary>
        /// Fill the adapter.
        /// </summary>
        public bool Fill
            (
                int startMfn,
                bool backward = false
            )
        {
            _restricted = false;
            var list = new List<int>(Portion);
            var max = Connection.GetMaxMfn();
            var first = startMfn;

            if (first <= 0)
            {
                first = 1;
            }

            var current = first;
            for (var i = 0; i < Portion; i++)
            {
                if (current >= max)
                {
                    break;
                }
                list.Add(current);
                current++;
            }

            if (list.Count < 1)
            {
                return false;
            }

            var records = FoundLine.Read
                (
                    Connection,
                    "@brief",
                    list
                );

            if (records.Length < 1)
            {
                return false;
            }

            First = first;
            Source.DataSource = records;
            if (backward)
            {
                Source.Position = Source.Count - 1;
            }
            else
            {
                Source.Position = 0;
            }

            return true;

        } // method Fill

        #endregion

    } // class RecordAdapter

} // namespace ManagedIrbis.WinForms
