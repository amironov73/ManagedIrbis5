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

using AM.Windows.Forms;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms
{
    /// <summary>
    /// Адаптер для отображения записей в гриде.
    /// </summary>
    public class RecordAdapter
        : VirtualAdapter
    {
        #region Properties

        /// <summary>
        /// Подключение.
        /// </summary>
        public ISyncProvider Connection { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public RecordAdapter
            (
                ISyncProvider connection
            )
        {
            Connection = connection;

        } // constructor

        #endregion

        #region Private members

        private int[]? _found;

        #endregion

        #region Public methods

        /// <summary>
        /// Ограничение только перечисленными найденными записями.
        /// </summary>
        /// <param name="found"></param>
        public void Fill(int[]? found) => _found = found;

        #endregion

        #region VirtualAdapter methods

        /// <inheritdoc cref="VirtualAdapter.ByIndex"/>
        public override object? ByIndex (object? value, int index) =>
            value is not FoundLine found
                ? default
                : index switch
                {
                    0 => found.Mfn,
                    1 => found.Selected,
                    2 => found.Icon,
                    3 => found.Description,
                    _ => default
                };

        /// <inheritdoc cref="VirtualAdapter.Clear"/>
        public override void Clear()
        {
            _found = default;
        }

        /// <inheritdoc cref="VirtualAdapter.PullData"/>
        public override VirtualData PullData
            (
                int firstLine,
                int lineCount
            )
        {
            // TODO: отрабатывать случаи lineCount <= 0

            var maxMfn = Connection.GetMaxMfn();
            if (_found is null)
            {
                if (firstLine + lineCount > maxMfn)
                {
                    lineCount = maxMfn - firstLine;
                }
            }
            else
            {
                var length = _found.Length;
                if (firstLine + lineCount > length)
                {
                    lineCount = length - lineCount;
                }
            }

            var mfnList = new List<int>(lineCount);
            for (var i = 0; i < lineCount; i++)
            {
                var mfn = _found is null
                    ? firstLine + i + 1
                    : _found[i];
                mfnList.Add(mfn);
            }

            var records = FoundLine.Read
                (
                    Connection,
                    "@brief",
                    mfnList
                );

            var result = new VirtualData
            {
                FirstLine = firstLine,
                LineCount = records.Length,
                TotalCount = maxMfn,
                Lines = records
            };

            return result;

        } // method PullData

        #endregion

    } // class RecordAdapter

} // namespace ManagedIrbis.WinForms
