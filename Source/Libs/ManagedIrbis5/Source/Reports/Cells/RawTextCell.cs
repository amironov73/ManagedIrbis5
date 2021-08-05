// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* RawTextCell.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.Reports
{
    /// <summary>
    ///
    /// </summary>
    public sealed class RawTextCell
        : TextCell
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public RawTextCell()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public RawTextCell
            (
                string text
            )
            : base(text)
        {
        }

        #endregion

        #region ReportCell

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
            var driver = context.Driver;
            driver.BeginCell(context, this);
            if (!string.IsNullOrEmpty(text))
            {
                context.Output.Write(text);
            }

            driver.EndCell(context, this);
        } // method Render

        #endregion

    } // class RawTextCell

} // namespace ManagedIrbis.Reports
