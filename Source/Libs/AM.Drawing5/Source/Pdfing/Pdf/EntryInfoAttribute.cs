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

using AM;

#endregion

namespace PdfSharpCore.Pdf;

/// <summary>
/// Specifies the type of a key's value in a dictionary.
/// </summary>
[Flags]
internal enum KeyType
{
    /// <summary>
    ///
    /// </summary>
    Name = 0x00000001,

    /// <summary>
    ///
    /// </summary>
    String = 0x00000002,

    /// <summary>
    ///
    /// </summary>
    Boolean = 0x00000003,

    /// <summary>
    ///
    /// </summary>
    Integer = 0x00000004,

    /// <summary>
    ///
    /// </summary>
    Real = 0x00000005,

    /// <summary>
    ///
    /// </summary>
    Date = 0x00000006,

    /// <summary>
    ///
    /// </summary>
    Rectangle = 0x00000007,

    /// <summary>
    ///
    /// </summary>
    Array = 0x00000008,

    /// <summary>
    ///
    /// </summary>
    Dictionary = 0x00000009,

    /// <summary>
    ///
    /// </summary>
    Stream = 0x0000000A,

    /// <summary>
    ///
    /// </summary>
    NumberTree = 0x0000000B,

    /// <summary>
    ///
    /// </summary>
    Function = 0x0000000C,

    /// <summary>
    ///
    /// </summary>
    TextString = 0x0000000D,

    /// <summary>
    ///
    /// </summary>
    ByteString = 0x0000000E,

    /// <summary>
    ///
    /// </summary>
    NameOrArray = 0x00000010,

    /// <summary>
    ///
    /// </summary>
    NameOrDictionary = 0x00000020,

    /// <summary>
    ///
    /// </summary>
    ArrayOrDictionary = 0x00000030,

    /// <summary>
    ///
    /// </summary>
    StreamOrArray = 0x00000040,

    /// <summary>
    ///
    /// </summary>
    StreamOrName = 0x00000050,

    /// <summary>
    ///
    /// </summary>
    ArrayOrNameOrString = 0x00000060,

    /// <summary>
    ///
    /// </summary>
    FunctionOrName = 0x000000070,

    /// <summary>
    ///
    /// </summary>
    Various = 0x000000080,

    /// <summary>
    ///
    /// </summary>
    TypeMask = 0x000000FF,

    /// <summary>
    ///
    /// </summary>
    Optional = 0x00000100,

    /// <summary>
    ///
    /// </summary>
    Required = 0x00000200,

    /// <summary>
    ///
    /// </summary>
    Inheritable = 0x00000400,

    /// <summary>
    ///
    /// </summary>
    MustBeIndirect = 0x00001000,

    /// <summary>
    ///
    /// </summary>
    MustNotBeIndirect = 0x00002000,
}

/// <summary>
/// Summary description for KeyInfo.
/// </summary>
internal class KeyInfoAttribute
    : Attribute
{
    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public KeyInfoAttribute()
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="keyType"></param>
    public KeyInfoAttribute
        (KeyType keyType
        )
    {
        //_version = version;
        KeyType = keyType;
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="version"></param>
    /// <param name="keyType"></param>
    public KeyInfoAttribute
        (
            string version,
            KeyType keyType
        )
    {
        Version = version;
        KeyType = keyType;
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="keyType"></param>
    /// <param name="objectType"></param>
    public KeyInfoAttribute
        (
            KeyType keyType,
            Type objectType
        )
    {
        KeyType = keyType;
        ObjectType = objectType;
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="version"></param>
    /// <param name="keyType"></param>
    /// <param name="objectType"></param>
    public KeyInfoAttribute
        (
            string version,
            KeyType keyType,
            Type objectType
        )
    {
        version.NotUsed();

        //_version = version;
        KeyType = keyType;
        ObjectType = objectType;
    }

    #endregion

    /// <summary>
    ///
    /// </summary>
    public string Version { get; set; } = "1.0";

    /// <summary>
    ///
    /// </summary>
    public KeyType KeyType { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Type? ObjectType { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? FixedValue { get; set; }
}
