// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* ShellWrapperDefinitions.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Runtime.InteropServices;

#endregion

#nullable enable

namespace AM.Windows.Forms.Dialogs.Interop;

// Dummy base interface for CommonFileDialog coclasses
internal interface NativeCommonFileDialog
{
    // пустое тело
}

// ---------------------------------------------------------
// Coclass interfaces - designed to "look like" the object
// in the API, so that the 'new' operator can be used in a
// straightforward way. Behind the scenes, the C# compiler
// morphs all 'new CoClass()' calls to 'new CoClassWrapper()'
[ComImport]
[Guid (IIDGuid.IFileOpenDialog)]
[CoClass (typeof (FileOpenDialogRCW))]
internal interface NativeFileOpenDialog
    : IFileOpenDialog
{
    // пустое тело
}

[ComImport,
 Guid (IIDGuid.IFileSaveDialog),
 CoClass (typeof (FileSaveDialogRCW))]
internal interface NativeFileSaveDialog
    : IFileSaveDialog
{
    // пустое тело
}

[ComImport]
[Guid (IIDGuid.IKnownFolderManager)]
[CoClass (typeof (KnownFolderManagerRCW))]
internal interface KnownFolderManager
    : IKnownFolderManager
{
    // пустое тело
}

// ---------------------------------------------------
// .NET classes representing runtime callable wrappers
[ComImport]
[ClassInterface (ClassInterfaceType.None)]
[TypeLibType (TypeLibTypeFlags.FCanCreate)]
[Guid (CLSIDGuid.FileOpenDialog)]
internal class FileOpenDialogRCW
{
    // пустое тело
}

[ComImport]
[ClassInterface (ClassInterfaceType.None)]
[TypeLibType (TypeLibTypeFlags.FCanCreate)]
[Guid (CLSIDGuid.FileSaveDialog)]
internal class FileSaveDialogRCW
{
    // пустое тело
}

[ComImport]
[ClassInterface (ClassInterfaceType.None)]
[TypeLibType (TypeLibTypeFlags.FCanCreate)]
[Guid (CLSIDGuid.KnownFolderManager)]
internal class KnownFolderManagerRCW
{
    // пустое тело
}
