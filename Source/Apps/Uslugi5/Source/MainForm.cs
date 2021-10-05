// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UseNameofExpression

/* MainForm.cs -- главная форма приложения
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using AM.Windows.Forms;

using LinqToDB;
using LinqToDB.Data;

using Istu.OldModel;
using Istu.UI;

using CM=System.Configuration.ConfigurationManager;

#endregion

#nullable enable

namespace Uslugi5
{
    /// <summary>
    /// Главная форма приложения.
    /// </summary>
    public partial class MainForm
        : Form
    {
        private readonly DataConnection db;
        private Reader? reader;
        private readonly Usluga[] uslugi;

        public MainForm()
        {
            InitializeComponent();

            db = IstuUtility.GetMsSqlConnection("kladovka");
            // TODO добавить Unit
            uslugi = UslugaDescription.ReadFile("uslugi.txt")
                .Select(description => new Usluga()
                {
                    Title = description.Title,
                    Price = description.Price
                })
                .ToArray();
            _bindingSource.DataSource = uslugi;
            _bindingSource.MoveFirst();

            var addEnable = CM.AppSettings["add"];
            if (addEnable != "1")
            {
                _addButton.Enabled = false;
                _reportButton.Enabled = false;
            }
        } // constructor

        private void _readButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            string rfid = _rfidBox.Text.Trim();
            if (string.IsNullOrEmpty(rfid))
            {
                MessageBox.Show("Не задан RFID!");
                return;
            }

            string name = rfid + "%";

            var candidates = db.Query<Reader>
                (
                    "select * from [readers] where [name] like @name order by [name]",
                    new DataParameter("@name", name)
                )
                .ToArray();

            if (candidates.Length == 1)
            {
                reader = candidates[0];
            }
            else if (candidates.Length > 1)
            {
                reader = ChooseReaderForm.ChooseReader(this, candidates);
            }
            else
            {
                reader = db.Query<Reader>
                    (
                        "select * from [readers] where [rfid]=@rfid "
                        + "or [barcode]=@rfid or [ticket]=@rfid",
                        new DataParameter("@rfid", rfid)
                    )
                    .FirstOrDefault();
            }

            if (reader == null)
            {
                MessageBox.Show("Нет читателя с таким RFID");
                return;
            }

            _infoBox.Text = reader.ToString();

        } // method ReadButton_Click

        private void _payUsluga
            (
                Usluga usluga
            )
        {
            if (reader is null)
            {
                MessageBox.Show("Не задан читатель!");
                return;
            }

            var summa = usluga.Summa;
            db.BeginTransaction();
            var count = db.Execute
                (
                    @"update [readers] set [debet]=[debet] - @sum where [ticket]=@ticket",
                    new DataParameter ("@sum", summa),
                    new DataParameter ("@ticket", reader.Ticket)
                );

            if (count != 1)
            {
                MessageBox.Show("Что-то пошло не так!");
                db.RollbackTransaction();
                throw new Exception();
            }

            usluga.Moment = DateTime.Now;
            usluga.Ticket = reader.Ticket;
            usluga.Summa = -usluga.Summa;
            db.Insert (usluga);
            db.CommitTransaction();

        } // method _payUsluga

        private void _clearTable()
        {
            foreach (var usluga in uslugi)
            {
                usluga.Amount = 0;
            }

            _bindingSource.ResetBindings (false);

        } // method _clearTable

        private void PrepareUsluga
            (
                Reader theReader,
                Usluga usluga
            )
        {
            if (usluga.Title == "Подписал обходной")
            {
                usluga.Amount = 1;
                usluga.Price = theReader.Debet;
                usluga.Summa = usluga.Price;
            }
            else
            {
                usluga.Summa = usluga.Amount * usluga.Price;
            }
        } // method PrepareUsluga


        private void _payButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            if (reader == null)
            {
                MessageBox.Show("Не задан читатель!");
                return;
            }

            var selected = uslugi.Where(u => u.Amount != 0).ToArray();
            if (selected.Length == 0)
            {
                MessageBox.Show("Не выбраны услуги!");
                return;
            }

            foreach (var usluga in selected)
            {
                PrepareUsluga(reader, usluga);
            }

            var sum = selected.Sum(u => u.Summa);
            if (sum > reader.Debet)
            {
                MessageBox.Show("Сумма превышает баланс читателя");
                return;
            }

            try
            {
                db.BeginTransaction();
                foreach (var usluga in selected)
                {
                    _payUsluga(usluga);
                }
                db.CommitTransaction();

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
                db.RollbackTransaction();
                _clearTable();
                return;
            }

            _clearTable();

            reader = null;
            _rfidBox.Clear();
            _infoBox.Clear();
            _rfidBox.Focus();

            MessageBox.Show("Оплата прошла успешно");

        } // method _payButton_Click

        private void MainForm_FormClosed
            (
                object sender,
                FormClosedEventArgs e
            )
        {
            db.Dispose();

        } // method MainForm_FormClosed

        private void _addButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            var addEnable = CM.AppSettings["add"];
            if (addEnable != "1")
            {
                _addButton.Enabled = false;
            }

            if (!decimal.TryParse(_moneyBox.Text, out var amount))
            {
                MessageBox.Show("Неверно задана сумма!");
                return;
            }

            _moneyBox.Clear();

            if (amount <= 0)
            {
                MessageBox.Show("Неверно задана сумма!");
                return;
            }

            if (reader == null)
            {
                MessageBox.Show("Не задан читатель!");
                return;
            }

            db.BeginTransaction();
            try
            {
                var count = db.Execute
                    (
                        @"update [readers]
                        set [debet]=isnull([debet],0) + @amount
                        where [ticket]=@ticket",
                        new DataParameter("@amount", amount),
                        new DataParameter("@ticket", reader.Ticket)
                    );

                if (count == 1)
                {
                    var usluga = new Usluga()
                    {
                        Moment = DateTime.Now,
                        Ticket = reader.Ticket,
                        Title = "Пополнение",
                        Price = amount,
                        Amount = 1,
                        Summa = amount,
                        Operator = Environment.MachineName
                    };
                    db.Insert(usluga);
                    db.CommitTransaction();
                    MessageBox.Show("Баланс успешно пополнен!");
                }
                else
                {
                    MessageBox.Show("Что-то пошло не так!");
                }
            }
            catch
            {
                db.RollbackTransaction();
            }

            _rfidBox.Text = reader.Ticket;
            _readButton_Click(this, e);

        } // method _addButton_Click

        private void _balanceButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            if (reader == null)
            {
                MessageBox.Show("Не задан читатель!");
                return;
            }

            var table = db.GetTable<Usluga>();
            var list = table.Where (u => u.Ticket == reader.Ticket)
                .OrderBy (u => u.Moment)
                .ToArray();
            var debet = db.Execute<decimal>
                (
                    "select [debet] from [readers] where [ticket]=@ticket",
                    new DataParameter("@ticket", reader.Ticket)
                );

            var builder = new StringBuilder();
            foreach (var u in list)
            {
                builder.AppendFormat($"{u.Moment:d} \t{u.Title,-40} \t{u.Amount} \t{u.Summa}");
                builder.AppendLine();
            }

            builder.AppendLine();
            builder.AppendFormat($"Остаток на счету: {debet}");
            PlainTextForm.ShowDialog (this, builder.ToString());

        } // method _balanceButton_Click

        private void _reportButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            var table = db.GetTable<Usluga>();
            var list = table.Where (u => u.Moment >= DateTime.Today)
                .OrderBy (u => u.Moment)
                .ToArray();
            var builder = new StringBuilder();
            foreach (var u in list)
            {
                builder.AppendFormat($"{u.Moment:d} \t{u.Ticket, -40} \t{u.Title,-40} \t {u.Price, -8} \t{u.Amount} \t{u.Summa}");
                builder.AppendLine();
            }

            var summa = list
                .Where(_ => _.Title == "Пополнение")
                .Sum(_ => _.Summa);
            builder.AppendLine();
            builder.AppendFormat("Сумма прихода: {0}", summa);

            builder.AppendLine();
            PlainTextForm.ShowDialog(this, builder.ToString(), maximized: true);

        } // method _reportButton_Click

    } // class MainForm

} // namespace Uslugi5
