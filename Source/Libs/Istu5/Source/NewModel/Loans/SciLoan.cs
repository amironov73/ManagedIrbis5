// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UseNameofExpression

/* SciLoan.cs -- книга научного фонда
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM;

using LinqToDB;

#endregion

#nullable enable

namespace Istu.NewModel.Loans;

/// <summary>
/// Книга научного фонда.
/// </summary>
public sealed class SciLoan
    : Loan
{
    #region Public methods

    /// <summary>
    /// Получение выдачи из DTO.
    /// </summary>
    public void FromPodsob
        (
            Podsob podsob
        )
    {
        Sure.NotNull (podsob);

        // TODO implement
    }

    /// <summary>
    /// Перекладывание данных в DTO.
    /// </summary>
    public void ToPodsob
        (
            Podsob podsob
        )
    {
        Sure.NotNull (podsob);

        // TODO implement
    }

    #endregion

    #region Loan members

    /// <inheritdoc cref="Loan.Give"/>
    public override void Give
        (
            Storehouse storehouse,
            Attendance? attendance
        )
    {
        Sure.NotNull (storehouse);

        // if (!ConfigurationUtility.GetBoolean("allow-give-scibook", true))
        // {
        //     throw new ApplicationException("Нельзя выдавать книги научного фонда");
        // }

        if (!IsFree)
        {
            throw new InvalidOperationException();
        }

        var podsob = new Podsob();
        ToPodsob (podsob);

        using var kladovka = storehouse.GetKladovka();
        kladovka.Insert (podsob);
        RegisterAttendance (storehouse, attendance);
    }

    /// <inheritdoc cref="Loan.Return"/>
    public override void Return
        (
            Storehouse storehouse,
            Attendance? attendance
        )
    {
        Sure.NotNull (storehouse);

        if (IsFree)
        {
            throw new InvalidOperationException();
        }
    }

    /// <inheritdoc cref="Loan.CanGive"/>
    public override bool CanGive
        (
            Storehouse storehouse,
            Attendance? attendance
        )
    {
        Sure.NotNull (storehouse);

        return IsFree;
    }

    #endregion
}
