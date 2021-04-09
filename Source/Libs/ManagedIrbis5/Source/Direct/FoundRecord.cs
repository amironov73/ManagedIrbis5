// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* FoundRecord.cs -- found record info
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.Direct
{
    /// <summary>
    /// Found record info.
    /// </summary>
    public struct FoundRecord
    {
        #region Properties

        /// <summary>
        /// MFN.
        /// </summary>
        public int Mfn { get; set; }

        /// <summary>
        /// Offset.
        /// </summary>
        public long Position { get; set; }

        /// <summary>
        /// Length of the record.
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// Field count.
        /// </summary>
        public int FieldCount { get; set; }

        /// <summary>
        /// Version of the record.
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// Flags.
        /// </summary>
        public int Flags { get; set; }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() => $"[{Mfn}] v{Version}";

        #endregion
    }
}
