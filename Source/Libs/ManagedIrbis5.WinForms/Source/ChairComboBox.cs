// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* ChairComboBox.cs -- выпадающий список кафедр обслуживания
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Threading.Tasks;
using System.Windows.Forms;

using ManagedIrbis.Readers;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms
{
    /// <summary>
    /// Выпадающий список кафедр обслуживания.
    /// </summary>
    [System.ComponentModel.DesignerCategory("Code")]
    public class ChairComboBox
        : ComboBox
    {
        #region Properties

        /// <summary>
        /// Selected chair.
        /// </summary>
        public ChairInfo? SelectedChair => SelectedItem as ChairInfo;

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ChairComboBox()
        {
            DropDownStyle = ComboBoxStyle.DropDownList;
        } // constructor

        #endregion

        #region Public methods

        /// <summary>
        /// Fill the combo box with chair list.
        /// </summary>
        public void FillWithChairs
            (
                ISyncIrbisProvider connection,
                bool addAllItem = false
            )
        {
            var chairs = ChairInfo.Read
                (
                    connection,
                    ChairInfo.ChairMenu,
                    addAllItem
                );
            Items.Clear();

            // ReSharper disable CoVariantArrayConversion
            Items.AddRange(chairs);
            // ReSharper restore CoVariantArrayConversion
        } // method FillWithChairs

        /// <summary>
        /// Fill the combo box with places list.
        /// </summary>
        public void FillWithPlaces
            (
                SyncConnection connection,
                bool addAllItem = false
            )
        {
            var chairs = ChairInfo.Read
                (
                    connection,
                    ChairInfo.PlacesMenu,
                    addAllItem
                );
            Items.Clear();

            // ReSharper disable CoVariantArrayConversion
            Items.AddRange(chairs);
            // ReSharper restore CoVariantArrayConversion
        } // method FillWithPlaces

        #endregion

    } // class ChairComboBox

} // namespace ManagedIrbis.WinForms
