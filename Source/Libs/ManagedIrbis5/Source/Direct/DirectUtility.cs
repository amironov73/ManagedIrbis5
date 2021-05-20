// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* XrfFile64.cs -- XRF-файл
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Buffers.Binary;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;

using AM;
using AM.IO;

#endregion

#nullable enable

namespace ManagedIrbis.Direct
{
    /// <summary>
    /// Методы для прямого доступа к базам данных.
    /// </summary>
    public static class DirectUtility
    {
        #region Private members

        private static readonly byte[] _l01Content32 = { };
        private static readonly byte[] _l02Content32 = { };
        private static readonly byte[] _n01Content32 = { };
        private static readonly byte[] _n02Content32 = { };
        private static readonly byte[] _cntContent32 = { };
        private static readonly byte[] _ifpContent32 = { };
        private static readonly byte[] _mstContent32 = { };
        private static readonly byte[] _xrfContent32 = { };

        private static readonly byte[] _ifpContent64 = { };
        private static readonly byte[] _l01Content64 = { };
        private static readonly byte[] _n01Content64 = { };
        private static readonly byte[] _xrfContent64 = { };
        private static readonly byte[] _mstContent64 = { };

        #endregion

        #region Public methods

        /// <summary>
        /// Сборка пути к файлу из компонентов.
        /// </summary>
        public static string CombinePath
            (
                string first,
                string second
            )
        {
            second = second.ConvertSlashes();
            if (Path.IsPathRooted(second) || Path.IsPathFullyQualified(second))
            {
                return second;
            }

            return Path.Combine(first.ConvertSlashes(), second);
        }

        /// <summary>
        /// Сборка пути к файлу из компонентов.
        /// </summary>
        public static string CombinePath
            (
                string first,
                string second,
                string third
            )
        {
            third = third.ConvertSlashes();
            if (Path.IsPathRooted(third) || Path.IsPathFullyQualified(third))
            {
                return third;
            }

            second = second.ConvertSlashes();
            if (Path.IsPathRooted(second) || Path.IsPathFullyQualified(second))
            {
                return Path.Combine(second, third);
            }

            return Path.Combine(first.ConvertSlashes(), second, third);
        }

        /// <summary>
        /// Создание акцессора.
        /// </summary>
        public static DirectAccess64 CreateAccessor
            (
                DirectProvider provider,
                string? databaseName
            )
        {
            databaseName ??= provider.Database;
            if (string.IsNullOrEmpty(databaseName))
            {
                // TODO: выставлять код ошибки
                throw new IrbisException();
            }

            // TODO: сделать через PAR-файл
            var fileName = CombinePath
                (
                    provider.DataPath,
                    databaseName,
                    databaseName + ".mst"
                );

            if (!Unix.FileExists(fileName))
            {
                // TODO: выставлять код ошибки
                throw new IrbisException();
            }

            return new DirectAccess64(fileName, provider.Mode);

        } // method CreateAccessor

        /// <summary>
        /// Create 8 database files for IRBIS32.
        /// </summary>
        public static void CreateDatabase32
            (
                string path
            )
        {
            Sure.NotNullNorEmpty(path, nameof(path));

            var cntFile = path + ".cnt";
            FileUtility.DeleteIfExists(cntFile);
            File.WriteAllBytes(cntFile, _cntContent32);

            var ifpFile = path + ".ifp";
            FileUtility.DeleteIfExists(ifpFile);
            File.WriteAllBytes(ifpFile, _ifpContent32);

            var l01File = path + ".l01";
            FileUtility.DeleteIfExists(l01File);
            File.WriteAllBytes(l01File, _l01Content32);

            var l02File = path + ".l02";
            FileUtility.DeleteIfExists(l02File);
            File.WriteAllBytes(l02File, _l02Content32);

            var mstFile = path + ".mst";
            FileUtility.DeleteIfExists(mstFile);
            File.WriteAllBytes(mstFile, _mstContent32);

            var n01File = path + ".n01";
            FileUtility.DeleteIfExists(n01File);
            File.WriteAllBytes(n01File, _n01Content32);

            var n02File = path + ".n02";
            FileUtility.DeleteIfExists(n02File);
            File.WriteAllBytes(n02File, _n02Content32);

            var xrfFile = path + ".xrf";
            FileUtility.DeleteIfExists(xrfFile);
            File.WriteAllBytes(xrfFile, _xrfContent32);
        } // method CreateDatabase32

