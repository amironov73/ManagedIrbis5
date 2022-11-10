// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ILoanManager.cs -- интерфейс менеджера книговыдачи
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

using Istu.OldModel.Loans;

#endregion

#nullable enable

namespace Istu.OldModel.Interfaces;

/// <summary>
/// Интерфейс менеджера книговыдачи.
/// </summary>
public interface ILoanManager
    : IDisposable
{
    /// <summary>
    /// Получение всех книг, числящихся за указанным читательским билетом.
    /// </summary>
    Loan[] GetLoans
        (
            string ticket
        );

    /// <summary>
    /// Получение книги научного фонда с указанным инвентарным номером.
    /// </summary>
    Loan GetSciLoan
        (
            string inventory
        );

    /// <summary>
    /// Получение книги учебного фонда с указанным штрих-кодом.
    /// </summary>
    Loan GetUchLoan
        (
            string barcode
        );

    /// <summary>
    /// Получение книги учебного фонда с указанной RFID-меткой.
    /// </summary>
    Loan GetUchLoanByRfid
        (
            string rfid
        );

    /// <summary>
    /// Выдача перечисленных книг.
    /// </summary>
    void GiveBooks
        (
            Attendance attendance,
            IEnumerable<Loan> loans
        );

    /// <summary>
    /// Возврат перечисленных книг.
    /// </summary>
    void ReturnBooks
        (
            Attendance attendance,
            IEnumerable<Loan> loans
        );

    /// <summary>
    /// Списание перечисленных книг.
    /// </summary>
    void WriteOffBooks
        (
            IEnumerable<Loan> loans
        );

    /// <summary>
    /// Обновление информации об указанной выдаче.
    /// </summary>
    void Update
        (
            Loan loan
        );

    /// <summary>
    /// Получение максимально возможного срока выдачи литературы.
    /// </summary>
    DateTime GetLongestLoan
        (
            string abonement,
            Reader reader,
            DateTime proposed
        );

    /// <summary>
    /// Получение максимально возможного количество выданных документов.
    /// </summary>
    int GetMaximumLoans
        (
            string abonement,
            Reader reader,
            int proposed
        );

    /// <summary>
    /// Получение выданных книг научного фонда с инвентарными номерами
    /// в указанном диапазоне.
    /// </summary>
    int[] GetLoanedInventories
        (
            int from,
            int to
        );

    /// <summary>
    /// Выдача "на руки" перечисленных изданий.
    /// </summary>
    void GiveToHands
        (
            Attendance attendance,
            IEnumerable<Loan> loans
        );

    /// <summary>
    /// Возврат "с рук" перечисленных изданий.
    /// </summary>
    void ReturnFromHands
        (
            Attendance attendance,
            IEnumerable<Loan> loans
        );

    // /// <summary>
    // ///
    // /// </summary>
    // void DeterminePilotCopy(Loan loan, IRemoteCatalog rc, int mfn, string inventory);

    /// <summary>
    /// Присвоение выданному документу сообщения,
    /// которое будет показано оператору.
    /// </summary>
    void SetAlert
        (
            Loan loan,
            string text
        );

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
    /// Получение книг учебного фонда, числящихся за данным
    /// номером учетной карточки.
    /// </summary>
    Loan[] ListUchLoans
        (
            string cardNumber
        );

    /// <summary>
    /// Списание книг учебного фонда по номеру учетной карточки.
    /// </summary>
    void WriteOffByCard
        (
            string cardNumber
        );

    // /// <summary>
    // /// Получаем список просроченных экземпляров
    // /// </summary>
    // /// <param name="expirationDate">The expiration date.</param>
    // /// <returns>Не обязательно единственные экземпляры.</returns>
    // List<SingleInfo> GetExpiredSingles(DateTime expirationDate);

    /// <summary>
    /// Маркировка перечисленных документов как прошедших инвентаризацию.
    /// </summary>
    void SetSeen
        (
            IEnumerable<Loan> loans,
            DateTime when,
            int operatorId
        );
}
