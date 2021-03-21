// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* ViafSuggestResult.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text.Json.Serialization;

#endregion

#nullable enable

namespace RestfulIrbis.Viaf
{
    /// <summary>
    /// Single resulf from VIAF.
    /// </summary>
    public sealed class ViafSuggestResult
    {
        #region Properties

        /// <summary>
        ///
        /// </summary>
        [JsonPropertyName("term")]
        public string? Term { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonPropertyName("displayForm")]
        public string? DisplayForm { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonPropertyName("nametype")]
        public string? NameType { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonPropertyName("lc")]
        public string? Lc { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonPropertyName("dnb")]
        public string? Dnb { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonPropertyName("selibr")]
        public string? Selibr { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonPropertyName("bav")]
        public string? Bav { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonPropertyName("bnf")]
        public string? Bnf { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonPropertyName("iccu")]
        public string? Iccu { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonPropertyName("bne")]
        public string? Bne { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonPropertyName("nkc")]
        public string? Nkc { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonPropertyName("ptbnp")]
        public string? Ptbnp { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonPropertyName("swnl")]
        public string? Swnl { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonPropertyName("viafid")]
        public string? ViafId { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonPropertyName("score")]
        public string? Score { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonPropertyName("recordID")]
        public string? RecordId { get; set; }

        #endregion
    }
}
