// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global

/* GblExecutive.cs -- executes GBL statements locally
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using AM.Collections;
using AM.IO;
using AM.Runtime;

#endregion

#nullable enable

namespace ManagedIrbis.Gbl
{
    /// <summary>
    /// Executes GBL statements locally.
    /// </summary>
    public sealed class GblExecutive
    {
        #region Properties

        /// <summary>
        /// Connection.
        /// </summary>
        public ISyncIrbisProvider Connection { get; private set; }

        /// <summary>
        /// Record.
        /// </summary>
        public Record Record { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public GblExecutive
            (
                ISyncIrbisProvider connection,
                Record record
            )
        {
            Connection = connection;
            Record = record;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Get the field from the specification.
        /// </summary>
        public Field? GetField
            (
                string? fieldSpecification
            )
        {
            return null;
        }

        /// <summary>
        /// Whether the subfield specification.
        /// </summary>
        public bool IsSubField
            (
                string specification
            )
        {
            return specification.Contains("^");
        }

        #endregion
    }
}
