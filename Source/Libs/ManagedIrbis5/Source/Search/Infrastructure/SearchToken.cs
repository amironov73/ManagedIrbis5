// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* SearchToken.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics;
using System.Text.Json.Serialization;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Infrastructure
{
    /// <summary>
    /// Token.
    /// </summary>
    [DebuggerDisplay("{Kind} {Text} {Position}")]
    internal sealed class SearchToken
    {
        #region Properties

        /// <summary>
        /// Token kind.
        /// </summary>
        [JsonPropertyName("kind")]
        public SearchTokenKind Kind { get; set; }

        /// <summary>
        /// Token position.
        /// </summary>
        [JsonPropertyName("position")]
        public int Position { get; set; }

        /// <summary>
        /// Token text.
        /// </summary>
        [JsonPropertyName("text")]
        public string? Text { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SearchToken
            (
                SearchTokenKind kind,
                int position,
                string? text
            )
        {
            Kind = kind;
            Position = position;
            Text = text;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() => Text.ToVisibleString();

        #endregion

    } // class SearchToken

} // namespace ManagedIrbis.Infrastructure
