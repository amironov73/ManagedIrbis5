// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* QRCodeData.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

#endregion

#nullable enable

namespace AM.Skia.QrCoding;

/// <summary>
///
/// </summary>
public class QRCodeData
    : IDisposable
{
    #region Properties

    /// <summary>
    ///
    /// </summary>
    public int Version { get; private set; }

    /// <summary>
    ///
    /// </summary>
    public List<BitArray> ModuleMatrix { get; set; }

    #endregion

    #region Construction

    /// <summary>
    ///
    /// </summary>
    /// <param name="version"></param>
    public QRCodeData
        (
            int version
        )
    {
        Version = version;
        var size = ModulesPerSideFromVersion (version);
        ModuleMatrix = new List<BitArray>();
        for (var i = 0; i < size; i++)
        {
            ModuleMatrix.Add (new BitArray (size));
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="rawData"></param>
    /// <param name="compressMode"></param>
    /// <exception cref="Exception"></exception>
    public QRCodeData
        (
            IEnumerable<byte> rawData,
            Compression compressMode
        )
    {
        var bytes = new List<byte> (rawData);

        //Decompress
        if (compressMode.Equals (Compression.Deflate))
        {
            using (var input = new MemoryStream (bytes.ToArray()))
            using (var output = new MemoryStream())
            {
                using (var dstream = new DeflateStream (input, CompressionMode.Decompress))
                {
                    dstream.CopyTo (output);
                }

                bytes = new List<byte> (output.ToArray());
            }
        }
        else if (compressMode.Equals (Compression.GZip))
        {
            using (var input = new MemoryStream (bytes.ToArray()))
            using (var output = new MemoryStream())
            {
                using (var dstream = new GZipStream (input, CompressionMode.Decompress))
                {
                    dstream.CopyTo (output);
                }

                bytes = new List<byte> (output.ToArray());
            }
        }

        if (bytes[0] != 0x51 || bytes[1] != 0x52 || bytes[2] != 0x52)
        {
            throw new Exception ("Invalid raw data file. Filetype doesn't match \"QRR\".");
        }

        //Set QR code version
        var sideLen = (int)bytes[4];
        bytes.RemoveRange (0, 5);
        Version = (sideLen - 21 - 8) / 4 + 1;

        //Unpack
        var modules = new Queue<bool>();
        foreach (var b in bytes)
        {
            var bArr = new BitArray (new byte[] { b });
            for (int i = 7; i >= 0; i--)
            {
                modules.Enqueue ((b & (1 << i)) != 0);
            }
        }

        //Build module matrix
        ModuleMatrix = new List<BitArray>();
        for (int y = 0; y < sideLen; y++)
        {
            ModuleMatrix.Add (new BitArray (sideLen));
            for (int x = 0; x < sideLen; x++)
            {
                ModuleMatrix[y][x] = modules.Dequeue();
            }
        }
    }

    #endregion

    #region Public methods

    /// <summary>
    ///
    /// </summary>
    /// <param name="compressMode"></param>
    /// <returns></returns>
    public byte[] GetRawData (Compression compressMode)
    {
        var bytes = new List<byte>();

        //Add header - signature ("QRR")
        bytes.AddRange (new byte[] { 0x51, 0x52, 0x52, 0x00 });

        //Add header - rowsize
        bytes.Add ((byte)ModuleMatrix.Count);

        //Build data queue
        var dataQueue = new Queue<int>();
        foreach (var row in ModuleMatrix)
        {
            foreach (var module in row)
            {
                dataQueue.Enqueue ((bool)module ? 1 : 0);
            }
        }

        for (int i = 0; i < 8 - (ModuleMatrix.Count * ModuleMatrix.Count) % 8; i++)
        {
            dataQueue.Enqueue (0);
        }

        //Process queue
        while (dataQueue.Count > 0)
        {
            byte b = 0;
            for (int i = 7; i >= 0; i--)
            {
                b += (byte)(dataQueue.Dequeue() << i);
            }

            bytes.Add (b);
        }

        var rawData = bytes.ToArray();

        //Compress stream (optional)
        if (compressMode.Equals (Compression.Deflate))
        {
            using (var output = new MemoryStream())
            using (var dstream = new DeflateStream (output, CompressionMode.Compress))
            {
                dstream.Write (rawData, 0, rawData.Length);
                rawData = output.ToArray();
            }
        }
        else if (compressMode.Equals (Compression.GZip))
        {
            using (var output = new MemoryStream())
            using (GZipStream gzipStream = new GZipStream (output, CompressionMode.Compress, true))
            {
                gzipStream.Write (rawData, 0, rawData.Length);
                rawData = output.ToArray();
            }
        }

        return rawData;
    }

    private static int ModulesPerSideFromVersion (int version)
    {
        return 21 + (version - 1) * 4;
    }

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose()
    {
        ModuleMatrix = null!;
        Version = 0;
    }

    #endregion

    /// <summary>
    ///
    /// </summary>
    public enum Compression
    {
        /// <summary>
        ///
        /// </summary>
        Uncompressed,

        /// <summary>
        ///
        /// </summary>
        Deflate,

        /// <summary>
        ///
        /// </summary>
        GZip
    }
}
