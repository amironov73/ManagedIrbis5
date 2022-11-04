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

using Istu.NewModel.Loans;

#endregion

#nullable enable

namespace Istu.NewModel.Interfaces;

/// <summary>
/// Интерфейс менеджера книговыдачи.
/// </summary>
public interface ILoanManager
    : IDisposable
{
    /// <summary>
    /// Получение массива документов, числящихся за данным читателем.
    /// </summary>
    Loan[] GetLoans
        (
            string ticket
        );

    /// <summary>
    /// Получение информации о выдаче книги научного фонда
    /// по указанному инвентарному номеру.
    /// </summary>
    Loan GetSciLoan
        (
            string inventory
        );

    /// <summary>
    /// Получение информации о выдаче книги учебного фонда
    /// по указанному штрих-коду.
    /// </summary>
    Loan GetUchLoan
        (
            string barcode
        );

    /// <summary>
    /// Получение информации о выдаче книги учебного фонда
    /// по указанному штрих-коду.
    /// </summary>
    Loan GetUchLoanByRfid
        (
            string rfid
        );

    /// <summary>
    /// Выдача указанных книг.
    /// </summary>
    void GiveBooks
        (
            Attendance attendance,
            IEnumerable<Loan> loans
        );

    /// <summary>
    /// Возврат указанных книг.
    /// </summary>
    void ReturnBooks
        (
            Attendance attendance,
            IEnumerable<Loan> loans
        );

    /// <summary>
    /// Списание указанных книг.
    /// </summary>
    void WriteOffBooks
        (
            IEnumerable<Loan> loans
        );

    /// <summary>
    /// Обновление информации о выдаче.
    /// </summary>
    void Update
        (
            Loan loan
        );

    /// <summary>
    /// Получение максимального срока выдачи для указанных
    /// читателя и пункта обслуживания.
    /// </summary>
    DateTime GetLongestLoan
        (
            string abonement,
            Reader reader,
            DateTime proposed
        );

    /// <summary>
    /// Получение ограничения по количеству одновременных выдач
    /// для указанных читателя и пункта обслуживания.
    /// </summary>
    int GetMaximumLoans
        (
            string abonement,
            Reader reader,
            int proposed
        );

    /// <summary>
    /// Получение массива инвентарных номеров книг научного фонда,
    /// числящихся выданными.
    /// </summary>
    int[] GetLoanedInventories
        (
            int from,
            int to
        );

    /// <summary>
    /// Выдача указанных документов из подсобного фонда
    /// читального зала на руки читателю.
    /// </summary>
    void GiveToHands
        (
            Attendance attendance,
            IEnumerable<Loan> loans
        );

    /// <summary>
    /// Возврат указанных документов в подсобый фонд читального зала.
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
    /// Задание текста сообщения для указанного выданного документа.
    /// </summary>
    void SetAlert
        (
            Loan loan,
            string? text
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
    /// Получение всех книг учебного фонда, приписанных
    /// к указанному номеру учетной карточки.
    /// </summary>
    Loan[] ListUchLoans
        (
            string cardNumber
        );

    /// <summary>
    /// Списание всех книг учебного фонда, приписанных
    /// к указанному номеру учетной карточки.
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
    /// Установка признака инвентаризации для указанных документов.
    /// </summary>
    void SetSeen
        (
            IEnumerable<Loan> loans,
            DateTime when,
            int operatorId
        );
}
