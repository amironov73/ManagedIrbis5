// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SiberianFoundGrid.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms.Grid
{
    /// <summary>
    ///
    /// </summary>
    public class SiberianFoundGrid
        : SiberianGrid
    {
        #region Properties

        /// <summary>
        /// Адаптер, подтягивающий записи с сервера.
        /// </summary>
        public RecordAdapter? Adapter { get; set; }

        /// <summary>
        /// MFN.
        /// </summary>
        public SiberianFoundMfnColumn MfnColumn { get; private set; }

        /// <summary>
        /// Checkbox.
        /// </summary>
        public SiberianFoundCheckColumn CheckColumn { get; private set; }

        /// <summary>
        /// Icon.
        /// </summary>
        public SiberianFoundIconColumn IconColumn { get; private set; }

        /// <summary>
        /// Description.
        /// </summary>
        public SiberianFoundDescriptionColumn DescriptionColumn { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public SiberianFoundGrid()
        {
            HeaderHeight = 26;

            MfnColumn = CreateColumn<SiberianFoundMfnColumn>();
            MfnColumn.Title = "MFN";
            MfnColumn.MinWidth = 50;
            MfnColumn.Width = 50;

            CheckColumn = CreateColumn<SiberianFoundCheckColumn>();
            CheckColumn.MinWidth = 20;
            CheckColumn.Width = 20;

            IconColumn = CreateColumn<SiberianFoundIconColumn>();
            IconColumn.MinWidth = 20;
            IconColumn.Width = 20;

            DescriptionColumn = CreateColumn<SiberianFoundDescriptionColumn>();
            DescriptionColumn.FillWidth = 100;
        }

        #endregion

        #region Private members

        // private int _firstLine, _lastLine, _lineCount, _rowCount;

        // private FoundLine[]? _cache;

        #endregion

        #region Public methods

        /// <summary>
        /// Use given WSS.
        /// </summary>
        public void Load
            (
                FoundLine[] lines
            )
        {
            Rows.Clear();

            foreach (var line in lines)
            {
                CreateRow(line);
            }

            Goto(Rows.Count - 1, 0);
        }

        #endregion

    }
}
