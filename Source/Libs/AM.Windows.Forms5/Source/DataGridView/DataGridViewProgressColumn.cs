// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* DataGridViewProgressColumn.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms
{
    /// <summary>
    ///
    /// </summary>
    public class DataGridViewProgressColumn
        : DataGridViewColumn
    {
        #region Properties

        /// <summary>
        /// Gets or sets the maximum.
        /// </summary>
        /// <value>The maximum.</value>
        [DefaultValue(100)]
        public int Maximum
        {
            get
            {
                return ((DataGridViewProgressCell)CellTemplate).Maximum;
            }
            set
            {
                ((DataGridViewProgressCell)CellTemplate).Maximum = value;
            }
        }

        /// <summary>
        /// Gets or sets the minimum.
        /// </summary>
        /// <value>The minimum.</value>
        [DefaultValue(0)]
        public int Minimum
        {
            get
            {
                return ((DataGridViewProgressCell)CellTemplate).Minimum;
            }
            set
            {
                ((DataGridViewProgressCell)CellTemplate).Minimum = value;
            }
        }


        #endregion

        #region Construction

        /// <summary>
        /// Default constructor.
        /// </summary>
        public DataGridViewProgressColumn()
            : base(new DataGridViewProgressCell())
        {
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion
    }
}
