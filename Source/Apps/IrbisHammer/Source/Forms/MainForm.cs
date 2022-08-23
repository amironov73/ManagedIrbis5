// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* MainForm.cs -- главная форма приложения
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Windows.Forms;

#endregion

namespace IrbisHammer.Forms;

/// <summary>
/// Главная форма приложения.
/// </summary>
public partial class MainForm
    : Form
{
    #region Properties

    /// <summary>
    /// Контекст выполнения команд.
    /// </summary>
    public HammerContext Context { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="args">
    /// Аргументы командной строки.
    /// </param>
    public MainForm
        (
            string[] args
        )
    {
        InitializeComponent();

        Context = new HammerContext (args);
        foreach (var command in Context.KnownCommands)
        {
            _commandBox.Items.Add (command);
        }

        foreach (var database in Context.KnownDatabases)
        {
            _databaseBox.Items.Add (database);
        }
    }

    #endregion

    #region EventHandlers

    private void _exitMenuItem_Click
        (
            object sender,
            EventArgs eventArgs
        )
    {
        Close();
    }

    #endregion
}
