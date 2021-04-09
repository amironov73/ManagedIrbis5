// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SiberianFieldGrid.cs --
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
    ///
    /// </summary>
    public class SiberianFieldGrid
        : SiberianGrid
    {
        #region Properties

        /// <summary>
        /// Column for field value.
        /// </summary>
        public SiberianFieldColumn FieldColumn { get; private set; }

        /// <summary>
        /// Column for repeat number.
        /// </summary>
        public SiberianRepeatColumn RepeatColumn { get; private set; }

        /// <summary>
        /// Column for tag and title.
        /// </summary>
        public SiberianTagColumn TagColumn { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SiberianFieldGrid()
        {
            HeaderHeight = 26;

            TagColumn = CreateColumn<SiberianTagColumn>();
            TagColumn.Title = "Field";
            TagColumn.FillWidth = 100;

            RepeatColumn = CreateColumn<SiberianRepeatColumn>();

            FieldColumn = CreateColumn<SiberianFieldColumn>();
            FieldColumn.Title = "Value";
            FieldColumn.FillWidth = 100;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Use given page.
        /// </summary>
        public void Load
            (
                WorksheetPage page,
                Record record
            )
        {
            Rows.Clear();

            foreach (WorksheetItem item in page.Items)
            {
                var repeat = 1;

                var found = record.Fields.GetField
                    (
                        item.Tag.SafeToInt32()
                    );
                if (found.Length == 0)
                {
                    var line = SiberianField.FromWorksheetItem
                        (
                            item
                        );
                    line.Repeat = repeat;
                    CreateRow(line);
                }
                else
                {
                    foreach (var field in found)
                    {
                        var line = SiberianField.FromWorksheetItem
                            (
                                item
                            );
                        line.Repeat = repeat;
                        line.Value = field.ToText();
                        if (repeat != 1)
                        {
                            line.Title = "--//--";
                        }
                        repeat++;
                        CreateRow(line);
                    }
                }
            }

            Goto(2, 0);
        }

        #endregion

        #region SiberianGrid members

        /// <inheritdoc/>
        protected override SiberianRow CreateRow()
        {
            var result = base.CreateRow();
            result.Height = 24;

            return result;
        }

        #endregion

    }
}
