// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SimpleTextCell.cs -- простая текстовая ячейка отчета
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.Reports
{
    /// <summary>
    /// Простая текстовая ячейка отчета.
    /// </summary>
    public sealed class SimpleTextCell
        : TextCell
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SimpleTextCell()
        {
        } // constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public SimpleTextCell
            (
                string text
            )
            : base(text)
        {
        } // constructor

        #endregion

        #region ReportCell members

        /// <inheritdoc cref="TextCell.Compute"/>
        public override string? Compute
            (
                ReportContext context
            )
        {
            OnBeforeCompute(context);

            var result = Text;

            OnAfterCompute(context);

            return result;
        } // method Compute

        /// <inheritdoc cref="TextCell.Render" />
        public override void Render
            (
                ReportContext context
            )
        {
            var text = Compute(context);
            if (!string.IsNullOrEmpty(text))
            {
                context.Output.Write(text);
            }

        } // method Render

        #endregion

    } // class SimpleTextCell

} // namespace ManagedIrbis.Reports
