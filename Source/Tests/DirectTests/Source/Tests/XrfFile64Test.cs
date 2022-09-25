// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* XrfFile64Test.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

using ManagedIrbis.Direct;

#endregion

#nullable enable

namespace DirectTests;

public class XrfFile64Test
{
    private static string _GetFileName() =>
        Unix.FindFileOrThrow (DirectUtility.CombinePath
            (
                Infrastructure.Irbis64RootPath,
                "Datai/IBIS/ibis.xrf"
            ));

    public static void XrfFile64_ReadRecord_1()
    {
        var fileName = _GetFileName();
        var mode = DirectAccessMode.ReadOnly;
        using var file = new XrfFile64 (fileName, mode);
        Infrastructure.AreEqual (fileName, file.FileName);
        var record = file.ReadRecord (1);
        Infrastructure.AreEqual (22951100L, record.Offset);
        Infrastructure.AreEqual (0, (int)record.Status);
    }

    public static void XrfFile64_LockRecord_1()
    {
        var fileName = _GetFileName();
        var mode = DirectAccessMode.Exclusive;
        using var file = new XrfFile64 (fileName, mode);
        Infrastructure.AreEqual (fileName, file.FileName);

        file.LockRecord (1, true);
        var record = file.ReadRecord (1);
        Infrastructure.AreEqual (64, (int)record.Status);

        file.LockRecord (1, false);
        record = file.ReadRecord (1);
        Infrastructure.AreEqual (0, (int)record.Status);
    }

    public static void XrfFile64_ReopenFile_1()
    {
        var fileName = _GetFileName();
        var mode = DirectAccessMode.Exclusive;
        using var file = new XrfFile64 (fileName, mode);

        mode = DirectAccessMode.ReadOnly;
        file.ReopenFile (mode);
    }
}
