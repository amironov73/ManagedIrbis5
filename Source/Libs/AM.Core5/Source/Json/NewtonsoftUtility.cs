// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo

/* NewtonsoftUtility.cs -- возня с Newtonsoft.Json
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

using JetBrains.Annotations;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#endregion

#nullable enable

namespace AM.Json;

/// <summary>
/// Возня с <code>Newtonsoft.Json</code>.
/// </summary>
[PublicAPI]
public static class NewtonsoftUtility
{
    #region Public methods

    /// <summary>
    /// Expand $type's.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static void ExpandTypes
        (
            JObject obj,
            string nameSpace,
            string assembly
        )
    {
        Sure.NotNull (obj);
        Sure.NotNullNorEmpty (nameSpace);
        Sure.NotNullNorEmpty (assembly);

        var values = obj
            .SelectTokens ("$..$type")
            .OfType<JValue>();
        foreach (var value in values)
        {
            if (value.Value is not null)
            {
                var typeName = value.Value.ToString();
                if (!string.IsNullOrEmpty (typeName))
                {
                    if (!typeName.Contains ('.'))
                    {
                        typeName = nameSpace
                                   + "."
                                   + typeName
                                   + ", "
                                   + assembly;
                        value.Value = typeName;
                    }
                }
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static void Include
        (
            JObject obj,
            Action<JProperty> resolver
        )
    {
        Sure.NotNull (obj);
        Sure.NotNull (resolver);

        var values = obj
            .SelectTokens ("$..$include")
            .OfType<JValue>()
            .ToArray();

        foreach (var value in values)
        {
            if (value.Parent is not null)
            {
                var property = (JProperty)value.Parent;
                resolver (property);
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static void Include
        (
            JObject obj
        )
    {
        Sure.NotNull (obj);

        var tokens = obj
            .SelectTokens ("$..$include")
            .ToArray();

        foreach (var token in tokens)
        {
            if (token.Parent != null)
            {
                var property = (JProperty)token.Parent;
                Resolve (property);
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static void Include
        (
            JObject obj,
            string newName
        )
    {
        Sure.NotNull (obj);
        Sure.NotNullNorEmpty (newName);

        void Resolver (JProperty prop)
        {
            Resolve (prop, newName);
        }

        Include (obj, Resolver);
    }

    /// <summary>
    /// Read <see cref="JArray"/> from specified
    /// local file.
    /// </summary>
    public static JArray ReadArrayFromFile
        (
            string fileName
        )
    {
        Sure.FileExists (fileName, nameof (fileName));

        var text = File.ReadAllText (fileName);
        var result = JArray.Parse (text);

        return result;
    }

    /// <summary>
    /// Read <see cref="JObject"/> from specified
    /// local JSON file.
    /// </summary>
    public static JObject ReadObjectFromFile
        (
            string fileName
        )
    {
        Sure.NotNullNorEmpty (fileName);

        var text = File.ReadAllText (fileName);
        var result = JObject.Parse (text);

        return result;
    }

    /// <summary>
    /// Read arbitrary object from specified
    /// local JSON file.
    /// </summary>
    public static T ReadObjectFromFile<T>
        (
            string fileName
        )
    {
        Sure.FileExists (fileName);

        var text = File.ReadAllText (fileName);
        var result = JsonConvert.DeserializeObject<T> (text);

        return result!;
    }

    /// <summary>
    /// Save the <see cref="JArray"/>
    /// to the specified local file.
    /// </summary>
    public static void SaveArrayToFile
        (
            JArray array,
            string fileName
        )
    {
        Sure.NotNull (array);
        Sure.NotNullNorEmpty (fileName);

        var text = array.ToString (Formatting.Indented);
        File.WriteAllText (fileName, text);
    }

    /// <summary>
    /// Save object to the specified local JSON file.
    /// </summary>
    public static void SaveObjectToFile
        (
            JObject obj,
            string fileName
        )
    {
        Sure.NotNull (obj);
        Sure.NotNullNorEmpty (fileName);

        var text = obj.ToString (Formatting.Indented);
        File.WriteAllText (fileName, text);
    }

    /// <summary>
    /// Save object to the specified local JSON file.
    /// </summary>
    public static void SaveObjectToFile
        (
            object obj,
            string fileName
        )
    {
        var json = JObject.FromObject (obj);

        SaveObjectToFile (json, fileName);
    }

    /// <summary>
    /// Resolver for <see cref="Include(JObject,Action{JProperty})"/>.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static void Resolve
        (
            JProperty property,
            string newName
        )
    {
        Sure.NotNull (property);
        Sure.NotNull (newName);

        // TODO use path for searching

        var fileName = property.Value.ToString();
        var text = File.ReadAllText (fileName);
        var value = JObject.Parse (text);
        var newProperty = new JProperty (newName, value);
        property.Replace (newProperty);
    }

    /// <summary>
    /// Resolver for <see cref="Include(JObject,Action{JProperty})"/>.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static void Resolve
        (
            JProperty property
        )
    {
        Sure.NotNull (property);

        // TODO use path for searching

        var obj = (JObject)property.Value;
        var newName = obj["name"]?.Value<string>();
        var fileName = obj["file"]?.Value<string>();
        if (!string.IsNullOrEmpty (newName)
            && !string.IsNullOrEmpty (fileName))
        {
            var text = File.ReadAllText (fileName);
            var value = JObject.Parse (text);
            var newProperty = new JProperty (newName, value);
            property.Replace (newProperty);
        }
    }

    /// <summary>
    /// Сериализация в строку с отступами.
    /// </summary>
    public static string SerializeIndented<T>
        (
            T obj
        )
    {
        var settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            Formatting = Formatting.Indented
        };
        var serializer = JsonSerializer.Create (settings);
        var writer = new StringWriter();
        serializer.Serialize (writer, obj);

        return writer.ToString();
    }

    /// <summary>
    /// Сериализация в короткую строку.
    /// </summary>
    public static string SerializeShort<T>
        (
            T obj
        )
    {
        var settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            Formatting = Formatting.None
        };
        var serializer = JsonSerializer.Create (settings);
        var writer = new StringWriter();
        serializer.Serialize (writer, obj);

        return writer.ToString();
    }

    #endregion
}
