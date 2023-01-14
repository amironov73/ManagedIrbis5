// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Reporting.Data;

using System;

#endregion

#nullable enable

namespace AM.Reporting
{
    partial class ReportSettings
    {
        #region Internal Methods

        /// <summary>
        /// Does nothing
        /// </summary>
        /// <param name="report"></param>
        internal void OnFinishProgress (Report report)
        {
        }

        /// <summary>
        /// Does nothing
        /// </summary>
        /// <param name="report"></param>
        /// <param name="str"></param>
        internal void OnProgress (Report? report, string str)
        {
        }

        /// <summary>
        /// Does nothing
        /// </summary>
        /// <param name="report"></param>
        internal void OnProgress (Report? report, string str, int int1, int int2)
        {
        }

        /// <summary>
        /// Does nothing
        /// </summary>
        /// <param name="report"></param>
        internal void OnStartProgress (Report report)
        {
        }

        internal void OnDatabaseLogin (DataConnectionBase sender, DatabaseLoginEventArgs e)
        {
            if (DatabaseLogin != null)
            {
                DatabaseLogin (sender, e);
            }
        }

        #endregion Internal Methods
    }
}
