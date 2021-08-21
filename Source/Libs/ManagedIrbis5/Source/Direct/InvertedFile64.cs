// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedParameter.Local

/* InvertedFile64.cs -- read inverted (index) file
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using AM;
using AM.Collections;
using AM.IO;
using AM.Logging;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Direct
{
    /// <summary>
    /// Read inverted (index) file of IRBIS64 database.
    /// </summary>
    public class InvertedFile64
        : IDisposable,
        ISupportLogging,
        IServiceProvider
    {
        #region Constants

        /// <summary>
        /// Длина записи N01/L01.
        /// </summary>
        public const int NodeLength = 2048;

        /// <summary>
        /// ibatrak максимальный размер термина
        /// </summary>
        public const int MaxTermSize = 255;

        /// <summary>
        /// ibatrak размер блока
        /// </summary>
        public const int BlockSize = 2050048;

        #endregion

        #region Properties

        /// <summary>
        /// Index file is fragmented.
        /// </summary>
        public bool Fragmented { get; private set; }

        /// <summary>
        /// File name.
        /// </summary>
        public string FileName { get; private set; }

        /// <summary>
        /// Access mode.
        /// </summary>
        public DirectAccessMode Mode { get; private set; }

        /// <summary>
        /// IFP file.
        /// </summary>
        public Stream? Ifp { get; private set; }

        /// <summary>
        /// Additional IFP files.
        /// </summary>
        public Stream[]? AdditionalIfp { get; private set; }

        /// <summary>
        /// Control record of the IFP file.
        /// </summary>
        public IfpControlRecord64 IfpControlRecord { get; private set; }

        /// <summary>
        /// Additional control records.
        /// </summary>
        public IfpControlRecord64[]? AdditionalControlRecord { get; set; }

        /// <summary>
        /// L01 node file.
        /// </summary>
        public Stream? L01 { get; private set; }

        /// <summary>
        /// Additional L01 node files.
        /// </summary>
        public Stream[]? AdditionalL01 { get; private set; }

        /// <summary>
        /// N01 node file.
        /// </summary>
        public Stream? N01 { get; private set; }

        /// <summary>
        /// Additional N01
        /// </summary>
        public Stream[]? AdditionalN01 { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public InvertedFile64
            (
                string fileName,
                DirectAccessMode mode = DirectAccessMode.Exclusive,
                IServiceProvider? serviceProvider = null
            )
        {
            Sure.NotNullNorEmpty(fileName, nameof(fileName));

            _serviceProvider = serviceProvider ?? Magna.Host.Services;
            _logger = (ILogger?) GetService(typeof(ILogger<MstFile64>));
            _logger?.LogTrace($"{nameof(InvertedFile64)}::Constructor ({fileName}, {mode})");

            _lockObject = new object();
            _encoding = new UTF8Encoding(false, true);

            FileName = Unix.FindFileOrThrow(fileName);
            Mode = mode;

            Ifp = DirectUtility.OpenFile(fileName, mode);
            IfpControlRecord = IfpControlRecord64.Read(Ifp);

            var l01Name = Unix.FindFileOrThrow(Path.ChangeExtension(fileName, ".l01"));
            L01 = DirectUtility.OpenFile(l01Name, mode);

            var n01Name = Unix.FindFileOrThrow(Path.ChangeExtension(fileName, ".n01"));
            N01 = DirectUtility.OpenFile(n01Name, mode);
        }

        #endregion

        #region Private members

        private ILogger? _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly object _lockObject;
        private readonly Encoding _encoding;

        private long _NodeOffset
            (
                int nodeNumber
            )
        {
            var result = (nodeNumber - 1) * (long)NodeLength;

            return result;
        }

        private NodeRecord64 _ReadNode
            (
                bool isLeaf,
                Stream stream,
                long offset
            )
        {
            lock (_lockObject)
            {
                stream.Position = offset;

                var result = new NodeRecord64(isLeaf)
                {
                    _stream = stream,
                    Leader =
                    {
                        Number = stream.ReadInt32Network(),
                        Previous = stream.ReadInt32Network(),
                        Next = stream.ReadInt32Network(),
                        TermCount = stream.ReadInt16Network(),
                        FreeOffset = stream.ReadInt16Network()
                    }
                };

                for (var i = 0; i < result.Leader.TermCount; i++)
                {
                    var item = new NodeItem64
                    {
                        Length = stream.ReadInt16Network(),
                        KeyOffset = stream.ReadInt16Network(),
                        LowOffset = stream.ReadInt32Network(),
                        HighOffset = stream.ReadInt32Network()
                    };
                    result.Items.Add(item);
                }

                foreach (var item in result.Items)
                {
                    stream.Position = offset + item.KeyOffset;
                    var buffer = StreamUtility.ReadBytes(stream, item.Length)
                        .ThrowIfNull("buffer");

                    var text = _encoding.GetString ( buffer );

                    item.Text = text;
                }

                return result;
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Read non-leaf node by number.
        /// </summary>
        public NodeRecord64 ReadNode
            (
                int number
            )
        {
            var result = _ReadNode
                (
                    false,
                    N01.ThrowIfNull("N01"),
                    _NodeOffset(number)
                );

            return result;
        }

        /// <summary>
        /// Read leaf node by number.
        /// </summary>
        public NodeRecord64 ReadLeaf
            (
                int number
            )
        {
            number = Math.Abs(number);
            var result = _ReadNode
                (
                    true,
                    L01.ThrowIfNull("L01"),
                    _NodeOffset(number)
                );

            return result;
        }

        /// <summary>
        /// Read next node.
        /// </summary>
        /// <returns><c>null</c> if there is no next node.
        /// </returns>
        public NodeRecord64? ReadNext
            (
                NodeRecord64 record
            )
        {
            var number = record.Leader.Next;

            if (number < 0)
            {
                return null;
            }

            var result = _ReadNode
                (
                    record.IsLeaf,
                    record._stream.ThrowIfNull("record._stream"),
                    _NodeOffset(number)
                );

            return result;
        }

        /// <summary>
        /// Read previous node.
        /// </summary>
        /// <returns><c>null</c> if there is no previous node.
        /// </returns>
        public NodeRecord64? ReadPrevious
            (
                NodeRecord64 record
            )
        {
            var number = record.Leader.Previous;
            if (number < 0)
            {
                return null;
            }

            var result = _ReadNode
                (
                    record.IsLeaf,
                    record._stream.ThrowIfNull("record._stream"),
                    _NodeOffset(number)
                );

            return result;
        }

        /// <summary>
        /// Read <see cref="IfpRecord64"/> from given offset.
        /// </summary>
        public IfpRecord64 ReadIfpRecord
            (
                long offset
            )
        {
            IfpRecord64 result = IfpRecord64.Read
                (
                    Ifp.ThrowIfNull("Ifp"),
                    offset
                );

            return result;
        }

        /// <summary>
        /// Read terms.
        /// </summary>
        public Term[] ReadTerms
            (
                TermParameters parameters
            )
        {
            // TODO Implement reverse order

            lock (_lockObject)
            {
                var key = parameters.StartTerm;
                if (string.IsNullOrEmpty(key))
                {
                    return Array.Empty<Term>();
                }

                var result = new LocalList<Term>();
                try
                {
                    key = key.ToUpperInvariant();

                    var firstNode = ReadNode(1);
                    var rootNode = ReadNode(firstNode.Leader.Number);
                    var currentNode = rootNode;

                    NodeItem64? goodItem = null, candidate = null;
                    var goodIndex = 0;
                    while (true)
                    {
                        var found = false;
                        var beyond = false;

                        if (ReferenceEquals(currentNode, null))
                        {
                            break;
                        }

                        for (var index = 0; index < currentNode.Leader.TermCount; index++)
                        {
                            var item = currentNode.Items[index];
                            var compareResult = string.CompareOrdinal
                            (
                                item.Text,
                                key
                            );
                            if (compareResult > 0)
                            {
                                candidate = item;
                                goodIndex = index;
                                beyond = true;
                                break;
                            }

                            goodItem = item;
                            goodIndex = index;
                            found = true;

                            if (compareResult == 0
                                && currentNode.IsLeaf)
                            {
                                goto FOUND;
                            }

                        }

                        if (ReferenceEquals(goodItem, null))
                        {
                            break;
                        }

                        if (found)
                        {
                            if (beyond || currentNode.Leader.Next == -1)
                            {
                                if (currentNode.IsLeaf)
                                {
                                    goodItem = candidate;
                                    goto FOUND;
                                }

                                currentNode = goodItem.RefersToLeaf
                                    ? ReadLeaf(goodItem.LowOffset)
                                    : ReadNode(goodItem.LowOffset);
                            }
                            else
                            {
                                currentNode = ReadNext(currentNode);
                            }
                        }
                        else
                        {
                            currentNode = goodItem.RefersToLeaf
                                ? ReadLeaf(goodItem.LowOffset)
                                : ReadNode(goodItem.LowOffset);
                        }
                    }

                    FOUND:
                    if (!ReferenceEquals(goodItem, null))
                    {
                        var count = parameters.NumberOfTerms;
                        while (count > 0)
                        {
                            if (ReferenceEquals(currentNode, null))
                            {
                                break;
                            }

                            var term = new Term
                            {
                                Text = goodItem.Text,
                                Count = 0
                            };
                            var offset = goodItem.FullOffset;
                            if (offset <= 0)
                            {
                                break;
                            }

                            IfpRecord64 ifp = ReadIfpRecord(offset);
                            term.Count += ifp.BlockLinkCount;
                            result.Add(term);
                            count--;
                            if (count > 0)
                            {
                                if (goodIndex >= currentNode.Leader.TermCount)
                                {
                                    currentNode = ReadNext(currentNode);
                                    goodIndex = 0;
                                }
                                else
                                {
                                    goodIndex++;
                                    goodItem = currentNode.Items[goodIndex];
                                }
                            }
                        }

                        return result.ToArray();
                    }
                }
                catch (Exception exception)
                {
                    Magna.TraceException
                        (
                            "InvertedFile64::SearchExact",
                            exception
                        );
                }
            }

            return Array.Empty<Term>();
        }

        /// <summary>
        /// Reopen files.
        /// </summary>
        public void ReopenFiles
            (
                DirectAccessMode mode
            )
        {
            if (Mode != mode)
            {
                lock (_lockObject)
                {
                    Mode = mode;

                    Ifp?.Dispose();
                    Ifp = DirectUtility.OpenFile(FileName, mode);
                    IfpControlRecord = IfpControlRecord64.Read(Ifp);

                    L01?.Dispose();
                    L01 = DirectUtility
                        .OpenFile(Path.ChangeExtension(FileName, ".l01"), mode);

                    N01?.Dispose();
                    N01 = DirectUtility
                        .OpenFile(Path.ChangeExtension(FileName, ".n01"), mode);
                }
            }
        }

        /// <summary>
        /// Search without truncation.
        /// </summary>
        public TermLink[] SearchExact
            (
                string? key
            )
        {
            if (string.IsNullOrEmpty(key))
            {
                return Array.Empty<TermLink>();
            }

            lock (_lockObject)
            {
                try
                {
                    key = key.ToUpperInvariant();

                    var firstNode = ReadNode(1);
                    var rootNode = ReadNode(firstNode.Leader.Number);
                    var currentNode = rootNode;

                    NodeItem64? goodItem = null;
                    while (true)
                    {
                        var found = false;
                        var beyond = false;

                        if (ReferenceEquals(currentNode, null))
                        {
                            break;
                        }

                        foreach (var item in currentNode.Items)
                        {
                            var compareResult = string.CompareOrdinal
                                (
                                    item.Text,
                                    key
                                );
                            if (compareResult > 0)
                            {
                                beyond = true;
                                break;
                            }

                            goodItem = item;
                            found = true;

                            if (compareResult == 0
                                && currentNode.IsLeaf)
                            {
                                goto FOUND;
                            }

                        }

                        if (ReferenceEquals(goodItem, null))
                        {
                            break;
                        }

                        if (found)
                        {
                            if (beyond || currentNode.Leader.Next == -1)
                            {
                                currentNode = goodItem.RefersToLeaf
                                    ? ReadLeaf(goodItem.LowOffset)
                                    : ReadNode(goodItem.LowOffset);
                            }
                            else
                            {
                                currentNode = ReadNext(currentNode);
                            }
                        }
                        else
                        {
                            currentNode = goodItem.RefersToLeaf
                                ? ReadLeaf(goodItem.LowOffset)
                                : ReadNode(goodItem.LowOffset);

                            if (currentNode.Items.Count == 0)
                            {
                                goodItem = null;
                                break;
                            }
                        }
                    }

                    FOUND:
                    if (goodItem != null)
                    {
                        // ibatrak записи могут иметь ссылки на следующие

                        var result = new LocalList<TermLink>();
                        var offset = goodItem.FullOffset;
                        while (offset > 0)
                        {
                            IfpRecord64 ifp = ReadIfpRecord(offset);
                            result.AddRange(ifp.Links);
                            offset = ifp.FullOffset > 0
                                ? ifp.FullOffset
                                : 0;
                        }

                        // TODO Implement .Distinct properly
                        return result
                            .ToArray()
                            .Distinct()
                            .ToArray();
                        // ibatrak до сюда
                    }
                }
                catch (Exception exception)
                {
                    Magna.TraceException
                        (
                            "InvertedFile64::SearchExact",
                            exception
                        );
                }
            }

            return Array.Empty<TermLink>();
        }

        /// <summary>
        /// Search with truncation.
        /// </summary>
        public TermLink[] SearchStart
            (
                string? key
            )
        {
            if (string.IsNullOrEmpty(key))
            {
                return Array.Empty<TermLink>();
            }

            lock (_lockObject)
            {
                var result = new List<TermLink>();

                key = key.ToUpperInvariant();

                var firstNode = ReadNode(1);
                var rootNode = ReadNode(firstNode.Leader.Number);
                var currentNode = rootNode;

                NodeItem64? goodItem = null;
                while (true)
                {
                    var found = false;
                    var beyond = false;

                    if (ReferenceEquals(currentNode, null))
                    {
                        break;
                    }

                    foreach (var item in currentNode.Items)
                    {
                        var compareResult = string.CompareOrdinal
                            (
                                item.Text,
                                key
                            );
                        if (compareResult > 0)
                        {
                            beyond = true;
                            break;
                        }

                        goodItem = item;
                        found = true;
                    }

                    if (goodItem == null)
                    {
                        break;
                    }

                    if (found)
                    {
                        if (beyond || currentNode.Leader.Next == -1)
                        {
                            if (goodItem.RefersToLeaf)
                            {
                                goto FOUND;
                            }

                            currentNode = ReadNode(goodItem.LowOffset);
                        }
                        else
                        {
                            currentNode = ReadNext(currentNode);
                        }
                    }
                    else
                    {
                        if (goodItem.RefersToLeaf)
                        {
                            goto FOUND;
                        }

                        currentNode = ReadNode(goodItem.LowOffset);
                    }
                }

                FOUND:
                if (goodItem != null)
                {
                    currentNode = ReadLeaf(goodItem.LowOffset);

                    while (true)
                    {
                        if (ReferenceEquals(currentNode, null))
                        {
                            break;
                        }

                        foreach (var item in currentNode.Items)
                        {
                            var compareResult = string.CompareOrdinal
                                (
                                    item.Text,
                                    key
                                );
                            if (compareResult >= 0)
                            {
                                var starts = item.Text!.StartsWith(key);
                                if (compareResult > 0 && !starts)
                                {
                                    goto DONE;
                                }

                                if (starts)
                                {
                                    //ibatrak записи могут иметь ссылки на следующие

                                    var offset = item.FullOffset;
                                    while (offset > 0)
                                    {
                                        IfpRecord64 ifp = ReadIfpRecord(offset);
                                        result.AddRange(ifp.Links);
                                        offset = ifp.FullOffset > 0
                                            ? ifp.FullOffset
                                            : 0;
                                    }

                                    //ibatrak до сюда
                                }
                            }
                        }

                        if (currentNode.Leader.Next > 0)
                        {
                            currentNode = ReadNext(currentNode);
                        }
                    }
                }

                DONE:
                return result
                    .Distinct()
                    .ToArray();
            }
        }

        /// <summary>
        /// Perform simple search.
        /// </summary>
        public int[] SearchSimple
            (
                string? key
            )
        {
            if (string.IsNullOrEmpty(key))
            {
                return Array.Empty<int>();
            }

            lock (_lockObject)
            {

                var result = Array.Empty<TermLink>();

                if (key.EndsWith("$"))
                {
                    key = key.Substring(0, key.Length - 1);
                    if (!string.IsNullOrEmpty(key))
                    {
                        result = SearchStart(key);
                    }
                }
                else
                {
                    result = SearchExact(key);
                }

                return result
                    .Select(link => link.Mfn)
                    .Distinct()
                    .ToArray();
            }
        }

        #endregion

        #region ISupportLogging members

        /// <inheritdoc cref="ISupportLogging.Logger"/>
        // TODO implement
        public ILogger? Logger => _logger;

        /// <inheritdoc cref="ISupportLogging.SetLogger"/>
        public void SetLogger(ILogger? logger)
            => _logger = logger;

        #endregion

        #region IServiceProvider members

        /// <inheritdoc cref="IServiceProvider.GetService"/>
        public object? GetService(Type serviceType)
            => _serviceProvider.GetService(serviceType);

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose" />
        public void Dispose()
        {
            _logger?.LogTrace($"{nameof(InvertedFile64)}::{nameof(Dispose)}");

            // TODO implement properly

            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            // ReSharper disable AssignNullToNotNullAttribute

            if (!ReferenceEquals(Ifp, null))
            {
                Ifp.Dispose();
                Ifp = null;
            }

            if (!ReferenceEquals(L01, null))
            {
                L01.Dispose();
                L01 = null;
            }

            if (!ReferenceEquals(N01, null))
            {
                N01.Dispose();
                N01 = null;
            }

            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ConditionIsAlwaysTrueOrFalse
        }

        #endregion

    } // class InvertedFile64

} // namespace ManagedIrbis.Direct

