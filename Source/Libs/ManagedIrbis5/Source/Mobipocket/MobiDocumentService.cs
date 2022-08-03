// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* MobiDocumentService.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;

using ManagedIrbis.Mobipocket.Headers;

#endregion

#nullable enable

namespace ManagedIrbis.Mobipocket;

/// <summary>
///
/// </summary>
public sealed class MobiDocumentService
{
    /// <summary>
    ///
    /// </summary>
    public MobiDocument LoadDocument (string filePath)
    {
        var document = new MobiDocument (filePath);

        using var fs = new FileStream (filePath, FileMode.Open, FileAccess.Read);
        document.PdbHeader = new PdbHeader (fs);
        document.MobiHeader = new MobiHeader (fs, document.PdbHeader.MobiHeaderSize);

        return document;
    }

    /// <summary>
    ///
    /// </summary>
    public void SaveDocument (MobiDocument document, string saveFilePath)
    {
        using var fs = File.OpenWrite (saveFilePath);

        document.Write (fs, saveFilePath);
    }
}
