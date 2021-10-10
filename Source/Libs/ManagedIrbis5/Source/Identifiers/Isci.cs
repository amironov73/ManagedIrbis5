// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* Isci.cs -- ISCI
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.Identifiers
{
    //
    // https://en.wikipedia.org/wiki/Industry_Standard_Coding_Identification
    //
    // Industry Standard Coding Identification (ISCI, also known
    // as Industry Standard Commercial Identification) was a standard
    // created to identify commercials that aired on TV in the United States,
    // for ad agencies and advertisers from 1970.
    //

    /// <summary>
    /// Industry Standard Coding Identification.
    /// </summary>
    public sealed class Isci
    {
        #region Properties

        /// <summary>
        /// Identifier.
        /// </summary>
        public string? Identifier { get; set; }

        #endregion

    } // class Isci

} // namespace ManagedIrbis.Identifiers
