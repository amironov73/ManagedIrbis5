// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* Config.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Configuration;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.FictionBook;

/// <summary>
///
/// </summary>
public class GenreSubstitutionElement
    : ConfigurationElement, IComparable
{
    /// <summary>
    ///
    /// </summary>
    [ConfigurationProperty ("from", IsRequired = true)]
    public string From
    {
        get => (string)this["from"];
        set => this["from"] = value;
    }

    /// <summary>
    ///
    /// </summary>
    [ConfigurationProperty ("to", DefaultValue = "Unknown")]
    public string To
    {
        get => (string)this["to"];
        set => this["to"] = value;
    }

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return $"{To} ({From})";
    }

    /// <inheritdoc cref="IComparable.CompareTo"/>
    public int CompareTo
        (
            object? obj
        )
    {
        return obj is GenreSubstitutionElement
            ? String.Compare (ToString(), obj.ToString(), StringComparison.Ordinal) : 0;
    }
}

/// <summary>
///
/// </summary>
public class EncodingElement
    : ConfigurationElement
{
    /// <summary>
    ///
    /// </summary>
    [ConfigurationProperty ("name", IsRequired = true)]
    public string Name
    {
        get => (string)this["name"];
        set => this["name"] = value;
    }
}

/// <summary>
///
/// </summary>
public class RenameProfileElement : ConfigurationElement
{
    /// <summary>
    ///
    /// </summary>
    [ConfigurationProperty ("name", IsRequired = true)]
    public string Name
    {
        get => (string)this["name"];
        set => this["name"] = value;
    }

    /// <summary>
    ///
    /// </summary>
    [ConfigurationProperty ("path", IsRequired = true)]
    public string Path
    {
        get => (string)this["path"];
        set => this["path"] = value;
    }

    /// <summary>
    ///
    /// </summary>
    [ConfigurationProperty ("fileName", IsRequired = true)]
    public string FileName
    {
        get => (string)this["fileName"];
        set => this["fileName"] = value;
    }
    /// <summary>
    ///
    /// </summary>

    [ConfigurationProperty ("characterSubstitution", IsDefaultCollection = true, IsRequired = false)]
    public CharacterSubstitutionCollection CharacterSubstitution =>
        (CharacterSubstitutionCollection)this["characterSubstitution"];
}

/// <summary>
///
/// </summary>
public class CharacterSubstitutionElement
    : ConfigurationElement
{
    /// <summary>
    ///
    /// </summary>
    [ConfigurationProperty ("from", IsRequired = true)]
    [StringValidator (InvalidCharacters = "\\")]
    public string From
    {
        get => (string)this["from"];
        set => this["from"] = value;
    }

    /// <summary>
    ///
    /// </summary>
    [ConfigurationProperty ("to", DefaultValue = "")]
    public string To
    {
        get => (string)this["to"];
        set => this["to"] = value;
    }

    /// <summary>
    ///
    /// </summary>
    [ConfigurationProperty ("repeat", DefaultValue = 1)]
    [IntegerValidator (MinValue = 1, MaxValue = 50)]
    public int Repeat
    {
        get => (int)this["repeat"];
        set => this["repeat"] = value;
    }
}

/// <summary>
///
/// </summary>
public class CommandTypeElement
    : ConfigurationElement
{
    /// <summary>
    ///
    /// </summary>
    [ConfigurationProperty ("checkedFiles", IsDefaultCollection = true, IsRequired = true)]
    public CommandsCollection CheckedFilesCommands => (CommandsCollection) this["checkedFiles"];

    /// <summary>
    ///
    /// </summary>
    [ConfigurationProperty ("focusedFile", IsDefaultCollection = true, IsRequired = true)]
    public CommandsCollection FocusedFileCommand => (CommandsCollection) this["focusedFile"];
}

