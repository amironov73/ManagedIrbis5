// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global

/* FreeBand.cs -- свободная полоса отчета
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace ManagedIrbis.Reports
{
    /// <summary>
    /// Свободная полоса отчета. Ее рендеринг определяется
    /// внешним источником данных.
    /// </summary>
    public class FreeBand
        : ReportBand
    {
        #region Properties

        /// <summary>
        ///
        /// </summary>
        public object? DataSource { get; set; }

        #endregion

        #region ReportBand members

        /// <inheritdoc cref="ReportBand.Render"/>
        public override void Render (ReportContext context) =>
            throw new NotImplementedException();

        #endregion

    } // class FreeBand

} // namespace ManagedIrbis.Reports
