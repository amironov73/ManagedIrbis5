// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Reflection;

using AM;

#endregion

#nullable enable

namespace PdfSharpCore.Pdf;

/// <summary>
/// Holds information about the value of a key in a dictionary. This information is used to create
/// and interpret this value.
/// </summary>
internal sealed class KeyDescriptor
{
    #region Construction

    /// <summary>
    /// Initializes a new instance of KeyDescriptor from the specified attribute during a KeysMeta
    /// initializes itself using reflection.
    /// </summary>
    public KeyDescriptor
        (
            KeyInfoAttribute attribute
        )
    {
        Sure.NotNull (attribute);

        Version = attribute.Version;
        KeyType = attribute.KeyType;
        FixedValue = attribute.FixedValue;
        ObjectType = attribute.ObjectType;

        if (Version == "")
        {
            Version = "1.0";
        }
    }

    #endregion

    /// <summary>
    /// Gets or sets the PDF version starting with the availability of the described key.
    /// </summary>
    public string Version { get; set; }

    /// <summary>
    ///
    /// </summary>
    public KeyType KeyType { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? KeyValue { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? FixedValue { get; }

    /// <summary>
    ///
    /// </summary>
    public Type? ObjectType { get; set; }

    /// <summary>
    ///
    /// </summary>
    public bool CanBeIndirect => (KeyType & KeyType.MustNotBeIndirect) == 0;

    /// <summary>
    /// Returns the type of the object to be created as value for the described key.
    /// </summary>
    public Type? GetValueType()
    {
        var type = ObjectType;
        if (type is null)
        {
            // If we have no ObjectType specified, use the KeyType enumeration.
            switch (KeyType & KeyType.TypeMask)
            {
                case KeyType.Name:
                    type = typeof (PdfName);
                    break;

                case KeyType.String:
                    type = typeof (PdfString);
                    break;

                case KeyType.Boolean:
                    type = typeof (PdfBoolean);
                    break;

                case KeyType.Integer:
                    type = typeof (PdfInteger);
                    break;

                case KeyType.Real:
                    type = typeof (PdfReal);
                    break;

                case KeyType.Date:
                    type = typeof (PdfDate);
                    break;

                case KeyType.Rectangle:
                    type = typeof (PdfRectangle);
                    break;

                case KeyType.Array:
                    type = typeof (PdfArray);
                    break;

                case KeyType.Dictionary:
                    type = typeof (PdfDictionary);
                    break;

                case KeyType.Stream:
                    type = typeof (PdfDictionary);
                    break;

                // The following types are not yet used

                case KeyType.NumberTree:
                    throw new NotImplementedException ("KeyType.NumberTree");

                case KeyType.NameOrArray:
                    throw new NotImplementedException ("KeyType.NameOrArray");

                case KeyType.ArrayOrDictionary:
                    throw new NotImplementedException ("KeyType.ArrayOrDictionary");

                case KeyType.StreamOrArray:
                    throw new NotImplementedException ("KeyType.StreamOrArray");

                case KeyType.ArrayOrNameOrString:
                    return null; // HACK: Make PdfOutline work

                //throw new NotImplementedException("KeyType.ArrayOrNameOrString");

                default:
                    Debug.Assert (false, "Invalid KeyType: " + KeyType);
                    break;
            }
        }

        return type;
    }
}

/// <summary>
/// Contains meta information about all keys of a PDF dictionary.
/// </summary>
internal class DictionaryMeta
{
    public DictionaryMeta (Type type)
    {
        // Rewritten for WinRT.
        CollectKeyDescriptors (type);

        //var fields = type.GetRuntimeFields();  // does not work
        //fields2.GetType();
        //foreach (FieldInfo field in fields)
        //{
        //    var attributes = field.GetCustomAttributes(typeof(KeyInfoAttribute), false);
        //    foreach (var attribute in attributes)
        //    {
        //        KeyDescriptor descriptor = new KeyDescriptor((KeyInfoAttribute)attribute);
        //        descriptor.KeyValue = (string)field.GetValue(null);
        //        _keyDescriptors[descriptor.KeyValue] = descriptor;
        //    }
        //}
    }

    // Background: The function GetRuntimeFields gets constant fields only for the specified type,
    // not for its base types. So we have to walk recursively through base classes.
    // The docmentation says full trust for the immediate caller is required for property BaseClass.
    // TODO: Rewrite this stuff for medium trust.
    void CollectKeyDescriptors (Type type)
    {
        // Get fields of the specified type only.
        var fields = type.GetTypeInfo().DeclaredFields;
        foreach (var field in fields)
        {
            var attributes = field.GetCustomAttributes (typeof (KeyInfoAttribute), false);
            foreach (var attribute in attributes)
            {
                var descriptor = new KeyDescriptor ((KeyInfoAttribute)attribute)
                {
                    KeyValue = (string?) field.GetValue (null)
                };
                _keyDescriptors[descriptor.KeyValue] = descriptor;
            }
        }

        type = type.GetTypeInfo().BaseType;
        if (type != typeof (object) && type != typeof (PdfObject))
        {
            CollectKeyDescriptors (type);
        }
    }

    /// <summary>
    /// Gets the KeyDescriptor of the specified key, or null if no such descriptor exits.
    /// </summary>
    public KeyDescriptor? this [string key]
    {
        get
        {
            _keyDescriptors.TryGetValue (key, out var keyDescriptor);
            return keyDescriptor;
        }
    }

    readonly Dictionary<string, KeyDescriptor> _keyDescriptors = new ();
}