/// <summary>
///
/// </summary>
public class CommandElement
    : ConfigurationElement
{
    /// <summary>
    ///
    /// </summary>
    [ConfigurationProperty ("name", IsRequired = true)]
    public string Name
    {
        get => (string)this["name"];
        set => this["name"] = value;
    }

    /// <summary>
    ///
    /// </summary>
    [ConfigurationProperty ("fileName", IsRequired = true)]
    public string FileName
    {
        get => (string)this["fileName"];
        set => this["fileName"] = value;
    }

    /// <summary>
    ///
    /// </summary>
    [ConfigurationProperty ("arguments", IsRequired = false, DefaultValue = "")]
    public string Arguments
    {
        get => (string)this["arguments"];
        set => this["arguments"] = value;
    }

    /// <summary>
    ///
    /// </summary>
    [ConfigurationProperty ("createNoWindow", IsRequired = false, DefaultValue = false)]
    public bool CreateNoWindow
    {
        get => (bool)this["createNoWindow"];
        set => this["createNoWindow"] = value;
    }

    /// <summary>
    ///
    /// </summary>
    [ConfigurationProperty ("onlyWithExtension", IsRequired = false, DefaultValue = "")]
    public string OnlyWithExtension
    {
        get => (string)this["onlyWithExtension"];
        set => this["onlyWithExtension"] = value;
    }

    /// <summary>
    ///
    /// </summary>
    [ConfigurationProperty ("waitAndReload", IsRequired = false, DefaultValue = true)]
    public bool WaitAndReload
    {
        get => (bool)this["waitAndReload"];
        set => this["waitAndReload"] = value;
    }
}
/// <summary>
///
/// </summary>

[ConfigurationCollection (typeof (CommandElement), CollectionType = ConfigurationElementCollectionType.BasicMap,
    AddItemName = "command")]
public class CommandsCollection
    : ConfigurationElementCollection
{
    /// <inheritdoc cref="ConfigurationElementCollection.CreateNewElement()"/>
    protected override ConfigurationElement CreateNewElement()
    {
        return new CommandElement();
    }

    /// <inheritdoc cref="ConfigurationElementCollection.GetElementKey"/>
    protected override object GetElementKey (ConfigurationElement element)
    {
        return (element as CommandElement)!.Name;
    }
}

/// <summary>
///
/// </summary>
[ConfigurationCollection (typeof (CharacterSubstitutionElement),
    CollectionType = ConfigurationElementCollectionType.BasicMap, AddItemName = "char")]
public class CharacterSubstitutionCollection
    : ConfigurationElementCollection
{
    /// <inheritdoc cref="ConfigurationElementCollection.CreateNewElement()"/>
    protected override ConfigurationElement CreateNewElement()
    {
        return new CharacterSubstitutionElement();
    }

    /// <inheritdoc cref="ConfigurationElementCollection.GetElementKey"/>
    protected override object GetElementKey
        (
            ConfigurationElement element
        )
    {
        return (element as CharacterSubstitutionElement)!.From;
    }
}

/// <summary>
///
/// </summary>
[ConfigurationCollection (typeof (GenreSubstitutionElement),
    CollectionType = ConfigurationElementCollectionType.BasicMap, AddItemName = "genre")]
