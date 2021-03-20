// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* MxRecord.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

#endregion

#nullable enable

namespace ManagedIrbis.Mx
{
    /// <summary>
    ///
    /// </summary>
    public sealed class MxRecord
    {
        #region Properties

        /// <summary>
        /// Sequential number.
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Database name.
        /// </summary>
        public string? Database { get; set; }

        /// <summary>
        /// MFN.
        /// </summary>
        public int Mfn { get; set; }

        /// <summary>
        /// Record index (field 903, if any).
        /// </summary>
        public string? Index { get; set; }

        /// <summary>
        /// Record itself.
        /// </summary>
        public Record? Record { get; set; }

        /// <summary>
        /// Bibliographic description.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// For sorting.
        /// </summary>
        public string? Order { get; set; }

        /// <summary>
        /// Arbitrary user data.
        /// </summary>
        public object? UserData { get; set; }

        #endregion
    }
}
