// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ResSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* JsonUtility.cs -- возня с JSON
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Json;

/// <summary>
/// Возня с JSON.
/// </summary>
[PublicAPI]
public static class JsonUtility
{
    #region Public methods

    /// <summary>
    /// Serialize to indented string.
    /// </summary>
    public static string SerializeIndented<T>
        (
            T obj
        )
    {
        var options = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            WriteIndented = true,
        };
        var result = JsonSerializer.Serialize (obj, options);

        return result;
    }

    /// <summary>
    /// Serialize to short string.
    /// </summary>
    public static string SerializeShort<T>
        (
            T obj
        )
    {
        var options = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            WriteIndented = false,
        };
        var result = JsonSerializer.Serialize (obj, options);

        return result;
    }

    /// <summary>
    /// Временная заглушка.
    /// </summary>
    public static T ReadObjectFromFile<T>
        (
            string fileName
        )
        where T : class
    {
        var content = File.ReadAllBytes (fileName);
        var options = new JsonReaderOptions
        {
            CommentHandling = JsonCommentHandling.Skip
        };
        var reader = new Utf8JsonReader (content, options);

        return JsonSerializer.Deserialize<T> (ref reader).ThrowIfNull();
    }

    /// <summary>
    /// Временная заглушка.
    /// </summary>
    public static void SaveObjectToFile
        (
            object obj,
            string fileName
        )
    {
        var content = JsonSerializer.Serialize (obj);
        File.WriteAllText (fileName, content);
    }

    #endregion
}
