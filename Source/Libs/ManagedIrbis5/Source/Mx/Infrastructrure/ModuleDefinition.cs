// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* ModuleDefinition.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text.Json.Serialization;

#endregion

#nullable enable

namespace ManagedIrbis.Mx.Infrastructrure
{
    /// <summary>
    ///
    /// </summary>
    public sealed class ModuleDefinition
    {
        #region Properties

        /// <summary>
        /// Name.
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Description.
        /// </summary>
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        /// <summary>
        /// Class name.
        /// </summary>
        [JsonPropertyName("class")]
        public string? ClassName { get; set; }

        /// <summary>
        /// Assembly path.
        /// </summary>
        [JsonPropertyName("assembly")]
        public string? AssemblyPath { get; set; }

        #endregion
    }
}
