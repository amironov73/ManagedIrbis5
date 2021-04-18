// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Local
// ReSharper disable PropertyCanBeMadeInitOnly.Local
// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* RecordViewGrid.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Linq;
using System.Windows.Forms;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms
{
    /// <summary>
    ///
    /// </summary>
    public partial class RecordViewGrid
        : UserControl
    {
        #region Nested classes

        class FieldInfo
        {
            #region Properties

            public int Tag { get; set; }

            public int Repeat { get; set; }

            public string? Text { get; set; }

            #endregion

            #region Public methods

            public static FieldInfo FromField
                (
                    Field field
                )
            {
                var result = new FieldInfo
                {
                    Tag = field.Tag,
                    Repeat = field.Repeat,
                    Text = field.ToText()
                };

                return result;
            }

            #endregion
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public RecordViewGrid()
        {
            InitializeComponent();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Clear.
        /// </summary>
        public void Clear()
        {
            _grid.DataSource = null;
        }

        /// <summary>
        /// Set record to view.
        /// </summary>
        public void SetRecord
            (
                Record? record
            )
        {
            if (ReferenceEquals(record, null))
            {
                _grid.DataSource = null;
                return;
            }

            _grid.AutoGenerateColumns = false;
            _grid.DataSource = record.Fields
                // ReSharper disable once ConvertClosureToMethodGroup
                .Select(field => FieldInfo.FromField(field))
                .ToArray();
        }

        #endregion
    }
}
