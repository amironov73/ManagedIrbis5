// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* MetafileExtensions.cs --
 * Ars Magna project, http://arsmagna.ru
 */

using System;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace AM.Windows.Forms.HtmlRenderer;

/// <summary>
///
/// </summary>
public static class MetafileExtensions
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="me"></param>
    /// <param name="fileName"></param>
    /// <exception cref="SystemException"></exception>
    public static void SaveAsEmf (Metafile me, string fileName)
    {
        /* http://social.msdn.microsoft.com/Forums/en-US/csharpgeneral/thread/12a1c749-b320-4ce9-aff7-9de0d7fd30ea
            How to save or serialize a Metafile: Solution found
            by : SWAT Team member _1
            Date : Friday, February 01, 2008 1:38 PM
            */
        var enfMetafileHandle = me.GetHenhmetafile().ToInt32();
        var bufferSize = GetEnhMetaFileBits (enfMetafileHandle, 0, null); // Get required buffer size.
        var buffer = new byte[bufferSize]; // Allocate sufficient buffer
        if (GetEnhMetaFileBits (enfMetafileHandle, bufferSize, buffer) <= 0) // Get raw metafile data.
        {
            throw new SystemException ("Fail");
        }

        var ms = File.Open (fileName, FileMode.Create);
        ms.Write (buffer, 0, bufferSize);
        ms.Close();
        ms.Dispose();
        if (!DeleteEnhMetaFile (enfMetafileHandle)) //free handle
        {
            throw new SystemException ("Fail Free");
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="hemf"></param>
    /// <param name="cbBuffer"></param>
    /// <param name="lpbBuffer"></param>
    /// <returns></returns>
    [DllImport ("gdi32")]
    public static extern int GetEnhMetaFileBits (int hemf, int cbBuffer, byte[]? lpbBuffer);

    /// <summary>
    ///
    /// </summary>
    /// <param name="hemfbitHandle"></param>
    /// <returns></returns>
    [DllImport ("gdi32")]
    public static extern bool DeleteEnhMetaFile (int hemfbitHandle);
}
