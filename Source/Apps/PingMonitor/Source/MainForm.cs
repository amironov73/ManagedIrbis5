// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable CoVariantArrayConversion
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* MainForm.cs -- главная форма приложения
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Windows.Forms;

using AM.Windows.Forms;

using ManagedIrbis;
using ManagedIrbis.Statistics;

#endregion

#nullable enable

namespace PingMonitor
{
    /// <summary>
    /// Главная форма приложения.
    /// </summary>
    public partial class MainForm
        : Form
    {
        #region Construction

        public MainForm()
        {
            InitializeComponent();
        }

        #endregion

        #region Private members

        private ISyncConnection? _connection;

        private IrbisPing? _pinger;

        public ISyncConnection GetConnection()
        {
            var result = (ISyncConnection) ConnectionUtility.GetConnectionFromConfig();
            result.Connect();

            return result;
        }

        #endregion

        private void MainForm_Load
            (
                object sender,
                EventArgs e
            )
        {
            this.ShowVersionInfoInTitle();

            _connection = GetConnection();
            _connection.Connect();

            this.Text += ": " + _connection.Host;

            _pinger = new IrbisPing(_connection)
            {
                Active = true
            };
            _pinger.StatisticsUpdated += _pinger_StatisticsUpdated;
            _plotter.Statistics = _pinger.Statistics;
        }

        private void _pinger_StatisticsUpdated
            (
                object? sender,
                EventArgs e
            )
        {
            var statistics = _pinger!.Statistics;
            if (statistics.Data.Count > 1500)
            {
                while (statistics.Data.Count > 1000)
                {
                    statistics.Data.Dequeue();
                }
            }

            _plotter.Invalidate();
        }

        private void MainForm_FormClosed
            (
                object sender,
                FormClosedEventArgs e
            )
        {
            _pinger!.Active = false;
            _pinger.Dispose();
            _connection!.Dispose();
        }
    }
}
