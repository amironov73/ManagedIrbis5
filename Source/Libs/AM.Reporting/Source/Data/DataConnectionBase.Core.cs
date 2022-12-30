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

using System.Collections.Generic;
using System.Data.Common;

#endregion

#nullable enable

namespace AM.Reporting.Data
{
    partial class DataConnectionBase
    {
        #region Private Methods

        /// <summary>
        /// Does nothing
        /// </summary>
        /// <param name="tableNames"></param>
        partial void FilterTables(List<string> tableNames);


        /// <summary>
        /// Does nothing
        /// </summary>
        private DbConnection GetDefaultConnection()
        {
            return null;
        }

        /// <summary>
        /// Does nothing
        /// </summary>
        /// <param name="connection"></param>
        /// <returns>false</returns>
        private bool ShouldNotDispose(DbConnection connection)
        {
            return false;
        }

        /// <summary>
        /// Does nothing
        /// </summary>
        partial void ShowLoginForm(string lastConnectionString);

        #endregion Private Methods
    }
}
