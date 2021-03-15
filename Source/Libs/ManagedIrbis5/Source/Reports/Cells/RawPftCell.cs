﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* RawPftCell.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using AM.Logging;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Client;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Reports
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class RawPftCell
        : PftCell
    {
        #region PftCell members

        /// <inheritdoc cref="PftCell.Render" />
        public override void Render
        (
            ReportContext context
        )
        {
            Code.NotNull(context, "context");

            Log.Trace("RawPftCell::Render");

            string text = Text;

            if (string.IsNullOrEmpty(text))
            {
                // TODO: Skip or not on empty format?

                return;
            }

            ReportDriver driver = context.Driver;
            string formatted = Compute(context);

            driver.BeginCell(context, this);
            context.Output.Write(formatted);
            driver.EndCell(context, this);
        }

        #endregion
    }
}
