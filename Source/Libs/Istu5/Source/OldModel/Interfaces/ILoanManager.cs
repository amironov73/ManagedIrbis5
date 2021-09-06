// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UseNameofExpression

/* ILoanManager.cs -- интерфейс менеджера книговыдачи
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

using Istu.OldModel.Loans;

#endregion

#nullable enable

namespace Istu.OldModel.Interfaces
{
    /// <summary>
    /// Интерфейс менеджера книговыдачи.
    /// </summary>
    public interface ILoanManager
        : IDisposable
    {
        /// <summary>
        ///
        /// </summary>
        Loan[] GetLoans(string ticket);

        /// <summary>
        ///
        /// </summary>
        Loan GetSciLoan(string inventory);

        /// <summary>
        ///
        /// </summary>
        Loan GetUchLoan(string barcode);

        /// <summary>
        ///
        /// </summary>
        Loan GetUchLoanByRfid(string rfid);

        /// <summary>
        ///
        /// </summary>
        void GiveBooks(Attendance attendance, IEnumerable<Loan> loans);

        /// <summary>
        ///
        /// </summary>
        void ReturnBooks(Attendance attendance, IEnumerable<Loan> loans);

        /// <summary>
        ///
        /// </summary>
        void WriteOffBooks(IEnumerable<Loan> loans);

        /// <summary>
        ///
        /// </summary>
        void Update(Loan loan);

        /// <summary>
        ///
        /// </summary>
        DateTime GetLongestLoan(string abonement, Reader reader, DateTime proposed);

        /// <summary>
        ///
        /// </summary>
        int GetMaximumLoans(string abonement, Reader reader, int proposed);

        /// <summary>
        ///
        /// </summary>
        int[] GetLoanedInventories(int from, int to);

        /// <summary>
        ///
        /// </summary>
        void GiveToHands(Attendance attendance, IEnumerable<Loan> loans);

        /// <summary>
        ///
        /// </summary>
        void ReturnFromHands(Attendance attendance, IEnumerable<Loan> loans);

        // /// <summary>
        // ///
        // /// </summary>
        // void DeterminePilotCopy(Loan loan, IRemoteCatalog rc, int mfn, string inventory);

        /// <summary>
        ///
        /// </summary>
        void SetAlert(Loan loan, string text);

        // /// <summary>
        // ///
        // /// </summary>
        // /// <param name="bookInfo"></param>
        // /// <returns></returns>
        // bool AdjustBookInfo(BookInfo bookInfo);

        // /// <summary>
        // ///
        // /// </summary>
        // /// <param name="cardNumber"></param>
        // /// <returns></returns>
        // LoanInfo GetLoanInfo(string cardNumber);

        /// <summary>
        /// Lists the loans for given card number.
        /// </summary>
        Loan[] ListUchLoans(string cardNumber);

        /// <summary>
        ///
        /// </summary>
        void WriteOffByCard(string cardNumber);

        // /// <summary>
        // /// Получаем список просроченных экземпляров
        // /// </summary>
        // /// <param name="expirationDate">The expiration date.</param>
        // /// <returns>Не обязательно единственные экземпляры.</returns>
        // List<SingleInfo> GetExpiredSingles(DateTime expirationDate);

        /// <summary>
        /// Sets the seen.
        /// </summary>
        void SetSeen
            (
                IEnumerable<Loan> loans,
                DateTime when,
                int operatorID
            );

    } // interface ILoanManager

} // namespace Istu.OldModel.Interfaces
