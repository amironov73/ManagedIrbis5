// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* RecordAdapter.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Windows.Forms;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms
{
    /// <summary>
    ///
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
                var record = (Record?)Source.Current;
                if (ReferenceEquals(record, null))
                {
                    return 0;
                }

                return record.Mfn;
            }
        }

        /// <summary>
        /// Connection.
        /// </summary>
        public ISyncIrbisProvider Connection { get; private set; }

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
                ISyncIrbisProvider connection
            )
        {
            Source = new BindingSource(Array.Empty<Record>(), null);
            First = 1;
            Portion = 100;
            Connection = connection;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Move to next term.
        /// </summary>
        public bool MoveNext()
        {
            BindingSource termSource = Source;
            CurrencyManager currencyManager = termSource.CurrencyManager;

            termSource.MoveNext();
            int count = currencyManager.Count;
            if (currencyManager.Position >= count - 1)
            {
                return Fill(First + count);
            }

            return true;
        }

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
        }

        /// <summary>
        /// Move to previous term.
        /// </summary>
        public bool MovePrevious()
        {
            BindingSource termSource = Source;
            CurrencyManager currencyManager = termSource.CurrencyManager;

            termSource.MovePrevious();
            if (currencyManager.Position < 1)
            {
                return Fill(First - Portion, true);
            }

            return true;
        }

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
        }

        /// <summary>
        /// Fill the adapter.
        /// </summary>
        public bool Fill()
        {
            return Fill(First);
        }

        /// <summary>
        /// Fill the adapter.
        /// </summary>
        public bool Fill
            (
                int startMfn,
                bool backward = false
            )
        {
            List<int> list = new List<int>(Portion);
            int max = Connection.GetMaxMfn();
            int first = startMfn;

            if (first <= 0)
            {
                first = 1;
            }

            int current = first;
            for (int i = 0; i < Portion; i++)
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

            /*
            FoundItem[] records = FoundItem.Read
                (
                    Connection,
                    "@brief",
                    list
                );

                */

            var records = Array.Empty<FoundItem>();

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
        }

        #endregion
    }
}
