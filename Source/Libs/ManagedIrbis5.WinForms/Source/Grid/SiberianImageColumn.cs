// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SiberianImageColumn.cs -- колонка, отображающая картинки
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using AM.Reflection;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms.Grid
{
    /// <summary>
    /// Колонка, отображающая картинки.
    /// </summary>
    public class SiberianImageColumn
        : SiberianColumn
    {
        #region SiberianColumn members

        /// <inheritdoc/>
        public override SiberianCell CreateCell()
        {
            SiberianCell result = new SiberianImageCell();
            result.Column = this;

            return result;
        }

        /// <inheritdoc cref="SiberianColumn.CreateEditor" />
        public override Control? CreateEditor
            (
                SiberianCell cell,
                bool edit,
                object? state
            )
        {
            return default;
        }

        /// <inheritdoc cref="SiberianColumn.GetData" />
        public override void GetData
            (
                object? theObject,
                SiberianCell cell
            )
        {
            var imageCell = (SiberianImageCell)cell;

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
                imageCell.Picture = (Image?) value;
            }
        }

        /// <inheritdoc cref="SiberianColumn.PutData" />
        public override void PutData
            (
                object? theObject,
                SiberianCell cell
            )
        {
            var imageCell = (SiberianImageCell)cell;

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

                property.SetValue(theObject, imageCell.Picture);
            }
        }

        #endregion

    } // class SiberianImageColumn

} // namespace ManagedIrbis.WinForms.Grid
