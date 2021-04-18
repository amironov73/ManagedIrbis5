// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SiberianIntegerColumn.cs -- колонка, отображающая целые числа
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
    /// Колонка, отображающая целые числа.
    /// </summary>
    public class SiberianIntegerColumn
        : SiberianColumn
    {
        #region SiberianColumn members

        /// <inheritdoc/>
        public override SiberianCell CreateCell()
        {
            SiberianCell result = new SiberianIntegerCell();
            result.Column = this;

            return result;
        }

        /// <inheritdoc />
        public override Control? CreateEditor
            (
                SiberianCell cell,
                bool edit,
                object? state
            )
        {
            return null;
        }

        /// <inheritdoc />
        public override void GetData
            (
                object? theObject,
                SiberianCell cell
            )
        {
            var integerCell = (SiberianIntegerCell)cell;

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
                integerCell.Value = (int?) value ?? 0;
            }
        }

        /// <inheritdoc />
        public override void PutData
            (
                object? theObject,
                SiberianCell cell
            )
        {
            var integerCell = (SiberianIntegerCell)cell;

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

                property.SetValue(theObject, integerCell.Value);
            }
        }

        #endregion

    } // class SiberianIntegerColumn

} // namespace ManagedIrbis.WinForms.Grid
