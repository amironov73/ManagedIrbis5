// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global

/* FoundLine.cs -- line in list of found documents
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Line in list of found documents.
    /// </summary>
    public sealed class FoundLine
    {
        #region Properties

        /// <summary>
        /// Serial number.
        /// </summary>
        public int SerialNumber { get; set; }

        /// <summary>
        /// MFN.
        /// </summary>
        public int Mfn { get; set; }

        /// <summary>
        /// Icon.
        /// </summary>
        public object? Icon { get; set; }

        /// <summary>
        /// Selected by user.
        /// </summary>
        public bool Selected { get; set; }

        /// <summary>
        /// Description.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// For list sorting.
        /// </summary>
        public string? Sort { get; set; }

        /// <summary>
        /// Arbitrary user data.
        /// </summary>
        public object? UserData { get; set; }

        #endregion

    } // class FoundLine

} // namespace ManagedIrbis
