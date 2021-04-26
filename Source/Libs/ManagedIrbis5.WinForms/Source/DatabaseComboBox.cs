// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* DatabaseComboBox.cs -- выпадающий список баз данных
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Windows.Forms;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms
{
    /// <summary>
    /// Выпадающий список баз данных.
    /// </summary>
    [System.ComponentModel.DesignerCategory("Code")]
    public class DatabaseComboBox
        : ComboBox
    {
        #region Properties

        /// <summary>
        /// Selected chair.
        /// </summary>
        public DatabaseInfo? SelectedDatabase => SelectedItem as DatabaseInfo;

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public DatabaseComboBox()
        {
            DropDownStyle = ComboBoxStyle.DropDownList;
        } // constructor

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Fill the combo box with databases list.
        /// </summary>
        public void FillWithDatabases
            (
                ISyncProvider connection,
                string listFile
            )
        {
            // DatabaseInfo[] databases = connection.ListDatabases(listFile);
            //
            // Items.AddRange(databases);
        } // method FillWithDatabases

        #endregion

    } // class DatabaseComboBox

} // namespace ManagedIrbis.WinForms
