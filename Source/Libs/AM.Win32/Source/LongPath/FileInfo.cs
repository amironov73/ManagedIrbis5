// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Security.AccessControl;
using System.Text;

using FileAccess = System.IO.FileAccess;
using FileMode = System.IO.FileMode;
using FileStream = System.IO.FileStream;
using StreamWriter = System.IO.StreamWriter;
using FileShare = System.IO.FileShare;
using FileOptions = System.IO.FileOptions;
using StreamReader = System.IO.StreamReader;

#endregion

#nullable enable

namespace AM.Win32.LongPath;

/// <summary>
/// 
/// </summary>
public class FileInfo
    : FileSystemInfo
{
    private readonly string _name;

    /// <summary>
    /// 
    /// </summary>
    public DirectoryInfo Directory
    {
        get
        {
            var dirName = DirectoryName;
            return dirName == null ? null : new DirectoryInfo (dirName);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public System.IO.FileInfo SysFileInfo
    {
        get { return new System.IO.FileInfo (FullPath); }
    }

    public override System.IO.FileSystemInfo SystemInfo
    {
        get { return SysFileInfo; }
    }

    public string DirectoryName
    {
        get { return Path.GetDirectoryName (FullPath); }
    }

    public override bool Exists
    {
        get
        {
            if (Common.IsRunningOnMono() && Common.IsPlatformUnix())
            {
                return SysFileInfo.Exists;
            }

            if (state == State.Uninitialized)
            {
                Refresh();
            }

            return state == State.Initialized &&
                   (data.fileAttributes & System.IO.FileAttributes.Directory) != System.IO.FileAttributes.Directory;
        }
    }

    public long Length
    {
        get { return GetFileLength(); }
    }

    public override string Name
    {
        get { return _name; }
    }

    public FileInfo (string fileName)
    {
        OriginalPath = fileName;
        FullPath = Path.GetFullPath (fileName);
        _name = Path.GetFileName (fileName);
        DisplayPath = GetDisplayPath (fileName);
    }

    private string GetDisplayPath (string originalPath)
    {
        return originalPath;
    }

    /// <include path='doc/members/member[@name="M:System.IO.FileInfo.GetFileLength"]/*' file='..\ref\mscorlib.xml' />
    private long GetFileLength()
    {
        if (Common.IsRunningOnMono() && Common.IsPlatformUnix())
        {
            return SysFileInfo.Length;
        }

        if (state == State.Uninitialized)
        {
            Refresh();
        }

        if (state == State.Error)
        {
            Common.ThrowIOError (errorCode, FullPath);
        }

        return ((long)data.fileSizeHigh) << 32 | (data.fileSizeLow & 0xFFFFFFFFL);
    }

    /// <include path='doc/members/member[@name="M:System.IO.FileInfo.AppendText"]/*' file='..\ref\mscorlib.xml' />
    public StreamWriter AppendText()
    {
        return File.CreateStreamWriter (FullPath, true);
    }

    /// <include path='doc/members/member[@name="M:System.IO.FileInfo.CopyTo(System.String)"]/*' file='..\ref\mscorlib.xml' />
    public FileInfo CopyTo (string destFileName)
    {
        return CopyTo (destFileName, false);
    }

    /// <include path='doc/members/member[@name="M:System.IO.FileInfo.CopyTo(System.String,System.Boolean)"]/*' file='..\ref\mscorlib.xml' />
    public FileInfo CopyTo (string destFileName, bool overwrite)
    {
        File.Copy (FullPath, destFileName, overwrite);
        return new FileInfo (destFileName);
    }

    /// <include path='doc/members/member[@name="M:System.IO.FileInfo.Create"]/*' file='..\ref\mscorlib.xml' />
    public FileStream Create()
    {
        return File.Create (FullPath);
    }

    /// <include path='doc/members/member[@name="M:System.IO.FileInfo.CreateText"]/*' file='..\ref\mscorlib.xml' />
    public StreamWriter CreateText()
    {
        return File.CreateStreamWriter (FullPath, false);
    }

    /// <include path='doc/members/member[@name="M:System.IO.FileInfo.Delete"]/*' file='..\ref\mscorlib.xml' />
    public override void Delete()
    {
        File.Delete (FullPath);
    }

    /// <include path='doc/members/member[@name="M:System.IO.FileInfo.MoveTo(System.String)"]/*' file='..\ref\mscorlib.xml' />
    public void MoveTo (string destFileName)
    {
        File.Move (FullPath, destFileName);
    }

    /// <include path='doc/members/member[@name="M:System.IO.FileInfo.Open(System.IO.FileMode)"]/*' file='..\ref\mscorlib.xml' />
    public FileStream Open (FileMode mode)
    {
        return Open (mode, FileAccess.ReadWrite, FileShare.None);
    }

    /// <include path='doc/members/member[@name="M:System.IO.FileInfo.Open(System.IO.FileMode,System.IO.FileAccess)"]/*' file='..\ref\mscorlib.xml' />
    public FileStream Open (FileMode mode, FileAccess access)
    {
        return Open (mode, access, FileShare.None);
    }

    /// <include path='doc/members/member[@name="M:System.IO.FileInfo.Open(System.IO.FileMode,System.IO.FileAccess,System.IO.FileShare)"]/*' file='..\ref\mscorlib.xml' />
    public FileStream Open (FileMode mode, FileAccess access, FileShare share)
    {
        if (Common.IsRunningOnMono() && Common.IsPlatformUnix())
        {
            return SysFileInfo.Open (mode, access, share);
        }

        return File.Open (FullPath, mode, access, share, 4096, FileOptions.SequentialScan);
    }

    public FileStream OpenRead()
    {
        if (Common.IsRunningOnMono() && Common.IsPlatformUnix())
        {
            return SysFileInfo.OpenRead();
        }

        return File.Open (FullPath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, FileOptions.None);
    }

    /// <include path='doc/members/member[@name="M:System.IO.FileInfo.OpenText"]/*' file='..\ref\mscorlib.xml' />
    public StreamReader OpenText()
    {
        return File.CreateStreamReader (FullPath, Encoding.UTF8, true, 1024);
    }

    /// <include path='doc/members/member[@name="M:System.IO.FileInfo.OpenWrite"]/*' file='..\ref\mscorlib.xml' />
    public FileStream OpenWrite()
    {
        return File.Open (FullPath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
    }

    /// <include path='doc/members/member[@name="M:System.IO.FileInfo.ToString"]/*' file='..\ref\mscorlib.xml' />
    public override string ToString()
    {
        return DisplayPath;
    }

    /// <include path='doc/members/member[@name="M:System.IO.FileInfo.Encrypt"]/*' file='..\ref\mscorlib.xml' />
    public void Encrypt()
    {
        File.Encrypt (FullPath);
    }

    /// <include path='doc/members/member[@name="M:System.IO.FileInfo.Decrypt"]/*' file='..\ref\mscorlib.xml' />
    public void Decrypt()
    {
        File.Decrypt (FullPath);
    }

    /// <include path='doc/members/member[@name="P:System.IO.FileInfo.IsReadOnly"]/*' file='..\ref\mscorlib.xml' />
    public bool IsReadOnly
    {
        get
        {
            if (Common.IsRunningOnMono() && Common.IsPlatformUnix())
            {
                return SysFileInfo.IsReadOnly;
            }

            return (Attributes & System.IO.FileAttributes.ReadOnly) != 0;
        }
        set
        {
            if (Common.IsRunningOnMono() && Common.IsPlatformUnix())
            {
                SysFileInfo.IsReadOnly = value;
                return;
            }

            if (value)
            {
                Attributes |= System.IO.FileAttributes.ReadOnly;
                return;
            }

            Attributes &= ~System.IO.FileAttributes.ReadOnly;
        }
    }

    public FileInfo Replace (string destinationFilename, string backupFilename)
    {
        return Replace (destinationFilename, backupFilename, false);
    }

    public FileInfo Replace (string destinationFilename, string backupFilename, bool ignoreMetadataErrors)
    {
        File.Replace (FullPath, destinationFilename, backupFilename, ignoreMetadataErrors);
        return new FileInfo (destinationFilename);
    }

    public FileSecurity GetAccessControl()
    {
        return File.GetAccessControl (FullPath);
    }

    public FileSecurity GetAccessControl (AccessControlSections includeSections)
    {
        return File.GetAccessControl (FullPath, includeSections);
    }

    public void SetAccessControl (FileSecurity security)
    {
        File.SetAccessControl (FullPath, security);
    }
}
