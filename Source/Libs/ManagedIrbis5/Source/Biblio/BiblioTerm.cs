// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global

/* BiblioTerm.cs -- термин в библиографическом указателе
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Biblio
{
    /// <summary>
    /// Термин в библиографическом указателе.
    /// </summary>
    public sealed class BiblioTerm
        : IVerifiable
    {
        #region Properties

        /// <summary>
        ///
        /// </summary>
        public TermCollection? Dictionary { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Extended title.
        /// </summary>
        public string? Extended { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string? Order { get; set; }

        /// <summary>
        ///
        /// </summary>
        public BiblioItem? Item { get; set; }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify (bool throwOnError)
        {
            var verifier = new Verifier<BiblioTerm> (this, throwOnError);

            // TODO do something

            return verifier.Result;
        }

        #endregion
    }
}
