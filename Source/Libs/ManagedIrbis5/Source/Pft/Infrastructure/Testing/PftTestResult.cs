// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftTestResult.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Text.Json.Serialization;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Testing
{
    /// <summary>
    ///
    /// </summary>
    public sealed class PftTestResult
    {
        #region Properties

        /// <summary>
        /// Duration.
        /// </summary>
        [JsonPropertyName("duration")]
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// Test failed?
        /// </summary>
        [JsonPropertyName("failed")]
        public bool Failed { get; set; }

        /// <summary>
        /// Finish time.
        /// </summary>
        [JsonPropertyName("finish")]
        public DateTime FinishTime { get; set; }

        /// <summary>
        /// Name of the test.
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Description.
        /// </summary>
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        /// <summary>
        /// Start time.
        /// </summary>
        [JsonPropertyName("start")]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Input.
        /// </summary>
        [JsonPropertyName("input")]
        public string? Input { get; set; }

        /// <summary>
        /// Tokens.
        /// </summary>
        [JsonPropertyName("tokens")]
        public string? Tokens { get; set; }

        /// <summary>
        /// Program AST dump.
        /// </summary>
        [JsonPropertyName("ast")]
        public string? Ast { get; set; }

        /// <summary>
        /// Output text.
        /// </summary>
        [JsonPropertyName("expected")]
        public string? Expected { get; set; }

        /// <summary>
        /// Output text.
        /// </summary>
        [JsonPropertyName("output")]
        public string? Output { get; set; }

        /// <summary>
        /// Exception text (if any).
        /// </summary>
        [JsonPropertyName("exception")]
        public string? Exception { get; set; }

        #endregion

    } // class PftTestResult

} // namespace ManagedIrbis.Pft.Infrastructure.Testing
