// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* StandardDatabases.cs -- стандартные базы данных, включенные в поставку
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Стандартные базы данных, включенные в поставку ИРБИС64.
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
