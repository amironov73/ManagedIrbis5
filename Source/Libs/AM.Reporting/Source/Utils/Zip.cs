// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

#endregion

#nullable enable

namespace AM.Reporting.Utils
{
    /// <summary>
    ///
    /// </summary>
    public class ZipArchive
    {
        List<ZipFileItem> fileList;
        List<ZipLocalFile> fileObjects;

        private uint CopyStream (Stream source, Stream target)
        {
            source.Position = 0;
            var bufflength = 8192;
            var crc = Crc32.Begin();
            var buff = new byte[bufflength];
            int i;
            while ((i = source.Read (buff, 0, bufflength)) > 0)
            {
                target.Write (buff, 0, i);
                crc = Crc32.Update (crc, buff, 0, i);
            }

            return Crc32.End (crc);
        }

        /// <summary>
        /// Clear all files in archive.
        /// </summary>
        public void Clear()
        {
            foreach (var item in fileList)
            {
                item.Clear();
            }

            foreach (var item in fileObjects)
            {
                item.Clear();
            }

            fileList.Clear();
            fileObjects.Clear();
            Errors = "";
            RootFolder = "";
            Comment = "";
        }

        /// <summary>
        /// Check for exisiting file in archive.
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        public bool FileExists (string FileName)
        {
            foreach (var item in fileList)
            {
                if (item.Name == FileName)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Adds the file form disk to the archive.
        /// </summary>
        /// <param name="FileName"></param>
        public void AddFile (string FileName)
        {
            if (!FileExists (FileName)) // check for exisiting file in archive
            {
                if (File.Exists (FileName))
                {
                    fileList.Add (new ZipFileItem (FileName));
                    if (RootFolder == string.Empty)
                    {
                        RootFolder = Path.GetDirectoryName (FileName);
                    }
                }
                else
                {
                    Errors += "File " + FileName + " not found\r";
                }
            }
        }

        /// <summary>
        /// Adds all files from directory (recursive) on the disk to the archive.
        /// </summary>
        /// <param name="DirName"></param>
        public void AddDir (string DirName)
        {
            List<string> files = new List<string>();
            files.AddRange (Directory.GetFiles (DirName));
            foreach (var file in files)
            {
                if ((File.GetAttributes (file) & FileAttributes.Hidden) != 0)
                {
                    continue;
                }

                AddFile (file);
            }

            List<string> folders = new List<string>();
            folders.AddRange (Directory.GetDirectories (DirName));
            foreach (var folder in folders)
            {
                if ((File.GetAttributes (folder) & FileAttributes.Hidden) != 0)
                {
                    continue;
                }

                AddDir (folder);
            }
        }

        /// <summary>
        /// Adds the stream to the archive.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="stream"></param>
        public void AddStream (string fileName, Stream stream)
        {
            if (!FileExists (fileName)) // check for exisiting file in archive
            {
                fileList.Add (new ZipFileItem (fileName, stream));
            }
        }

        private void AddStreamToZip (Stream stream, ZipLocalFile ZipFile)
        {
            if (stream.Length > 128)
            {
                using (var deflate = new DeflateStream (ZipFile.FileData, CompressionMode.Compress, true))
                    ZipFile.LocalFileHeader.Crc32 = CopyStream (stream, deflate);
                ZipFile.LocalFileHeader.CompressionMethod = 8;
            }
            else
            {
                ZipFile.LocalFileHeader.Crc32 = CopyStream (stream, ZipFile.FileData);
                ZipFile.LocalFileHeader.CompressionMethod = 0;
            }

            ZipFile.LocalFileHeader.CompressedSize = (uint)ZipFile.FileData.Length;
            ZipFile.LocalFileHeader.UnCompressedSize = (uint)stream.Length;
        }

        /// <summary>
        /// Creates the zip and writes it to rhe Stream
        /// </summary>
        /// <param name="Stream"></param>
        public void SaveToStream (Stream Stream)
        {
            ZipLocalFile ZipFile;
            ZipCentralDirectory ZipDir;
            ZipFileHeader ZipFileHeader;
            long CentralStartPos, CentralEndPos;

            for (var i = 0; i < fileList.Count; i++)
            {
                ZipFile = new ZipLocalFile();
                using (ZipFile.FileData = new MemoryStream())
                {
                    if (fileList[i].Disk)
                    {
                        ZipFile.LocalFileHeader.FileName =
                            fileList[i].Name.Replace (RootFolder + Path.DirectorySeparatorChar, "");
                        using (var file = new FileStream (fileList[i].Name, FileMode.Open))
                            AddStreamToZip (file, ZipFile);
                    }
                    else
                    {
                        ZipFile.LocalFileHeader.FileName = fileList[i].Name;
                        fileList[i].Stream.Position = 0;
                        AddStreamToZip (fileList[i].Stream, ZipFile);
                    }

                    ZipFile.Offset = (uint)Stream.Position;
                    ZipFile.LocalFileHeader.LastModFileDate = fileList[i].FileDateTime;
                    ZipFile.SaveToStream (Stream);
                }

                ZipFile.FileData = null;
                fileObjects.Add (ZipFile);
            }

            CentralStartPos = Stream.Position;
            for (var i = 0; i < fileObjects.Count; i++)
            {
                ZipFile = fileObjects[i];
                ZipFileHeader = new ZipFileHeader
                {
                    CompressionMethod = ZipFile.LocalFileHeader.CompressionMethod,
                    LastModFileDate = ZipFile.LocalFileHeader.LastModFileDate,
                    GeneralPurpose = ZipFile.LocalFileHeader.GeneralPurpose,
                    Crc32 = ZipFile.LocalFileHeader.Crc32,
                    CompressedSize = ZipFile.LocalFileHeader.CompressedSize,
                    UnCompressedSize = ZipFile.LocalFileHeader.UnCompressedSize,
                    RelativeOffsetLocalHeader = ZipFile.Offset,
                    FileName = ZipFile.LocalFileHeader.FileName
                };
                ZipFileHeader.SaveToStream (Stream);
            }

            CentralEndPos = Stream.Position;
            ZipDir = new ZipCentralDirectory
            {
                TotalOfEntriesCentralDirOnDisk = (ushort)fileList.Count,
                TotalOfEntriesCentralDir = (ushort)fileList.Count,
                SizeOfCentralDir = (uint)(CentralEndPos - CentralStartPos),
                OffsetStartingDiskDir = (uint)CentralStartPos
            };
            ZipDir.SaveToStream (Stream);
        }

        /// <summary>
        /// Creates the ZIP archive and writes it to the file.
        /// </summary>
        /// <param name="FileName"></param>
        public void SaveToFile (string FileName)
        {
            using (var file = new FileStream (FileName, FileMode.Create))
                SaveToStream (file);
        }

        /// <summary>
        /// Gets or sets the Root Folder.
        /// </summary>
        public string RootFolder { get; set; }

        /// <summary>
        /// Gets or sets the errors.
        /// </summary>
        public string Errors { get; set; }

        /// <summary>
        /// Gets or sets the commentary to the archive.
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Gets count of files in archive.
        /// </summary>
        public int FileCount => fileList.Count;

        /// <summary>
        /// Creates the new zip archive.
        /// </summary>
        public ZipArchive()
        {
            fileList = new List<ZipFileItem>();
            fileObjects = new List<ZipLocalFile>();
            Clear();
        }
    }

    internal class ZipFileItem
    {
        private uint GetDosDateTime (DateTime date)
        {
            return (uint)(
                ((date.Year - 1980 & 0x7f) << 25) |
                ((date.Month & 0xF) << 21) |
                ((date.Day & 0x1F) << 16) |
                ((date.Hour & 0x1F) << 11) |
                ((date.Minute & 0x3F) << 5) |
                (date.Second >> 1));
        }

        public string Name { get; set; }

        public Stream Stream { get; private set; }

        public bool Disk { get; set; }

        public uint FileDateTime { get; set; }

        public void Clear()
        {
            if (Stream != null)
            {
                Stream.Dispose();
                Stream = null;
            }
        }

        public ZipFileItem()
        {
            Stream = new MemoryStream();
            FileDateTime = GetDosDateTime (SystemFake.DateTime.Now);
            Disk = false;
        }

        public ZipFileItem (string fileName, Stream stream)
        {
            this.Stream = stream;
            Name = fileName;
            FileDateTime = GetDosDateTime (SystemFake.DateTime.Now);
            Disk = false;
        }

        public ZipFileItem (string fileName)
        {
            Name = fileName;
            FileDateTime = GetDosDateTime (File.GetLastWriteTime (fileName));
            Disk = true;
        }
    }

    internal class ZipLocalFileHeader
    {
        private string extraField;
        private string fileName;

        public void SaveToStream (Stream Stream)
        {
            Stream.Write (BitConverter.GetBytes (LocalFileHeaderSignature), 0, 4);
            Stream.Write (BitConverter.GetBytes (Version), 0, 2);
            Stream.Write (BitConverter.GetBytes (GeneralPurpose), 0, 2);
            Stream.Write (BitConverter.GetBytes (CompressionMethod), 0, 2);
            Stream.Write (BitConverter.GetBytes (LastModFileDate), 0, 4);
            Stream.Write (BitConverter.GetBytes (Crc32), 0, 4);
            Stream.Write (BitConverter.GetBytes (CompressedSize), 0, 4);
            Stream.Write (BitConverter.GetBytes (UnCompressedSize), 0, 4);
            Stream.Write (BitConverter.GetBytes (FileNameLength), 0, 2);
            Stream.Write (BitConverter.GetBytes (ExtraFieldLength), 0, 2);
            if (FileNameLength > 0)
            {
                Stream.Write (System.Text.Encoding.UTF8.GetBytes (fileName), 0, FileNameLength);
            }

            if (ExtraFieldLength > 0)
            {
                Stream.Write (Converter.StringToByteArray (extraField), 0, ExtraFieldLength);
            }
        }

        public uint LocalFileHeaderSignature { get; }

        public ushort Version { get; set; }

        public ushort GeneralPurpose { get; set; }

        public ushort CompressionMethod { get; set; }

        public uint LastModFileDate { get; set; }

        public uint Crc32 { get; set; }

        public uint CompressedSize { get; set; }

        public uint UnCompressedSize { get; set; }

        public ushort FileNameLength { get; set; }

        public ushort ExtraFieldLength { get; set; }

        public string FileName
        {
            get => fileName;
            set
            {
                fileName = value.Replace ('\\', '/');
                FileNameLength = (ushort)System.Text.Encoding.UTF8.GetBytes (value).Length;
            }
        }

        public string ExtraField
        {
            get => extraField;
            set
            {
                extraField = value;
                ExtraFieldLength = (ushort)value.Length;
            }
        }

        // constructor
        public ZipLocalFileHeader()
        {
            LocalFileHeaderSignature = 0x04034b50;
            Version = 20;
            GeneralPurpose = 0x800;
            CompressionMethod = 0;
            Crc32 = 0;
            LastModFileDate = 0;
            CompressedSize = 0;
            UnCompressedSize = 0;
            extraField = "";
            fileName = "";
            FileNameLength = 0;
            ExtraFieldLength = 0;
        }
    }

    internal class ZipCentralDirectory
    {
        private string comment;

        public void SaveToStream (Stream Stream)
        {
            Stream.Write (BitConverter.GetBytes (EndOfChentralDirSignature), 0, 4);
            Stream.Write (BitConverter.GetBytes (NumberOfTheDisk), 0, 2);
            Stream.Write (BitConverter.GetBytes (NumberOfTheDiskStartCentralDir), 0, 2);
            Stream.Write (BitConverter.GetBytes (TotalOfEntriesCentralDirOnDisk), 0, 2);
            Stream.Write (BitConverter.GetBytes (TotalOfEntriesCentralDir), 0, 2);
            Stream.Write (BitConverter.GetBytes (SizeOfCentralDir), 0, 4);
            Stream.Write (BitConverter.GetBytes (OffsetStartingDiskDir), 0, 4);
            Stream.Write (BitConverter.GetBytes (CommentLength), 0, 2);
            if (CommentLength > 0)
            {
                Stream.Write (Converter.StringToByteArray (comment), 0, CommentLength);
            }
        }

        public uint EndOfChentralDirSignature { get; }

        public ushort NumberOfTheDisk { get; set; }

        public ushort NumberOfTheDiskStartCentralDir { get; set; }

        public ushort TotalOfEntriesCentralDirOnDisk { get; set; }

        public ushort TotalOfEntriesCentralDir { get; set; }

        public uint SizeOfCentralDir { get; set; }

        public uint OffsetStartingDiskDir { get; set; }

        public ushort CommentLength { get; set; }

        public string Comment
        {
            get => comment;
            set
            {
                comment = value;
                CommentLength = (ushort)value.Length;
            }
        }

        // constructor
        public ZipCentralDirectory()
        {
            EndOfChentralDirSignature = 0x06054b50;
            NumberOfTheDisk = 0;
            NumberOfTheDiskStartCentralDir = 0;
            TotalOfEntriesCentralDirOnDisk = 0;
            TotalOfEntriesCentralDir = 0;
            SizeOfCentralDir = 0;
            OffsetStartingDiskDir = 0;
            CommentLength = 0;
            comment = "";
        }
    }

    internal class ZipFileHeader
    {
        private string extraField;
        private string fileComment;
        private string fileName;

        public void SaveToStream (Stream Stream)
        {
            Stream.Write (BitConverter.GetBytes (CentralFileHeaderSignature), 0, 4);
            Stream.Write (BitConverter.GetBytes (VersionMadeBy), 0, 2);
            Stream.Write (BitConverter.GetBytes (VersionNeeded), 0, 2);
            Stream.Write (BitConverter.GetBytes (GeneralPurpose), 0, 2);
            Stream.Write (BitConverter.GetBytes (CompressionMethod), 0, 2);
            Stream.Write (BitConverter.GetBytes (LastModFileDate), 0, 4);
            Stream.Write (BitConverter.GetBytes (Crc32), 0, 4);
            Stream.Write (BitConverter.GetBytes (CompressedSize), 0, 4);
            Stream.Write (BitConverter.GetBytes (UnCompressedSize), 0, 4);
            Stream.Write (BitConverter.GetBytes (FileNameLength), 0, 2);
            Stream.Write (BitConverter.GetBytes (ExtraFieldLength), 0, 2);
            Stream.Write (BitConverter.GetBytes (FileCommentLength), 0, 2);
            Stream.Write (BitConverter.GetBytes (DiskNumberStart), 0, 2);
            Stream.Write (BitConverter.GetBytes (InternalFileAttribute), 0, 2);
            Stream.Write (BitConverter.GetBytes (ExternalFileAttribute), 0, 4);
            Stream.Write (BitConverter.GetBytes (RelativeOffsetLocalHeader), 0, 4);
            Stream.Write (System.Text.Encoding.UTF8.GetBytes (fileName), 0, FileNameLength);
            Stream.Write (Converter.StringToByteArray (extraField), 0, ExtraFieldLength);
            Stream.Write (Converter.StringToByteArray (fileComment), 0, FileCommentLength);
        }

        public uint CentralFileHeaderSignature { get; }

        public ushort VersionMadeBy { get; }

        public ushort VersionNeeded { get; }

        public ushort GeneralPurpose { get; set; }

        public ushort CompressionMethod { get; set; }

        public uint LastModFileDate { get; set; }

        public uint Crc32 { get; set; }

        public uint CompressedSize { get; set; }

        public uint UnCompressedSize { get; set; }

        public ushort FileNameLength { get; set; }

        public ushort ExtraFieldLength { get; set; }

        public ushort FileCommentLength { get; set; }

        public ushort DiskNumberStart { get; set; }

        public ushort InternalFileAttribute { get; set; }

        public uint ExternalFileAttribute { get; set; }

        public uint RelativeOffsetLocalHeader { get; set; }

        public string FileName
        {
            get => fileName;
            set
            {
                fileName = value.Replace ('\\', '/');
                FileNameLength = (ushort)System.Text.Encoding.UTF8.GetBytes (value).Length;
            }
        }

        public string ExtraField
        {
            get => extraField;
            set
            {
                extraField = value;
                ExtraFieldLength = (ushort)value.Length;
            }
        }

        public string FileComment
        {
            get => fileComment;
            set
            {
                fileComment = value;
                FileCommentLength = (ushort)value.Length;
            }
        }

        // constructor
        public ZipFileHeader()
        {
            CentralFileHeaderSignature = 0x02014b50;
            RelativeOffsetLocalHeader = 0;
            UnCompressedSize = 0;
            CompressedSize = 0;
            Crc32 = 0;
            ExternalFileAttribute = 0;
            extraField = "";
            fileComment = "";
            fileName = "";
            CompressionMethod = 0;
            DiskNumberStart = 0;
            LastModFileDate = 0;
            VersionMadeBy = 20;
            GeneralPurpose = 0x800;
            FileNameLength = 0;
            InternalFileAttribute = 0;
            ExtraFieldLength = 0;
            VersionNeeded = 20;
            FileCommentLength = 0;
        }
    }

    internal class ZipLocalFile
    {
        public void SaveToStream (Stream Stream)
        {
            LocalFileHeader.SaveToStream (Stream);
            FileData.Position = 0;
            FileData.WriteTo (Stream);
            FileData.Dispose();
            FileData = null;
        }

        public ZipLocalFileHeader LocalFileHeader { get; }

        public MemoryStream FileData { get; set; }

        public uint Offset { get; set; }

        public void Clear()
        {
            if (FileData != null)
            {
                FileData.Dispose();
                FileData = null;
            }
        }

        // constructor
        public ZipLocalFile()
        {
            LocalFileHeader = new ZipLocalFileHeader();
            Offset = 0;
        }
    }
}
