// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable CoVariantArrayConversion
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* ChairComboBox.cs -- выпадающий список кафедр обслуживания
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Windows.Forms;

using AM;

using ManagedIrbis.Providers;
using ManagedIrbis.Readers;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms;

/// <summary>
/// Выпадающий список кафедр обслуживания.
/// </summary>
[System.ComponentModel.DesignerCategory ("Code")]
public sealed class ChairComboBox
    : ComboBox
{
    #region Properties

    /// <summary>
    /// Выбранная кафедра.
    /// </summary>
    public ChairInfo? SelectedChair => SelectedItem as ChairInfo;

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public ChairComboBox()
    {
        DropDownStyle = ComboBoxStyle.DropDownList;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Заполнение комбобокса списком кафедр.
    /// </summary>
    public void FillWithChairs
        (
            ISyncProvider connection,
            bool addAllItem = false
        )
    {
        Sure.NotNull (connection);
        connection.EnsureConnected();

        var chairs = ChairInfo.Read
            (
                connection,
                ChairInfo.ChairMenu,
                addAllItem
            );
        Items.Clear();
        Items.AddRange (chairs);
    }

    /// <summary>
    /// Заполнение комбобокса списокм мест обслуживания.
    /// Fill the combo box with places list.
    /// </summary>
    public void FillWithPlaces
        (
            SyncConnection connection,
            bool addAllItem = false
        )
    {
        Sure.NotNull (connection);
        connection.EnsureConnected();

        var chairs = ChairInfo.Read
            (
                connection,
                ChairInfo.PlacesMenu,
                addAllItem
            );
        Items.Clear();
        Items.AddRange (chairs);
    }

    #endregion
}
