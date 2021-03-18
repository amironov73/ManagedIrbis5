// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* NewPageBand.cs -- полоса отчета, начинающая новую страницу
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.Reports
{
    /// <summary>
    /// Полоса отчета, начинающая новую страницу.
    /// </summary>
    public sealed class NewPageBand
        : ReportBand
    {
        #region ReportBand members

        /// <inheritdoc cref="ReportBand.Render" />
        public override void Render
            (
                ReportContext context
            )
        {
            var driver = context.Driver;
            driver.NewPage(context, this);

            // базовый метод Render не вызывается!
        } // method Render

        #endregion

    } // class NewPageBand

} // namespace ManagedIrbis.Reports
