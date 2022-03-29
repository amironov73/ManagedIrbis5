// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo

/* MainForm.cs -- главная форма приложения
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using AM;

using DevExpress.Utils.Taskbar.Core;
using DevExpress.XtraEditors;

using ManagedIrbis.AppServices;
using ManagedIrbis.Readers;

using CM=System.Configuration.ConfigurationManager;

#endregion

#nullable enable

namespace BulletinMaker;

/// <summary>
/// Главная форма приложения
/// </summary>
public partial class MainForm
    : XtraForm
{
    #region Construction

    /// <summary>
    /// Конструктор
    /// </summary>
    public MainForm
        (
            IrbisApplication application
        )
    {
        Sure.NotNull (application);

        _application = application;
        InitializeComponent();
    }

    #endregion

    #region Private members

    private readonly IrbisApplication _application;

    #endregion
}