        /// <summary>
        /// Create 5 database files for IRBIS64.
        /// </summary>
        public static void CreateDatabase64
            (
                string path
            )
        {
            Sure.NotNullNorEmpty(path, nameof(path));

            var ifpFile = path + ".ifp";
            FileUtility.DeleteIfExists(ifpFile);
            File.WriteAllBytes(ifpFile, _ifpContent64);

            var l01File = path + ".l01";
            FileUtility.DeleteIfExists(l01File);
            File.WriteAllBytes(l01File, _l01Content64);

            var mstFile = path + ".mst";
            FileUtility.DeleteIfExists(mstFile);
            File.WriteAllBytes(mstFile, _mstContent64);

            var n01File = path + ".n01";
            FileUtility.DeleteIfExists(n01File);
            File.WriteAllBytes(n01File, _n01Content64);

            var xrfFile = path + ".xrf";
            FileUtility.DeleteIfExists(xrfFile);
            File.WriteAllBytes(xrfFile, _xrfContent64);
        } // method CreateDatabase64

        /// <summary>
        /// Исправляем 64-битное смещение, записанное в формате ИРБИС64.
        /// </summary>
        public static void FixNetwork64
            (
                ref long offset
            )
        {
            if (BitConverter.IsLittleEndian)
            {
                var span = MemoryMarshal.Cast<long, int>(MemoryMarshal.CreateSpan(ref offset, 1));
                span[0] = BinaryPrimitives.ReverseEndianness(span[0]);
                span[1] = BinaryPrimitives.ReverseEndianness(span[1]);
            }

        } // method FixNetwork64

        /// <summary>
        /// Open specified file.
        /// </summary>
        public static Stream OpenFile
            (
                string fileName,
                DirectAccessMode mode
            )
        {
            Sure.NotNullNorEmpty(fileName, nameof(fileName));

            Stream result;
            switch (mode)
            {
                case DirectAccessMode.Exclusive:
                    result = InsistentFile.OpenForExclusiveWrite(fileName);
                    break;

                case DirectAccessMode.Shared:
                    result = InsistentFile.OpenForSharedWrite(fileName);
                    break;

                case DirectAccessMode.ReadOnly:
                    result = InsistentFile.OpenForSharedRead(fileName);
                    break;

                default:
                    Magna.Error
                        (
                            nameof(DirectoryUtility) + "::" + nameof(OpenFile)
                            + "unexpected mode="
                            + mode
                        );

                    throw new IrbisException($"Unexpected mode={mode}");
            }

            result = new NonBufferedStream(result);

            return result;

        } // method OpenFile

        /// <summary>
        /// Чтение 32-битного целого в формате ИРБИС64.
        /// </summary>
        public static int ReadNetworkInt32
            (
                this MemoryMappedViewAccessor accessor,
                long position
            )
        {
            var result = accessor.ReadInt32(position);
            var buffer = BitConverter.GetBytes(result);
            StreamUtility.NetworkToHost32(buffer, 0);
            result = BitConverter.ToInt32(buffer, 0);

            return result;
        } // method ReadNetworkInt32

        /// <summary>
        /// Чтение 32-битного целого в формате ИРБИС64.
        /// </summary>
        public static unsafe int ReadNetworkInt32
            (
                this MemoryMappedViewStream stream
            )
        {
            var bytes = stackalloc byte[4];
            var buffer = new Span<byte> (bytes, 4);
            stream.Read(buffer);
            return BinaryPrimitives.ReadInt32BigEndian(buffer);
        } // method ReadNetworkInt32

        /// <summary>
        ///
        /// </summary>
        public static long ReadNetworkInt64
            (
                this MemoryMappedViewAccessor accessor,
                long position
            )
        {
            var result = accessor.ReadInt64(position);
            var buffer = BitConverter.GetBytes(result);
            StreamUtility.NetworkToHost64(buffer, 0);
            result = BitConverter.ToInt64(buffer, 0);

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        public static unsafe long ReadNetworkInt64
            (
                this MemoryMappedViewStream stream
            )
        {
            var bytes = stackalloc byte[8];
            var buffer = new Span<byte>(bytes, 8);
            stream.Read(buffer);
            StreamUtility.NetworkToHost64(buffer);
            var result = BitConverter.ToInt64(buffer);

            return result;
        } // method ReadNetworkInt64

        /// <summary>
        /// Open the <see cref="MemoryMappedFile"/> for read only.
        /// </summary>
        public static MemoryMappedFile OpenMemoryMappedFile
            (
                string fileName
            )
        {
            var stream = new FileStream
                (
                    fileName,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.Read
                );

            var result = MemoryMappedFile.CreateFromFile
                (
                    stream,
                    null, // map name = anonymous
                    stream.Length, // capacity = all the file
                    MemoryMappedFileAccess.Read,
                    HandleInheritability.None,
                    false // close the stream when the MMF is closed
                );

            return result;
        }

        #endregion

    } // class DirectUtility

} // namespace ManagedIrbis.Direct
