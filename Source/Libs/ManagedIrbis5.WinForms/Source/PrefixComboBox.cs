// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PrefixComboBox.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Windows.Forms;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms
{
    /// <summary>
    ///
    /// </summary>
    [System.ComponentModel.DesignerCategory("Code")]
    public class PrefixComboBox
        : ComboBox
    {
        #region Properties



        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PrefixComboBox()
        {
            DropDownStyle = ComboBoxStyle.DropDownList;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Fill the combo box with scenarios list.
        /// </summary>
        public void FillWithScenarios
            (
                ISyncProvider connection,
                string database
            )
        {
            /*

            SearchScenario[] scenarios = SearchScenario.LoadSearchScenarios
                (
                    connection,
                    database
                );
            if (ReferenceEquals(scenarios, null))
            {
                // TODO do something
                throw new IrbisException();
            }

            // ReSharper disable once CoVariantArrayConversion
            Items.AddRange(scenarios);

            */
        }

        #endregion
    }
}
