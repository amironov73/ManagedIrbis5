// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SiberianDoubleColumn.cs -- колонка, отображающая числа с плавающей точкой
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
    /// Колонка, отображающая числа с плавающей точкой двойной точности.
    /// </summary>
    public class SiberianDoubleColumn
        : SiberianColumn
    {
        #region SiberianColumn members

        /// <inheritdoc cref="SiberianColumn.CreateCell" />
        public override SiberianCell CreateCell()
        {
            SiberianCell result = new SiberianDoubleCell();
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

        /// <inheritdoc />
        public override void GetData
            (
                object? theObject,
                SiberianCell cell
            )
        {
            var doubleCell = (SiberianDoubleCell)cell;

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
                doubleCell.Value = (double?) value ?? 0;
            }
        }

        /// <inheritdoc />
        public override void PutData
            (
                object? theObject,
                SiberianCell cell
            )
        {
            var doubleCell = (SiberianDoubleCell)cell;

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

                property.SetValue(theObject, doubleCell.Value);
            }
        }

        #endregion

    } // class SiberianDoubleColumn

} // namespace ManagedIrbis.WinForms.Grid