public class GenresCollection
    : ConfigurationElementCollection
{
    /// <inheritdoc cref="ConfigurationElementCollection.CreateNewElement()"/>
    protected override ConfigurationElement CreateNewElement()
    {
        return new GenreSubstitutionElement();
    }

    /// <inheritdoc cref="ConfigurationElementCollection.GetElementKey"/>
    protected override object GetElementKey (ConfigurationElement element)
    {
        return (element as GenreSubstitutionElement)!.From;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="genreName"></param>
    /// <returns></returns>
    public string FindSubstitution
        (
            string genreName
        )
    {
        if (string.IsNullOrEmpty (genreName))
        {
            return string.Empty;
        }

        foreach (GenreSubstitutionElement gs in this)
        {
            if (String.Equals (gs.From, genreName, StringComparison.InvariantCultureIgnoreCase))
            {
                return gs.To;
            }
        }

        return genreName;
    }
}

/// <summary>
///
/// </summary>
[ConfigurationCollection (typeof (EncodingElement), CollectionType = ConfigurationElementCollectionType.BasicMap,
    AddItemName = "encoding")]
public class EncodingsCollection
    : ConfigurationElementCollection
{
    /// <inheritdoc cref="ConfigurationElementCollection.CreateNewElement()"/>
    protected override ConfigurationElement CreateNewElement()
    {
        return new EncodingElement();
    }

    /// <inheritdoc cref="ConfigurationElementCollection.GetElementKey"/>
    protected override object GetElementKey (ConfigurationElement element)
    {
        return (element as EncodingElement)!.Name;
    }

    /// <summary>
    ///
    /// </summary>
    [ConfigurationProperty ("translateEncodings", DefaultValue = true)]
    public bool TranslateEncodings
    {
        get => (bool)this["translateEncodings"];
        set => this["translateEncodings"] = value;
    }

    /// <summary>
    ///
    /// </summary>
    [ConfigurationProperty ("indentedFormatting", DefaultValue = true)]
    public bool IndentFile
    {
        get => (bool)this["indentedFormatting"];
        set => this["indentedFormatting"] = value;
    }

    /// <summary>
    ///
    /// </summary>
    [ConfigurationProperty ("compressionEncoding", IsRequired = false, DefaultValue = "utf-8")]
    public string CompressionEncoding
    {
        get => (string)this["compressionEncoding"];
        set => this["compressionEncoding"] = value;
    }
}

/// <summary>
///
/// </summary>
[ConfigurationCollection (typeof (RenameProfileElement), CollectionType = ConfigurationElementCollectionType.BasicMap,
    AddItemName = "profile")]
public class RenameProfilesCollection
    : ConfigurationElementCollection
{
    /// <summary>
    ///
    /// </summary>
    [ConfigurationProperty ("globalTranslit", IsDefaultCollection = true, IsRequired = false)]
    public CharacterSubstitutionCollection GlobalTranslit => (CharacterSubstitutionCollection)this["globalTranslit"];

    /// <summary>
    ///
    /// </summary>
    [ConfigurationProperty ("globalCharacterSubstitution", IsDefaultCollection = true, IsRequired = false)]
    public CharacterSubstitutionCollection GlobalCharacterSubstitution => (CharacterSubstitutionCollection)this["globalCharacterSubstitution"];

    /// <inheritdoc cref="ConfigurationElementCollection.CreateNewElement()"/>
    protected override ConfigurationElement CreateNewElement()
    {
        return new RenameProfileElement();
    }

    /// <inheritdoc cref="ConfigurationElementCollection.GetElementKey"/>
    protected override object GetElementKey (ConfigurationElement element)
    {
        return (element as RenameProfileElement)!.Name;
    }
}

/// <summary>
///
/// </summary>
public class FB2ConfigurationSection
    : ConfigurationSection
{
    /// <summary>
    ///
    /// </summary>
    [ConfigurationProperty ("normalizeNames", IsRequired = false, DefaultValue = true)]
    public bool NormalizeNames
    {
        get => (bool)this["normalizeNames"];
        set => this["normalizeNames"] = value;
    }

    /// <summary>
    ///
    /// </summary>
    [ConfigurationProperty ("fb2Extension", IsRequired = false, DefaultValue = ".fb2")]
    public string FB2Extension
    {
        get => (string)this["fb2Extension"];
        set => this["fb2Extension"] = value;
    }

    /// <summary>
    ///
    /// </summary>
    [ConfigurationProperty ("fb2zipExtension", IsRequired = false, DefaultValue = ".fb2.zip")]
    public string FB2ZIPExtension
    {
        get => (string)this["fb2zipExtension"];
        set => this["fb2zipExtension"] = value;
    }

    /// <summary>
    ///
    /// </summary>
    [ConfigurationProperty ("genreSubstitution", IsDefaultCollection = true, IsRequired = true)]
    public GenresCollection GenreSubstitutions => (GenresCollection)this["genreSubstitution"];

    /// <summary>
    ///
    /// </summary>
    [ConfigurationProperty ("encodings", IsDefaultCollection = true, IsRequired = true)]
    public EncodingsCollection Encodings => (EncodingsCollection)this["encodings"];

    /// <summary>
    ///
    /// </summary>
    [ConfigurationProperty ("renameProfiles", IsDefaultCollection = true, IsRequired = true)]
    public RenameProfilesCollection RenameProfiles => (RenameProfilesCollection)this["renameProfiles"];

    /// <summary>
    ///
    /// </summary>
    [ConfigurationProperty ("commands", IsRequired = true)]
    public CommandTypeElement Commands
    {
        get => (CommandTypeElement)this["commands"];
        set => this["commands"] = value;
    }
}

/// <summary>
///
/// </summary>
public class FB2Config
{
    private static FB2ConfigurationSection? _fb2Configuration;

    /// <summary>
    ///
    /// </summary>
    public static FB2ConfigurationSection Current =>
        _fb2Configuration ??= (ConfigurationManager.GetSection ("FB2") as FB2ConfigurationSection).ThrowIfNull();
}
