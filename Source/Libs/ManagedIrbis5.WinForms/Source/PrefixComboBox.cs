// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable CoVariantArrayConversion
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PrefixComboBox.cs -- выпадающий список поисковых сценариев
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Windows.Forms;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Providers;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms
{
    /// <summary>
    /// Выпадающий список поисковых сценариев.
    /// </summary>
    [System.ComponentModel.DesignerCategory("Code")]
    public class PrefixComboBox
        : ComboBox
    {
        #region Properties

        /// <summary>
        /// Выбранный пользователем поисковый сценарий.
        /// </summary>
        public SearchScenario? SelectedScenario => SelectedItem as SearchScenario;

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
                ISyncProvider provider,
                string database
            )
        {
            Items.Clear();

            // TODO: использовать более интеллектуальный поиск сценариев

            var specification = new FileSpecification
            {
                Path = IrbisPath.MasterFile,
                Database = database,
                FileName = $"{database}.ini"
            };
            var iniFile = provider.ReadIniFile(specification);
            if (iniFile is null)
            {
                if (provider is SyncConnection connection)
                {
                    iniFile = connection.IniFile;
                }
            }

            if (iniFile is null)
            {
                return;
            }

            var scenarios = SearchScenario.ParseIniFile(iniFile);
            Items.AddRange(scenarios);
            if (scenarios.Length != 0)
            {
                SelectedIndex = 0;
            }

        } // method FillWithScenarios

        #endregion

    } // class PrefixComboBox

} // namespace ManagedIrbis.WinForms
