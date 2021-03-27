// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* ViafSuggestResponse.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text.Json.Serialization;

#endregion

#nullable enable

namespace RestfulIrbis.Viaf
{
    /// <summary>
    /// VIAF response
    /// </summary>
    public class ViafSuggestResponse
    {
        /// <summary>
        /// AsyncQuery.
        /// </summary>
        [JsonPropertyName("query")]
        public string? Query { get; set; }

        /// <summary>
        /// Results.
        /// </summary>
        [JsonPropertyName("result")]
        public ViafSuggestResult[]? SuggestResults { get; set; }
    }
}
