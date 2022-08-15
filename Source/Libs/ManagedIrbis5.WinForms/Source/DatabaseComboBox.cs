// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable CoVariantArrayConversion
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* DatabaseComboBox.cs -- выпадающий список баз данных
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Windows.Forms;

using AM;

using ManagedIrbis.Providers;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms;

/// <summary>
/// Выпадающий список баз данных.
/// </summary>
[System.ComponentModel.DesignerCategory ("Code")]
public sealed class DatabaseComboBox
    : ComboBox
{
    #region Properties

    /// <summary>
    /// Выбранная пользователем база данных.
    /// </summary>
    public DatabaseInfo? SelectedDatabase => SelectedItem as DatabaseInfo;

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public DatabaseComboBox()
    {
        DropDownStyle = ComboBoxStyle.DropDownList;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Заполнение комбобокса списком баз данных.
    /// </summary>
    public void FillWithDatabases
        (
            ISyncProvider connection,
            string listFile
        )
    {
        Sure.NotNull (connection);
        Sure.NotNullNorEmpty (listFile);
        connection.EnsureConnected();

        var databases = connection.ListDatabases (listFile);
        Items.Clear();
        Items.AddRange (databases);
    }

    #endregion
}
