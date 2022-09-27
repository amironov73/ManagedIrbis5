// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* TermGridTest.cs -- тест для грида, отображающего термины поискового словаря
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Windows.Forms;

using ManagedIrbis;
using ManagedIrbis.WinForms.Grid;

#endregion

#nullable enable

namespace SiberianTests;

/// <summary>
/// Тест для грида, отображающего термины поискового словаря.
/// </summary>
public sealed class TermGridTest
    : ISiberianTest
{
    #region ISiberianTest

    public void RunTest
        (
            IWin32Window? ownerWindow
        )
    {
        using var form = new DummyForm
        {
            Width = 800,
            Height = 600
        };

        var grid = new SiberianTermGrid
        {
            Dock = DockStyle.Fill
        };
        form.Controls.Add (grid);

        const string connectionString = "host=127.0.0.1;port=6666;user=librarian;password=secret;";
        using var connection = ConnectionFactory.Shared.CreateSyncConnection();
        connection.ParseConnectionString (connectionString);
        connection.Connect();

        var parameters = new TermParameters
        {
            Database = "IBIS",
            NumberOfTerms = 100,
            //ReverseOrder = false,
            StartTerm = "K=",
            //Format = null
        };

        var terms = connection.ReadTerms (parameters) ?? Array.Empty<Term>();
        terms = Term.TrimPrefix (terms, "K=");
        connection.Disconnect();

        grid.Load (terms);

        form.ShowDialog (ownerWindow);
    }

    #endregion
}
