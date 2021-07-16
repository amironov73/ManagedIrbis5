// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* MystemAnalysis.cs -- анализ слова
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Linq;
using System.Text.Json.Serialization;

#endregion

#nullable enable

namespace AM.AOT.Stemming
{
    /// <summary>
    /// Анализ слова.
    /// </summary>
    public sealed class MystemAnalysis
    {
        #region Properties

        /// <summary>
        /// Основная форма слова.
        /// </summary>
        [JsonPropertyName("lex")]
        public string? Lexeme { get; set; }

        /// <summary>
        /// Относительный вес
        /// </summary>
        [JsonPropertyName("wt")]
        public double Weight { get; set; }

        /// <summary>
        /// Грамматический разбор слова.
        /// </summary>
        [JsonPropertyName("gr")]
        public string? Grammeme { get; set; }

        /// <summary>
        /// Часть речи.
        /// </summary>
        public string PartOfSpeech => Split[0].Trim('=');

        /// <summary>
        /// Обсценное?
        /// </summary>
        public bool IsObscene => Split.Contains("обсц");

        #endregion

        #region Private members

        private string[] Split => string.IsNullOrEmpty(Grammeme)
            ? new[] { String.Empty }
            : Grammeme.Split(',');

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString() => $"{Lexeme}, {PartOfSpeech}";

        #endregion

    } // class MyStemAnalysis

} // namespace AM.AOT.Stemming
