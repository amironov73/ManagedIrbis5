﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* TableBand.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Reports
{
    /// <summary>
    /// Table band.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class TableBand
        : CompositeBand
    {
        #region Properties

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region ReportBand members

        /// <inheritdoc cref="CompositeBand.Render" />
        public override void Render
            (
                ReportContext context
            )
        {
            Code.NotNull(context, "context");

            OnBeforeRendering(context);

            ReportDriver driver = context.Driver;
            IrbisReport report = Report
                .ThrowIfNull("Report not set");

            driver.BeginTable(context, report);

            ReportBand header = Header;
            if (!ReferenceEquals(header, null))
            {
                header.RenderOnce(context);
            }

            int count = context.Records.Count;
            for (int index = 0; index < count; index++)
            {
                context.Index = index;
                context.CurrentRecord = context.Records[index];

                Body.Render(context);
            }

            ReportBand footer = Footer;
            if (!ReferenceEquals(footer, null))
            {
                footer.RenderOnce(context);
            }

            driver.EndTable(context, report);

            OnAfterRendering(context);
        }

        #endregion
    }
}
