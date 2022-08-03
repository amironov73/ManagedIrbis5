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

using ManagedIrbis.Epub.Internal;

#endregion

#nullable enable

namespace ManagedIrbis.Epub.Schema;

public class EpubPackage
{
    public EpubVersion EpubVersion { get; set; }
    public EpubMetadata Metadata { get; set; }
    public EpubManifest Manifest { get; set; }
    public EpubSpine Spine { get; set; }
    public EpubGuide Guide { get; set; }

    public string GetVersionString()
    {
        return VersionUtils.GetVersionString(EpubVersion);
    }
}