// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* Program.cs -- точка входа в программу
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text;

using ManagedIrbis;
using ManagedIrbis.Formatting;

#endregion

#nullable enable

namespace HardTests;

static class Program
{
    static int Main
        (
            string[] args
        )
    {
        void action (HardFormat formatter, StringBuilder builder, Record record)
             => formatter.Brief (builder, record);

        OfflineTests.Run
            (
                args,
                action
            );

        return 0;
    }
}
