// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* StandardDatabases.cs -- стандартные базы данных, включенные в поставку
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// SСтандартные базы данных, включенные в поставку ИРБИС64.
    /// </summary>
    public static class StandardDatabases
    {
        #region Constants

        /// <summary>
        /// Digital catalogue.
        /// </summary>
        public const string ElectronicCatalog = "IBIS";

        /// <summary>
        /// Picking.
        /// </summary>
        public const string Acquisition = "CMPL";

        /// <summary>
        /// Readers.
        /// </summary>
        public const string Readers = "RDR";

        /// <summary>
        /// Orders for books.
        /// </summary>
        public const string Requests = "RQST";

        #endregion
    }
}
