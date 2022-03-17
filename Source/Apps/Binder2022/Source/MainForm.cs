// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo

/* BindingMaster.cs -- умеет работать с журналами и подшивками
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

#endregion

#nullable enable

namespace Binder2022;

internal sealed partial class MainForm
    : DevExpress.XtraEditors.XtraForm
{
    #region Construction

    public MainForm
        (
            Program program
        )
    {
        _program = program;

        InitializeComponent();
    }

    #endregion

    #region Private members

    private readonly Program _program;

    #endregion
}
