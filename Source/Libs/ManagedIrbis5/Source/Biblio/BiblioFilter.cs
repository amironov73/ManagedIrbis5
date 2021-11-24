// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* BiblioFilter.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text.Json.Serialization;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Biblio
{
    /// <summary>
    /// Удалить как ненужный ???
    /// </summary>
    public class BiblioFilter
        : IVerifiable
    {
        #region Properties

        /// <summary>
        /// Expression for record formatting.
        /// </summary>
        [JsonPropertyName ("format")]
        public string? FormatExpression { get; set; }

        /// <summary>
        /// Expression for record selection.
        /// </summary>
        [JsonPropertyName ("select")]
        public string? SelectExpression { get; set; }

        /// <summary>
        /// Expression for record sorting.
        /// </summary>
        [JsonPropertyName ("sort")]
        public string? SortExpression { get; set; }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier = new Verifier<BiblioFilter> (this, throwOnError);

            // TODO do something

            return verifier.Result;
        }

        #endregion
    }
}
