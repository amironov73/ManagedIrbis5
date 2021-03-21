// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* BiblioItem.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;
using AM.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Biblio
{
    /// <summary>
    ///
    /// </summary>
    public sealed class BiblioItem
        : IVerifiable
    {
        #region Properties

        /// <summary>
        /// Chapter the item belongs to.
        /// </summary>
        public BiblioChapter? Chapter { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Record.
        /// </summary>
        public Record? Record { get; set; }

        /// <summary>
        /// Bibliographical description.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string? Order { get; set; }

        /// <summary>
        /// Terms.
        /// </summary>
        public NonNullCollection<BiblioTerm> Terms { get; private set; }

        /// <summary>
        /// Arbitrary user data.
        /// </summary>
        [JsonIgnore]
        public object? UserData { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public BiblioItem()
        {
            Terms = new NonNullCollection<BiblioTerm>();
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<BiblioItem> verifier
                = new Verifier<BiblioItem>(this, throwOnError);

            // TODO do something

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            var result = new StringBuilder();
            result.Append(Order);
            result.AppendLine();
            result.Append(Description);

            return result.ToString();
        }

        #endregion
    }
}
