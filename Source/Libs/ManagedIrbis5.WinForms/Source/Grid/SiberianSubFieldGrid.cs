// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SiberianSubFieldGrid.cs -- таблица, отображающая подполя MARC-записи
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

using ManagedIrbis.Workspace;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms.Grid
{
    /// <summary>
    /// Таблица, отображающая подполя MARC-записи.
    /// </summary>
    public class SiberianSubFieldGrid
        : SiberianGrid
    {
        #region Properties

        /// <summary>
        /// Column for field value.
        /// </summary>
        public SiberianSubFieldColumn SubFieldColumn { get; private set; }

        /// <summary>
        /// Column for code and title.
        /// </summary>
        public SiberianCodeColumn CodeColumn { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SiberianSubFieldGrid()
        {
            HeaderHeight = 26;

            CodeColumn = CreateColumn<SiberianCodeColumn>();
            CodeColumn.Title = "Subfield";
            CodeColumn.FillWidth = 100;

            SubFieldColumn = CreateColumn<SiberianSubFieldColumn>();
            SubFieldColumn.Title = "Value";
            SubFieldColumn.FillWidth = 100;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Use given WSS.
        /// </summary>
        public void Load
            (
                WssFile worksheet,
                Field field
            )
        {
            Rows.Clear();

            foreach (var item in worksheet.Items)
            {
                var code = item.Tag.FirstChar();
                var subField = field.Subfields.GetFirstSubField(code);
                var line = SiberianSubField.FromWorksheetItem ( item );
                if (!ReferenceEquals(subField, null))
                {
                    line.Value = subField.Value.ToString();
                }

                CreateRow(line);
            }

            Goto(1, 0);
        }

        #endregion

        #region SiberianGrid members

        /// <inheritdoc cref="SiberianGrid.CreateRow()" />
        protected override SiberianRow CreateRow()
        {
            var result = base.CreateRow();
            result.Height = 24;

            return result;
        }

        #endregion

    } // class SiberianSubFieldGrid

} // namespace ManagedIrbis.WinForms.Grid
