// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global
// ReSharper disable CommentTypo
// ReSharper disable EventNeverSubscribedTo.Global
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedMemberInSuper.Global
// ReSharper disable UnusedParameter.Local
// ReSharper disable VirtualMemberCallInConstructor

/* DawgContainer.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

#endregion

#nullable enable

namespace AM.Dawg;

/// <summary>
///
/// </summary>
public sealed class DawgContainer<TPayload>
    : IEnumerable<KeyValuePair<string, TPayload>>
{
    #region Construction

    /// <summary>
    ///
    /// </summary>
    internal DawgContainer (IDawg<TPayload> dawg)
    {
        _dawg = dawg;
    }

    #endregion

    #region Private members

    readonly IDawg<TPayload> _dawg;

    private void Save
        (
            Stream stream,
            Action<OldDawg<TPayload>, BinaryWriter> save
        )
    {
        // Do not close the BinaryWriter. Users might want to append more data to the stream.
        var writer = new BinaryWriter (stream);
        writer.Write (GetSignature());
        save ((OldDawg<TPayload>) _dawg, writer);
    }

    private static int GetSignature()
    {
        var bytes = Encoding.UTF8.GetBytes ("DAWG");

        return unchecked (bytes[0]
            + bytes[1] << 8
            + bytes[2] << 16
            + bytes[3] << 24);
    }

    private static Action<BinaryWriter, TPayload> GetStandardWriter()
    {
        return BuiltinTypeIO.GetWriter<TPayload>();
    }

    /// <summary>
    ///
    /// </summary>
    private static IDawg<TPayload> LoadIDawg
        (
            Stream stream,
            Func<BinaryReader, TPayload> readPayload
        )
    {
        Sure.NotNull (stream);
        Sure.NotNull (readPayload);

        using var reader = new BinaryReader (stream);
        var signature = GetSignature();
        var firstInt = reader.ReadInt32();
        if (firstInt == signature)
        {
            var version = reader.ReadInt32();
            return version switch
            {
                1 => new MatrixDawg<TPayload> (reader, readPayload),
                2 => new YaleDawg<TPayload> (reader, readPayload),
                _ => throw new Exception ("This file was produced by a more recent version of DawgSharp.")
            };
        }

        // The old, unversioned, file format had the number of nodes as the first 4 bytes of the stream.
        // It is extremely unlikely that they happen to be exactly the same as the signature "DAWG".
        // return LoadOldDawg (reader, firstInt, readPayload);
        return LoadOldDawg (reader, firstInt, readPayload);
    }

    private static OldDawg<TPayload> LoadOldDawg
        (
            BinaryReader reader,
            int nodeCount,
            Func<BinaryReader, TPayload> readPayload
        )
    {
        var nodes = new Node<TPayload>[nodeCount];
        var rootIndex = reader.ReadInt32();
        var chars = reader.ReadChars (nodeCount);
        for (var i = 0; i < nodeCount; ++i)
        {
            var node = new Node<TPayload>();
            int childCount = reader.ReadInt16();

            while (childCount-- > 0)
            {
                var childIndex = reader.ReadInt32();

                node.Children.Add (chars[childIndex], nodes[childIndex]);
            }

            node.Payload = readPayload (reader);

            nodes[i] = node;
        }

        return new OldDawg<TPayload> (nodes[rootIndex]);
    }

    /// <summary>
    /// This method is only used for testing.
    /// </summary>
    [Obsolete ("This method is only used for testing.")]
    internal void SaveAsMatrixDawg
        (
            Stream stream,
            Action<BinaryWriter, TPayload>? writePayload = null
        )
    {
        Save (stream, (d, w) => d.root.SaveAsMatrixDawg (w, writePayload ?? GetStandardWriter()));
    }

    /// <summary>
    /// This method is only used for testing.
    /// </summary>
    [Obsolete ("This method is only used for testing.")]
    internal void SaveAsYaleDawg
        (
            Stream stream,
            Action<BinaryWriter, TPayload>? writePayload = null
        )
    {
        Save (stream, (d, w) => d.root.SaveAsYaleDawg (w, writePayload ?? GetStandardWriter()));
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Индексатор.
    /// </summary>
    public TPayload this [IEnumerable<char> word]
    {
        get
        {
            Sure.NotNull (word);

            return _dawg[word];
        }
    }

    /// <summary>
    ///
    /// </summary>
    public int GetLongestCommonPrefixLength
        (
            IEnumerable<char> word
        )
    {
        Sure.NotNull (word);

        return _dawg.GetLongestCommonPrefixLength (word);
    }

    /// <summary>
    /// Returns all items with a given word.
    /// </summary>
    public IEnumerable<KeyValuePair<string, TPayload>> MatchPrefix
        (
            IEnumerable<char> prefix
        )
    {
        Sure.NotNull (prefix);

        return _dawg.MatchPrefix (prefix);
    }

    /// <summary>
    /// Returns all items that are substrings of a given word.
    /// </summary>
    public IEnumerable<KeyValuePair<string, TPayload>> GetPrefixes
        (
            IEnumerable<char> word
        )
    {
        Sure.NotNull (word);

        return _dawg.GetPrefixes (word);
    }

    /// <summary>
    ///
    /// </summary>
    public int GetNodeCount()
    {
        return _dawg.GetNodeCount();
    }

    /// <summary>
    /// Save the DAWG to a file / stream.
    /// </summary>
    /// <param name="stream">Поток.</param>
    /// <param name="writePayload">Optional, can be null for basic types (int, string, etc).</param>
    public void SaveTo
        (
            Stream stream,
            Action<BinaryWriter, TPayload>? writePayload = null
        )
    {
        Sure.NotNull (stream);

#pragma warning disable 618
        SaveAsYaleDawg (stream, writePayload ?? GetStandardWriter());
#pragma warning restore 618
    }

    /// <summary>
    ///
    /// </summary>
    public static DawgContainer<TPayload> Load
        (
            Stream stream,
            Func<BinaryReader, TPayload>? readPayload = null
        )
    {
        Sure.NotNull (stream);

        return new (LoadIDawg (stream, readPayload ?? BuiltinTypeIO.GetReader<TPayload>()));
    }

    /// <summary>
    ///
    /// </summary>
    public KeyValuePair<string, TPayload> GetRandomItem
        (
            Random random
        )
    {
        Sure.NotNull (random);

        return _dawg.GetRandomItem (random);
    }

    #endregion

    #region IEnumerable<T> members

    /// <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
    public IEnumerator<KeyValuePair<string, TPayload>> GetEnumerator()
    {
        return MatchPrefix (string.Empty).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    #endregion
}
