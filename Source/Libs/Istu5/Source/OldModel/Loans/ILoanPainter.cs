// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UseNameofExpression

/* ILoanPainter.cs -- интерфейс отрисовки выдачи.
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace Istu.OldModel.Loans
{
    /// <summary>
    /// Интерфейс отрисовки выдачи.
    /// </summary>
    public interface ILoanPainter
    {
        /// <summary>
        /// Отрисовка выдачи.
        /// </summary>
        void PaintLoan (Loan loan, object cell);

    } // interface ILoanPainter

} // namespace Istu.OldModel.Loans
