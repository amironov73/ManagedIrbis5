// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftVariableGrid.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using ManagedIrbis.Pft.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms.Pft
{
    /// <summary>
    ///
    /// </summary>
    public partial class PftVariableGrid
        : UserControl
    {
        #region Nested classes

        class VariableInfo
        {
            #region Properties

            public string Name { get; set; }

            public object Value { get; set; }

            #endregion

            #region Public methods

            public static VariableInfo FromVariable
                (
                    PftVariable variable
                )
            {
                object value = variable.StringValue;
                if (variable.IsNumeric)
                {
                    value = variable.NumericValue;
                }

                var result = new VariableInfo
                {
                    Name = variable.Name,
                    Value = value
                };

                return result;
            }

            #endregion
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftVariableGrid()
        {
            InitializeComponent();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Clear.
        /// </summary>
        public void Clear()
        {
            _grid.DataSource = null;
        }

        /// <summary>
        /// Set variables.
        /// </summary>
        public void SetVariables
            (
                PftVariableManager manager
            )
        {
            var all = manager.GetAllVariables();
            var list = new List<VariableInfo>();
            foreach (var variable in all)
            {
                var info = VariableInfo.FromVariable(variable);
                list.Add(info);
            }
            _grid.AutoGenerateColumns = false;
            _grid.DataSource = list.OrderBy(v => v.Name).ToArray();
        }

        #endregion
    }
}
