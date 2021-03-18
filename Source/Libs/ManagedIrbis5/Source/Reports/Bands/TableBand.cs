// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global

/* TableBand.cs -- полоса отчета, содержащая таблицу
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Reports
{
    /// <summary>
    /// Полоса отчета, содержащая таблицу.
    /// </summary>
    public class TableBand
        : CompositeBand
    {
        #region ReportBand members

        /// <inheritdoc cref="CompositeBand.Render" />
        public override void Render
            (
                ReportContext context
            )
        {
            OnBeforeRendering(context);

            var driver = context.Driver;
            var report = Report
                .ThrowIfNull("Report not set");

            driver.BeginTable(context, report);

            var header = Header;
            header?.RenderOnce(context);

            var count = context.Records.Count;
            for (var index = 0; index < count; index++)
            {
                context.Index = index;
                context.CurrentRecord = context.Records[index];

                Body.Render(context);
            }

            var footer = Footer;
            footer?.RenderOnce(context);

            driver.EndTable(context, report);

            OnAfterRendering(context);
        } // method Render

        #endregion

    } // class TableBand

} // namespace ManagedIrbis.Reports
