// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SiberianCheckColumn.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Linq;
using System.Windows.Forms;

using AM.Reflection;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms.Grid
{
    /// <summary>
    ///
    /// </summary>
    public class SiberianCheckColumn
        : SiberianColumn
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SiberianCheckColumn()
        {
            //BackColor = Color.White;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region SiberianColumn members

        /// <inheritdoc/>
        public override SiberianCell CreateCell()
        {
            SiberianCell result = new SiberianCheckCell();
            result.Column = this;

            return result;
        }

        /// <inheritdoc />
        public override Control? CreateEditor
            (
                SiberianCell cell,
                bool edit,
                object state
            )
        {
            return null;
        }

        /// <inheritdoc />
        public override void GetData
            (
                object theObject,
                SiberianCell cell
            )
        {
            var checkCell = (SiberianCheckCell)cell;

            if (!string.IsNullOrEmpty(Member)
                && !ReferenceEquals(theObject, null))
            {
                var type = theObject.GetType();
                var memberInfo = type.GetMember(Member)
                    .First();
                var property = new PropertyOrField
                    (
                        memberInfo
                    );

                var value = property.GetValue(theObject);
                checkCell.State = ReferenceEquals(value, null)
                    ? false
                    : (bool) value;
            }
        }

        /// <inheritdoc />
        public override void PutData
            (
                object theObject,
                SiberianCell cell
            )
        {
            var checkCell = (SiberianCheckCell)cell;

            if (!string.IsNullOrEmpty(Member)
                && !ReferenceEquals(theObject, null))
            {
                var type = theObject.GetType();
                var memberInfo = type.GetMember(Member)
                    .First();
                var property = new PropertyOrField
                    (
                        memberInfo
                    );

                property.SetValue(theObject, checkCell.State);
            }
        }

        #endregion

    }
}
