// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global

/* DetailsBand.cs -- полоса с детальным представлением данных (повторяющаяся)
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.Reports
{
    /// <summary>
    /// Полоса с детальным представлением данных (повторяющася).
    /// </summary>
    public class DetailsBand
        : ReportBand
    {

        #region ReportBand members

        /// <inheritdoc cref="ReportBand.Render" />
        public override void Render
            (
                ReportContext context
            )
        {
            OnBeforeRendering(context);

            RenderAllRecords(context);

            OnAfterRendering(context);
        } // method Render

        #endregion

    } // class DetailsBand

} // namespace ManagedIrbis.Reports
