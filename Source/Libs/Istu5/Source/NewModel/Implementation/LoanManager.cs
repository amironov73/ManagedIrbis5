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

using Istu.NewModel.Interfaces;
using Istu.NewModel.Loans;

using LinqToDB;
using LinqToDB.Data;

#endregion

#nullable enable

namespace Istu.NewModel.Implementation;

/// <summary>
/// Менеджер книговыдачи.
/// </summary>
public sealed class LoanManager
    : ILoanManager
{
    #region Properties

    /// <summary>
    /// Кладовка.
    /// </summary>
    public Storehouse Storehouse { get; }

    /// <summary>
    /// Таблица <c>подсобного фонда</c>.
    /// </summary>
    public ITable<Podsob> Podsobs => _GetPodsob().GetPodsob();

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public LoanManager
        (
            Storehouse storehouse
        )
    {
        Sure.NotNull (storehouse);

        Storehouse = storehouse;
    }

    #endregion

    #region Private members

    private DataConnection? _dataConnection;

    private DataConnection _GetPodsob() => _dataConnection ??= Storehouse.GetKladovka();

    #endregion

    #region ILoanManager members

    /// <inheritdoc cref="ILoanManager.GetLoans"/>
    public Loan[] GetLoans
        (
            string ticket
        )
    {
        Sure.NotNullNorEmpty (ticket);

        var _ = Podsobs.Where (p => p.Ticket == ticket).ToArray();

        throw new NotImplementedException();
    }

    /// <inheritdoc cref="ILoanManager.GetSciLoan"/>
    public Loan GetSciLoan
        (
            string inventory
        )
    {
        Sure.NotNullNorEmpty (inventory);

        throw new NotImplementedException();
    }

    /// <inheritdoc cref="ILoanManager.GetUchLoan"/>
    public Loan GetUchLoan
        (
            string barcode
        )
    {
        Sure.NotNullNorEmpty (barcode);

        throw new NotImplementedException();
    }

    /// <inheritdoc cref="ILoanManager.GetUchLoanByRfid"/>
    public Loan GetUchLoanByRfid
        (
            string rfid
        )
    {
        Sure.NotNullNorEmpty (rfid);

        throw new NotImplementedException();
    }

    /// <inheritdoc cref="ILoanManager.GiveBooks"/>
    public void GiveBooks
        (
            Attendance attendance,
            IEnumerable<Loan> loans
        )
    {
        Sure.NotNull (attendance);
        Sure.NotNull ((object?) loans);

        throw new NotImplementedException();
    }

    /// <inheritdoc cref="ILoanManager.ReturnBooks"/>
    public void ReturnBooks
        (
            Attendance attendance,
            IEnumerable<Loan> loans
        )
    {
        Sure.NotNull (attendance);
        Sure.NotNull ((object?) loans);

        throw new NotImplementedException();
    }

    /// <inheritdoc cref="ILoanManager.WriteOffBooks"/>
    public void WriteOffBooks
        (
            IEnumerable<Loan> loans
        )
    {
        Sure.NotNull ((object?) loans);

        throw new NotImplementedException();
    }

    /// <inheritdoc cref="ILoanManager.Update"/>
    public void Update
        (
            Loan loan
        )
    {
        Sure.NotNull (loan);

        throw new NotImplementedException();
    }

    /// <inheritdoc cref="ILoanManager.GetLongestLoan"/>
    public DateTime GetLongestLoan
        (
            string abonement,
            Reader reader,
            DateTime proposed
        )
    {
        Sure.NotNullNorEmpty (abonement);
        Sure.NotNull (reader);

        throw new NotImplementedException();
    }

    /// <inheritdoc cref="ILoanManager.GetMaximumLoans"/>
    public int GetMaximumLoans
        (
            string abonement,
            Reader reader,
            int proposed
        )
    {
        Sure.NotNullNorEmpty (abonement);
        Sure.NotNull (reader);

        throw new NotImplementedException();
    }

    /// <inheritdoc cref="ILoanManager.GetLoanedInventories"/>
    public int[] GetLoanedInventories
        (
            int fromNumber,
            int toNumber
        )
    {
        Sure.Positive (fromNumber);
        Sure.Positive (toNumber);

        throw new NotImplementedException();
    }

    /// <inheritdoc cref="ILoanManager.GiveToHands"/>
    public void GiveToHands
        (
            Attendance attendance,
            IEnumerable<Loan> loans
        )
    {
        Sure.NotNull (attendance);
        Sure.NotNull ((object?) loans);

        throw new NotImplementedException();
    }

    /// <inheritdoc cref="ILoanManager.ReturnFromHands"/>
    public void ReturnFromHands
        (
            Attendance attendance,
            IEnumerable<Loan> loans
        )
    {
        Sure.NotNull (attendance);
        Sure.NotNull ((object?) loans);

        throw new NotImplementedException();
    }

    /// <inheritdoc cref="ILoanManager.SetAlert"/>
    public void SetAlert
        (
            Loan loan,
            string text
        )
    {
        Sure.NotNull (loan);

        throw new NotImplementedException();
    }

    /// <inheritdoc cref="ILoanManager.ListUchLoans"/>
    public Loan[] ListUchLoans
        (
            string cardNumber
        )
    {
        Sure.NotNullNorEmpty (cardNumber);

        throw new NotImplementedException();
    }

    /// <inheritdoc cref="ILoanManager.WriteOffByCard"/>
    public void WriteOffByCard
        (
            string cardNumber
        )
    {
        Sure.NotNullNorEmpty (cardNumber);

        throw new NotImplementedException();
    }

    /// <inheritdoc cref="ILoanManager.SetSeen"/>
    public void SetSeen
        (
            IEnumerable<Loan> loans,
            DateTime when,
            int operatorID
        )
    {
        Sure.NotNull ((object?) loans);

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
    }

    #endregion
}
