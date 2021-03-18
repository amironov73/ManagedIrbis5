// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global

/* ParagraphBand.cs -- полоса отчета, содержащая параграф текста
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.Reports
{
    /// <summary>
    /// Полоса отчета, содержащая параграф текста.
    /// </summary>
    public sealed class ParagraphBand
        : ReportBand
    {
        #region Properties

        /// <summary>
        /// Style name.
        /// </summary>
        public string? StyleSpecification { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ParagraphBand()
        {
        } // constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public ParagraphBand
            (
                string text
            )
        {

            Cells.Add(new SimpleTextCell(text));
        } // constructor

        #endregion

        #region ReportBand members

        /// <inheritdoc cref="ReportBand.Render" />
        public override void Render
            (
                ReportContext context
            )
        {
            var driver = context.Driver;
            driver.BeginParagraph(context, this);

            if (!string.IsNullOrEmpty(StyleSpecification))
            {
                driver.WriteServiceText(context, StyleSpecification);
            }

            foreach (var cell in Cells)
            {
                cell.Render(context);
            }

            driver.EndParagraph(context, this);
        } // method Render

        #endregion

    } // class ParagraphBand

} // namespace ManagedIrbis.Reports
