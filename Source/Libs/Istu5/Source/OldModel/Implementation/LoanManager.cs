// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UseNameofExpression

/* LoanManager.cs -- менеджер  книговыдачи
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;

using AM;

using Istu.OldModel.Interfaces;
using Istu.OldModel.Loans;

using LinqToDB;
using LinqToDB.Data;

#endregion

#nullable enable

namespace Istu.OldModel.Implementation
{
    /// <summary>
    /// Менеджер книговыдачи.
    /// </summary>
    public sealed class LoanManager
        : ILoanManager
    {
        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public LoanManager
            (
                Storehouse storehouse
            )
        {
            _storehouse = storehouse;

        } // constructor

        #endregion

        #region Private members

        private readonly Storehouse _storehouse;
        // private DataConnection? _dataConnection;

        // private DataConnection _GetPodsob() => _dataConnection ??= _storehouse.GetKladovka();

        #endregion

        #region ILoanManager members

        /// <inheritdoc cref="ILoanManager.GetLoans"/>
        public Loan[] GetLoans(string ticket)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="ILoanManager.GetSciLoan"/>
        public Loan GetSciLoan(string inventory)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="ILoanManager.GetUchLoan"/>
        public Loan GetUchLoan(string barcode)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="ILoanManager.GetUchLoanByRfid"/>
        public Loan GetUchLoanByRfid(string rfid)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="ILoanManager.GiveBooks"/>
        public void GiveBooks(Attendance attendance, IEnumerable<Loan> loans)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="ILoanManager.ReturnBooks"/>
        public void ReturnBooks(Attendance attendance, IEnumerable<Loan> loans)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="ILoanManager.WriteOffBooks"/>
        public void WriteOffBooks(IEnumerable<Loan> loans)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="ILoanManager.Update"/>
        public void Update(Loan loan)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="ILoanManager.GetLongestLoan"/>
        public DateTime GetLongestLoan(string abonement, Reader reader, DateTime proposed)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="ILoanManager.GetMaximumLoans"/>
        public int GetMaximumLoans(string abonement, Reader reader, int proposed)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="ILoanManager.GetLoanedInventories"/>
        public int[] GetLoanedInventories(int @from, int to)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="ILoanManager.GiveToHands"/>
        public void GiveToHands(Attendance attendance, IEnumerable<Loan> loans)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="ILoanManager.ReturnFromHands"/>
        public void ReturnFromHands(Attendance attendance, IEnumerable<Loan> loans)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="ILoanManager.SetAlert"/>
        public void SetAlert(Loan loan, string text)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="ILoanManager.ListUchLoans"/>
        public Loan[] ListUchLoans(string cardNumber)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="ILoanManager.WriteOffByCard"/>
        public void WriteOffByCard(string cardNumber)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="ILoanManager.SetSeen"/>
        public void SetSeen(IEnumerable<Loan> loans, DateTime when, int operatorID)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            // if (_dataConnection is not null)
            // {
            //     _dataConnection.Dispose();
            //     _dataConnection = null;
            // }
        } // method Dispose

        #endregion

    } // class LoanManager

} // namespace Istu.OldModel.Implementation
