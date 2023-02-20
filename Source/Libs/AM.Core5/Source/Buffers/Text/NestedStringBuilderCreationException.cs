// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo

/* NestedStringBuilderCreationException.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Buffers.Text;

// Currently, this class is internals
internal class NestedStringBuilderCreationException
    : InvalidOperationException
{
    internal protected NestedStringBuilderCreationException (string typeName, string extraMessage = "")
        : base ($"A nested call with `notNested: true`, or Either You forgot to call {typeName}.Dispose() of  in the past.{extraMessage}")
    {
    }

    internal protected NestedStringBuilderCreationException (string message, Exception innerException) : base (message,
        innerException)
    {
    }
}
