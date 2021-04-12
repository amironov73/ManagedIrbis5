// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* TreeGridTextColumn.cs
 * Ars Magna project, http://arsmagna.ru
 */

#region Using drectives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

#nullable enable

namespace AM.Windows.Forms
{
    /// <summary>
    ///
    /// </summary>
    public class TreeGridTextColumn
        : TreeGridColumn
    {
        #region Construction

        //public TreeGridTextColumn()
        //{
        //}

        #endregion

        #region TreeGridColumn members

        public override bool Editable
        {
            get { return true; }
        }

        #endregion
    }
}
