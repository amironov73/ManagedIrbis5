// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftGlobalGrid.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
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
    public partial class PftGlobalGrid
        : UserControl
    {
        #region Nested classes

        class GlobalInfo
        {
            #region Properties

            public int Number { get; set; }

            public string Value { get; set; }

            #endregion

            #region Public methods

            public static GlobalInfo FromGlobal
                (
                    PftGlobal variable
                )
            {
                var value = string.Join
                    (
                        Environment.NewLine,
                        variable.Fields.Select
                            (
                                f => f.ToText()
                            )
                    );

                var result = new GlobalInfo
                {
                    Number = variable.Number,
                    Value = value
                };

                return result;
            }

            #endregion
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor
        /// </summary>
        public PftGlobalGrid()
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
        /// Set globals.
        /// </summary>
        public void SetGlobals
            (
                PftGlobalManager manager
            )
        {
            var list = new List<GlobalInfo>();
            foreach (var variable in manager.GetAllVariables())
            {
                var item = GlobalInfo.FromGlobal(variable);
                list.Add(item);
            }

            _grid.AutoGenerateColumns = false;
            _grid.DataSource = list.ToArray();
        }

        #endregion
    }
}
