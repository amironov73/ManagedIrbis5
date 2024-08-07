﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* IniFile.cs -- работа с INI-файлами
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;

using AM.Collections;
using AM.Runtime;
using AM.Text;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace AM.IO;

/// <summary>
/// Работа с INI-файлами.
/// </summary>
public class IniFile
    : MarshalByRefObject,
    IHandmadeSerializable,
    IEnumerable<IniFile.Section>,
    IDisposable
{
    #region Nested classes

    /// <summary>
    /// Line (element) of the INI-file.
    /// </summary>
    [DebuggerDisplay ("{Key}={Value}")]
    public sealed class Line
        : IHandmadeSerializable
    {
        #region Properties

        /// <summary>
        /// Key (name) of the element.
        /// </summary>
        public string Key { get; private set; }

        /// <summary>
        /// Value of the element.
        /// </summary>
        public string? Value
        {
            get => _value;
            set
            {
                _value = value;
                Modified = true;
            }
        }

        /// <summary>
        /// Modification flag.
        /// </summary>
        public bool Modified { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Line()
        {
            Key = string.Empty;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public Line
            (
                string key,
                string? value
            )
        {
            CheckKeyName (key);

            Key = key;
            _value = value;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public Line
            (
                string key,
                string? value,
                bool modified
            )
        {
            CheckKeyName (key);

            Key = key;
            _value = value;
            Modified = modified;
        }

        #endregion

        #region Private members

        private string? _value;

        #endregion

        #region Public methods

        /// <summary>
        /// Write the line to the stream.
        /// </summary>
        public void Write
            (
                TextWriter writer
            )
        {
            if (string.IsNullOrEmpty (Value))
            {
                writer.WriteLine (Key);
            }
            else
            {
                writer.WriteLine
                    (
                        "{0}={1}",
                        Key, Value
                    );
            }
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Sure.NotNull (reader, nameof (reader));

            Key = reader.ReadString();
            _value = reader.ReadNullableString();
            Modified = reader.ReadBoolean();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Sure.NotNull (writer, nameof (writer));

            writer
                .Write (Key);
            writer
                .WriteNullable (Value)
                .Write (Modified);
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() => $"{Key}={Value}";

        #endregion
    }

    // =========================================================

    /// <summary>
    /// INI-file section.
    /// </summary>
    public sealed class Section
        : IHandmadeSerializable,
            IEnumerable<Line>
    {
        #region Properties

        /// <summary>
        /// Count of lines.
        /// </summary>
        public int Count => _lines.Count;

        /// <summary>
        /// All the keys of the section.
        /// </summary>
        public IEnumerable<string> Keys
        {
            get
            {
                foreach (var line in _lines)
                {
                    yield return line.Key;
                }
            }
        }

        /// <summary>
        /// Section is modified?
        /// </summary>
        public bool Modified { get; set; }

        /// <summary>
        /// Section name.
        /// </summary>
        public string? Name
        {
            get => _name;
            set => SetName (value.ThrowIfNull());
        }

        /// <summary>
        /// INI-file.
        /// </summary>
        public IniFile Owner { get; private set; }

        /// <summary>
        /// Indexer.
        /// </summary>
        public string? this [string key]
        {
            get => GetValue (key, null);
            set => SetValue (key, value);
        }

        #endregion

        #region Construction

        internal Section
            (
                IniFile owner,
                string? name
            )
        {
            Owner = owner;
            _name = name;
            _lines = new NonNullCollection<Line>();
        }

        #endregion

        #region Private members

        private string? _name;

        private NonNullCollection<Line> _lines;

        #endregion

        #region Public methods

        /// <summary>
        /// Add new item to the section.
        /// </summary>
        public void Add
            (
                string key,
                string? value
            )
        {
            var line = new Line (key, value);
            Add (line);
        }

        /// <summary>
        /// Add new line to the section.
        /// </summary>
        public void Add
            (
                Line line
            )
        {
            Sure.NotNull (line, nameof (line));

            CheckKeyName (line.Key);
            if (ContainsKey (line.Key))
            {
                Magna.Logger.LogError
                    (
                        nameof (IniFile) + "::" + nameof (Add)
                        + ": duplicate key={Key}",
                        line.Key
                    );

                throw new ArgumentException ("duplicate key " + line.Key);
            }

            _lines.Add (line);
        }

        /// <summary>
        /// Apply to other section.
        /// </summary>
        public void ApplyTo
            (
                Section section
            )
        {
            Sure.NotNull (section, nameof (section));

            foreach (var line in this)
            {
                section[line.Key] = line.Value;
            }
        }

        /// <summary>
        /// Clear the section.
        /// </summary>
        public void Clear()
        {
            _lines.Clear();
            Modified = true;
            Owner.Modified = true;
        }

        /// <summary>
        /// Whether the section have line with given key?
        /// </summary>
        public bool ContainsKey
            (
                string key
            )
        {
            Sure.NotNullNorEmpty (key, nameof (key));

            foreach (var line in _lines)
            {
                if (line.Key.SameString (key))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Get value associated with specified key.
        /// </summary>
        public string? GetValue
            (
                string key,
                string? defaultValue
            )
        {
            CheckKeyName (key);

            foreach (var line in _lines)
            {
                if (line.Key.SameString (key))
                {
                    return line.Value;
                }
            }

            return defaultValue;
        }

        /// <summary>
        /// Get value associated with given key.
        /// </summary>
        public T? GetValue<T>
            (
                string key,
                T? defaultValue
            )
        {
            Sure.NotNullNorEmpty (key, nameof (key));

            var value = GetValue (key, null);
            if (string.IsNullOrEmpty (value))
            {
                return defaultValue;
            }

            var result = Utility.ConvertTo<T> (value);

            return result;
        }

        /// <summary>
        /// Remove specified key.
        /// </summary>
        public Section Remove
            (
                string key
            )
        {
            CheckKeyName (key);

            foreach (var line in _lines)
            {
                if (line.Key.SameString (key))
                {
                    _lines.Remove (line);
                    Modified = true;
                    Owner.Modified = true;
                    break;
                }
            }

            return this;
        }

        /// <summary>
        /// Set name of the section.
        /// </summary>
        public void SetName
            (
                string name
            )
        {
            Sure.NotNullNorEmpty (name, nameof (name));
            _name = name;
            Modified = true;
            Owner.Modified = true;
        }

        /// <summary>
        /// Set value associated with given key.
        /// </summary>
        public Section SetValue
            (
                string key,
                string? value
            )
        {
            CheckKeyName (key);

            Line? target = null;
            foreach (var line in _lines)
            {
                if (line.Key.SameString (key))
                {
                    target = line;
                    break;
                }
            }

            if (target is null)
            {
                target = new Line (key, value);
                _lines.Add (target);
            }

            target.Value = value;

            return this;
        }

        /// <summary>
        /// Set value associate with given key.
        /// </summary>
        public Section SetValue<T>
            (
                string key,
                T value
            )
        {
            CheckKeyName (key);

            if (value is null)
            {
                Remove (key);
            }
            else
            {
                var text = value.ToString();
                SetValue (key, text);
            }

            return this;
        }

        /// <summary>
        /// Try to get value for given key.
        /// </summary>
        public bool TryGetValue
            (
                string key,
                out string? value
            )
        {
            CheckKeyName (key);

            foreach (var line in _lines)
            {
                if (line.Key.SameString (key))
                {
                    value = line.Value;
                    return true;
                }
            }

            value = null;

            return false;
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            _name = reader.ReadNullableString();
            _lines = reader.ReadNonNullCollection<Line>();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer.WriteNullable (_name);
            writer.WriteCollection (_lines);
        }

        #endregion

        #region IEnumerable<Line> members

        [ExcludeFromCodeCoverage]
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <inheritdoc cref="IEnumerable{T}.GetEnumerator" />
        public IEnumerator<Line> GetEnumerator() => _lines.GetEnumerator();

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            var builder = StringBuilderPool.Shared.Get();
            builder
                .Append ($"[{Name}]")
                .AppendLine();

            foreach (var line in _lines)
            {
                builder.AppendLine (line.ToString());
            }

            return builder.ReturnShared();
        }

        #endregion
    }

    #endregion

    // =========================================================

    #region Properties

    /// <summary>
    /// Encoding.
    /// </summary>
    public Encoding? Encoding { get; set; }

    /// <summary>
    /// Name of the file.
    /// </summary>
    public string? FileName { get; set; }

    /// <summary>
    /// Modified?
    /// </summary>
    public bool Modified { get; set; }

    /// <summary>
    /// Section indexer.
    /// </summary>
    public Section? this [string sectionName] => GetSection (sectionName);

    /// <summary>
    /// Value indexer.
    /// </summary>
    public string? this
        [
            string sectionName,
            string keyName
        ]
    {
        get => GetValue (sectionName, keyName, null);
        set => SetValue (sectionName, keyName, value);
    }

    /// <summary>
    /// Writable?
    /// </summary>
    public bool Writable { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    public IniFile()
    {
        _sections = new NonNullCollection<Section>();
        Magna.Logger.LogTrace (nameof (IniFile) + "::Constructor");
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    public IniFile
        (
            string fileName,
            Encoding? encoding = null,
            bool writable = false
        )
        : this()
    {
        Sure.NotNullNorEmpty (fileName, nameof (fileName));

        Magna.Logger.LogTrace
            (
                nameof (IniFile) + "::Constructor"
                + ": fileName={Filename}",
                fileName
            );

        FileName = fileName;
        Encoding = encoding;
        Writable = writable;

        Read();
    }

    #endregion

    #region Private members

    private readonly NonNullCollection<Section> _sections;

    internal static void CheckKeyName
        (
            string keyName
        )
    {
        if (string.IsNullOrEmpty (keyName))
        {
            Magna.Logger.LogError
                (
                    nameof (IniFile) + "::" + nameof (CheckKeyName)
                    + ": keyName={KeyName}",
                    keyName.ToVisibleString()
                );

            throw new ArgumentException (nameof (keyName));
        }

        if (keyName.Contains ('='))
        {
            Magna.Logger.LogError
                (
                    nameof (IniFile) + "::" + nameof (CheckKeyName)
                    + ": keyName={KeyName}",
                    keyName
                );

            throw new ArgumentException (nameof (keyName));
        }
    }

    private static void _SaveSection
        (
            TextWriter writer,
            Section section
        )
    {
        if (!string.IsNullOrEmpty (section.Name))
        {
            writer.WriteLine
                (
                    "[{0}]", section.Name
                );
        }

        foreach (var line in section)
        {
            line.Write (writer);
        }
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Apply to the INI-file.
    /// </summary>
    public void ApplyTo
        (
            IniFile iniFile
        )
    {
        Sure.NotNull (iniFile, nameof (iniFile));

        foreach (var thisSection in this)
        {
            var name = thisSection.Name;
            if (!string.IsNullOrEmpty (name))
            {
                var otherSection = iniFile.GetOrCreateSection (name);
                thisSection.ApplyTo (otherSection);
            }
        }
    }

    /// <summary>
    /// Clear the INI-file.
    /// </summary>
    public IniFile Clear()
    {
        _sections.Clear();

        return this;
    }

    /// <summary>
    /// Clear modification flag in all sections and lines.
    /// </summary>
    public void ClearModification()
    {
        Modified = false;

        foreach (var section in _sections)
        {
            section.Modified = false;
            foreach (var line in section)
            {
                line.Modified = false;
            }
        }
    }

    /// <summary>
    /// Contains section with given name?
    /// </summary>
    public bool ContainsSection
        (
            string name
        )
    {
        CheckKeyName (name);

        foreach (var section in _sections)
        {
            if (section.Name.SameString (name))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Create section with specified name.
    /// </summary>
    public Section CreateSection
        (
            string name
        )
    {
        CheckKeyName (name);

        if (ContainsSection (name))
        {
            Magna.Logger.LogError
                (
                    nameof (IniFile) + "::" + nameof (CreateSection)
                    + ": duplicate name={Name}",
                    name
                );

            throw new ArgumentException ("duplicate name " + name);
        }

        var result = new Section (this, name);
        _sections.Add (result);

        return result;
    }

    /// <summary>
    /// Get or create (if not exist) section with given name.
    /// </summary>
    public Section GetOrCreateSection
        (
            string name
        )
    {
        CheckKeyName (name);

        var result = GetSection (name)
                     ?? CreateSection (name);

        return result;
    }

    /// <summary>
    /// Get section with given name.
    /// </summary>
    public Section? GetSection
        (
            string name
        )
    {
        CheckKeyName (name);

        foreach (var section in _sections)
        {
            if (section.Name.SameString (name))
            {
                return section;
            }
        }

        return null;
    }

    /// <summary>
    /// Get all the sections.
    /// </summary>
    public Section[] GetSections() => _sections.ToArray();

    /// <summary>
    /// Get value from the given section and key.
    /// </summary>
    public string? GetValue
        (
            string sectionName,
            string keyName,
            string? defaultValue
        )
    {
        var section = GetSection (sectionName);
        var result = section is null
            ? defaultValue
            : section.GetValue (keyName, defaultValue);

        return result;
    }

    /// <summary>
    /// Get value from the given section and key.
    /// </summary>
    public T? GetValue<T>
        (
            string sectionName,
            string keyName,
            T? defaultValue
        )
    {
        var section = GetSection (sectionName);
        var result = section is null
            ? defaultValue
            : section.GetValue (keyName, defaultValue);

        return result;
    }

    /// <summary>
    /// Merge the section.
    /// </summary>
    public void MergeSection
        (
            Section section
        )
    {
        var sectionName = section.Name;
        if (sectionName is null)
        {
            // TODO: слить с безымянной секцией
            return;
        }

        var found = GetSection (sectionName);
        if (found is null)
        {
            _sections.Add (section);
        }
        else
        {
            foreach (var key in section.Keys)
            {
                if (!found.ContainsKey (key))
                {
                    found[key] = section[key];
                }
            }
        }
    }

    /// <summary>
    /// Remove specified section.
    /// </summary>
    public IniFile RemoveSection
        (
            string name
        )
    {
        CheckKeyName (name);

        foreach (var section in _sections)
        {
            if (section.Name.SameString (name))
            {
                _sections.Remove (section);
                break;
            }
        }

        return this;
    }

    /// <summary>
    /// Remove specified value.
    /// </summary>
    public IniFile RemoveValue
        (
            string sectionName,
            string keyName
        )
    {
        var section = GetSection (sectionName);
        section?.Remove (keyName);

        return this;
    }

    /// <summary>
    /// Reread the <see cref="IniFile"/> from the file.
    /// </summary>
    public void Read()
    {
        if (string.IsNullOrEmpty (FileName))
        {
            return;
        }

        var encoding = Encoding ?? Encoding.Default;

        Read (FileName.ThrowIfNull(), encoding);
    }

    /// <summary>
    /// Reread from the file.
    /// </summary>
    public void Read
        (
            string fileName,
            Encoding encoding
        )
    {
        Sure.NotNullNorEmpty (fileName, nameof (fileName));
        Sure.NotNull (encoding, nameof (encoding));

        using var reader = TextReaderUtility.OpenRead
            (
                fileName,
                encoding
            );
        Read (reader);
    }

    /// <summary>
    /// Reread from the stream.
    /// </summary>
    public void Read
        (
            TextReader reader
        )
    {
        Sure.NotNull (reader, nameof (reader));

        char[] separators = { '=' };
        _sections.Clear();
        Section? section = null;

        while (reader.ReadLine() is { } line)
        {
            line = line.Trim();
            if (string.IsNullOrEmpty (line))
            {
                continue;
            }

            if (line.StartsWith ("["))
            {
                if (!line.EndsWith ("]"))
                {
                    Magna.Logger.LogError
                        (
                            nameof (IniFile) + "::" + nameof (Read)
                            + ": unclosed section name={Name}",
                            line
                        );

                    throw new FormatException();
                }

                var name = line.Substring (1, line.Length - 2);
                section = CreateSection (name);
            }
            else
            {
                if (section == null)
                {
                    section = new Section (this, null);
                    _sections.Add (section);
                }

                var parts = line.Split (separators, 2);

                var key = parts[0];
                if (!string.IsNullOrEmpty (key))
                {
                    var value = parts.Length == 2
                        ? parts[1]
                        : null;
                    section.SetValue (key, value);
                }
            }
        }

        ClearModification();
    }

    /// <summary>
    /// Write INI-file into the stream.
    /// </summary>
    public void Save
        (
            TextWriter writer
        )
    {
        Sure.NotNull (writer, nameof (writer));

        var first = true;
        foreach (var section in _sections)
        {
            if (!first)
            {
                writer.WriteLine();
            }

            _SaveSection
                (
                    writer,
                    section
                );

            first = false;
        }

        Modified = false;
    }

    /// <summary>
    /// Save the INI-file to specified file.
    /// </summary>
    public void Save
        (
            string fileName
        )
    {
        Sure.NotNullNorEmpty (fileName, nameof (fileName));

        var encoding = Encoding ?? Encoding.Default;

        using var writer = TextWriterUtility.Create
            (
                fileName,
                encoding
            );
        Save (writer);
    }

    /// <summary>
    /// Set value for specified section and key.
    /// </summary>
    public IniFile SetValue
        (
            string sectionName,
            string keyName,
            string? value
        )
    {
        var section = GetOrCreateSection (sectionName);
        section.SetValue (keyName, value);

        return this;
    }

    /// <summary>
    /// Set value for specified section and key.
    /// </summary>
    public IniFile SetValue<T>
        (
            string sectionName,
            string keyName,
            T value
        )
    {
        var section = GetOrCreateSection (sectionName);
        section.SetValue (keyName, value);

        return this;
    }

    /// <summary>
    /// Write modified values to the stream.
    /// </summary>
    public void WriteModifiedValues
        (
            TextWriter writer
        )
    {
        Sure.NotNull (writer, nameof (writer));

        var first = true;
        foreach (var section in _sections)
        {
            var lines = section
                .Where (line => line.Modified)
                .ToArray();

            if (lines.Length != 0)
            {
                if (!first)
                {
                    writer.WriteLine();
                }

                if (!string.IsNullOrEmpty (section.Name))
                {
                    writer.WriteLine
                        (
                            "[{0}]",
                            section.Name
                        );
                }

                foreach (var line in lines)
                {
                    line.Write (writer);
                }

                first = false;
            }
            else if (section.Modified)
            {
                if (!first)
                {
                    writer.WriteLine();
                }

                _SaveSection (writer, section);
                first = false;
            }
        }
    }

    #endregion

    #region IHandmadeSerializable

    /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
    public void RestoreFromStream
        (
            BinaryReader reader
        )
    {
        Sure.NotNull (reader, nameof (reader));

        FileName = reader.ReadNullableString();
        var encodingName = reader.ReadNullableString();
        Encoding = string.IsNullOrEmpty (encodingName)
            ? null
            : Encoding.GetEncoding (encodingName);
        Modified = reader.ReadBoolean();
        _sections.Clear();
        var count = reader.ReadPackedInt32();
        for (var i = 0; i < count; i++)
        {
            var section = new Section (this, null);
            section.RestoreFromStream (reader);
            _sections.Add (section);
        }
    }

    /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
    public void SaveToStream
        (
            BinaryWriter writer
        )
    {
        Sure.NotNull (writer, nameof (writer));

        writer.WriteNullable (FileName);

        var encodingName = Encoding?.EncodingName;
        writer.WriteNullable (encodingName);
        writer.Write (Modified);
        writer.WritePackedInt32 (_sections.Count);
        foreach (var section in _sections)
        {
            section.SaveToStream (writer);
        }
    }

    #endregion

    #region IEnumerable<Section> members

    [ExcludeFromCodeCoverage]
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <inheritdoc cref="IEnumerable{T}.GetEnumerator" />
    public IEnumerator<Section> GetEnumerator() => _sections.GetEnumerator();

    #endregion

    #region IDisposable members

    /// <inheritdoc cref="IDisposable.Dispose" />
    public void Dispose()
    {
        Magna.Logger.LogTrace (nameof (IniFile) + "::" + nameof (Dispose));

        if (Writable
            && Modified
            && !string.IsNullOrEmpty (FileName))
        {
            Save (FileName.ThrowIfNull());
        }
    }

    #endregion
}
