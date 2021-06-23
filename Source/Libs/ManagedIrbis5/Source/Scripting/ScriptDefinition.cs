// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo

/* ScriptDefinition.cs -- описание скрипта
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text.Json.Serialization;

#endregion

#nullable enable

namespace ManagedIrbis.Scripting
{
    /// <summary>
    /// Описание скрипта.
    /// </summary>
    public sealed class ScriptDefinition
    {
        #region Properties

        /// <summary>
        /// Директивы #using.
        /// </summary>
        [JsonPropertyName("usings")]
        public string[]? Usings { get; set; }

        /// <summary>
        /// Директивы #define.
        /// </summary>
        [JsonPropertyName("defines")]
        public string[]? Defines { get; set; }

        /// <summary>
        /// Исходные тексты.
        /// </summary>
        [JsonPropertyName("sources")]
        public string[]? Sources { get; set; }

        /// <summary>
        /// Ссылки на сборки.
        /// </summary>
        [JsonPropertyName("references")]
        public string[]? References { get; set; }

        /// <summary>
        /// Не надо ссылок по умолчанию.
        /// </summary>
        [JsonPropertyName("no-default-references")]
        public bool NoDefaultReferences { get; set; }

        #endregion

    } // class ScriptDefinition

} // namespace ManagedIrbis.Scripting
