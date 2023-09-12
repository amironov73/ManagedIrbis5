// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* KeyedObject.cs -- объект, снабженный ключом
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using JetBrains.Annotations;

using Newtonsoft.Json;

#endregion

namespace RestfulIrbis.OpenLibrary;

/// <summary>
/// Объект, снабженный ключом.
/// </summary>
[PublicAPI]
public class KeyedObject
{
    #region Properties

    /// <summary>
    /// Ключ.
    /// </summary>
    [JsonProperty ("key")]
    public string? Key { get; set; }

    #endregion
}
