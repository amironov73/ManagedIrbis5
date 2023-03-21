// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* MainForm.cs -- главная форма программы
 * Ars Magna project, http://arsmagna.ru
 */

#pragma warning disable IDE1006 // Naming Styles

#region Using directives

using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using AM.Threading;
using AM.Windows.Forms;
using AM.Windows.Forms.MarkupExtensions;

using Istu.OldModel;

#endregion

#nullable enable

namespace FastTracker;

/// <summary>
/// Главная форма программы.
/// </summary>
internal sealed class MainForm
    : Form
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public MainForm()
    {
        var formSize = new Size (780, 500);
        MinimumSize = formSize;
        Size = formSize;

        Text = "Отслеживание заказов";
        var panel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill
        };
        panel.ColumnCount = 1;
        panel.ColumnStyles.Add (new ColumnStyle (SizeType.Percent, 100));
        panel.RowCount = 3;
        panel.RowStyles.Add (new RowStyle (SizeType.AutoSize));
        panel.RowStyles.Add (new RowStyle (SizeType.AutoSize));
        panel.RowStyles.Add (new RowStyle (SizeType.Percent, 100));

        var toolbar = new ToolStrip
        {
            Dock = DockStyle.Top,
            Items =
            {
                new ToolStripButton ("Обновить").OnClick (SomeReaction),
                new ToolStripButton ("Распечатать").OnClick (SomeReaction),
                new ToolStripButton ("Выдать").OnClick (SomeReaction),
                new ToolStripButton ("Отказать").OnClick (SomeReaction),
                new ToolStripButton ("Удалить").OnClick (SomeReaction),
            }
        };
        panel.Controls.Add (toolbar, 0, 0);

        _busyState = new BusyState();
        var busyStripe = new BusyStripe
        {
            // Visible = false,
            Dock = DockStyle.Top,
            Height = 6,
            ForeColor = Color.LimeGreen,
            // Text = "Обновление данных на сервере"
        };
        busyStripe.SubscribeTo (_busyState);
        panel.Controls.Add (busyStripe, 0, 1);

        var orders = FakeData.FakeOrderSource.GetOrders();
        _dataSource = new BindingListSource<Order>();
        _dataSource.AddRange (orders);
        _gridView = new HandyGrid
        {
            Dock = DockStyle.Fill,
            // DataSource = orders
        };
        _gridView.SetDataObject (orders);
        panel.Controls.Add (_gridView, 0, 2);

        Controls.Add (panel);
    }

    #endregion

    #region Private members

    private readonly BusyState _busyState;
    private readonly HandyGrid _gridView;
    private readonly BindingListSource<Order> _dataSource;

    private async void SomeReaction
        (
            object? sender,
            EventArgs eventArgs
        )
    {
        var controller = new BusyController (_busyState);
        await controller.RunAsync (() =>
        {
            Thread.Sleep (3000);
        });
        MessageBox.Show ("Выполнили");
    }

    #endregion
}
