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
using System.ComponentModel;

using AM.Data;

using LinqToDB;

#endregion

#nullable enable

namespace Istu.NewModel.Loans
{
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
            // TODO implement

        } // method FromPodsob

        /// <summary>
        /// Перекладывание данных в DTO.
        /// </summary>
        public void ToPodsob
            (
                Podsob podsob
            )
        {
            // TODO implement

        } // method ToPodsob

        #endregion

        #region Loan members

        /// <inheritdoc cref="Loan.Give"/>
        public override void Give
            (
                Storehouse storehouse,
                Attendance? attendance
            )
        {
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
        } // method Give

        /// <inheritdoc cref="Loan.Return"/>
        public override void Return
            (
                Storehouse storehouse,
                Attendance? attendance
            )
        {
            if (IsFree)
            {
                throw new InvalidOperationException();
            }
        } // method Return

        /// <inheritdoc cref="Loan.CanGive"/>
        public override bool CanGive
            (
                Storehouse storehouse,
                Attendance? attendance
            )
        {
            return IsFree;
        } // method CanGive

        #endregion

    } // class SciLoan

} // namespace Istu.NewModel.Loans
