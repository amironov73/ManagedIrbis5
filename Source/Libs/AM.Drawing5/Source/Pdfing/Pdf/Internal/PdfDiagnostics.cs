// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/*
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace PdfSharpCore.Pdf.Internal;

internal class PdfDiagnostics
{
    public static bool TraceCompressedObjects { get; set; } = true;

    public static bool TraceXrefStreams
    {
        get => _traceXrefStreams && TraceCompressedObjects;
        set => _traceXrefStreams = value;
    }
    private static bool _traceXrefStreams = true;

    public static bool TraceObjectStreams
    {
        get => _traceObjectStreams && TraceCompressedObjects;
        set => _traceObjectStreams = value;
    }
    private static bool _traceObjectStreams = true;
}
