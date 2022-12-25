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
// ReSharper disable UnusedParameter.Local
// ReSharper disable VirtualMemberCallInConstructor

/* EmptyRazorProjectFileSystem.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

using Microsoft.AspNetCore.Razor.Language;

#endregion

#nullable enable

namespace MiniRazor;

[ExcludeFromCodeCoverage]
internal class EmptyRazorProjectFileSystem
    : RazorProjectFileSystem
{
    public static EmptyRazorProjectFileSystem Instance { get; } = new();

    public override IEnumerable<RazorProjectItem> EnumerateItems(string basePath) =>
        Enumerable.Empty<RazorProjectItem>();

    public override RazorProjectItem GetItem(string path) =>
        GetItem(path, null);

    public override RazorProjectItem GetItem(string path, string? fileKind) =>
        new NotFoundProjectItem(string.Empty, path, fileKind);


    [ExcludeFromCodeCoverage]
    private class NotFoundProjectItem
        : RazorProjectItem
    {
        public override string BasePath { get; }

        public override string FilePath { get; }

        public override string FileKind { get; }

        public override bool Exists => false;

        public override string PhysicalPath => throw new NotSupportedException();

        public NotFoundProjectItem(string basePath, string path, string? fileKind)
        {
            BasePath = basePath;
            FilePath = path;
            FileKind = fileKind ?? FileKinds.GetFileKindFromFilePath(path);
        }

        public override Stream Read() => throw new NotSupportedException();
    }
}
