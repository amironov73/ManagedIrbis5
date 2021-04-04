// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* Ref.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Runtime.CompilerServices;

#endregion

#nullable enable

namespace AM.Events
{
    //
    // Заимствовано из проекта DebounceMonitoring
    //
    // https://github.com/SIDOVSKY/DebounceMonitoring
    //
    // copyright Vadim Sedov
    //

    internal sealed class Ref<T> where T
        : struct
    {
        public T Value;

        public Ref(T value = default)
        {
            Value = value;
        }

        public override string ToString() => Value.ToString();
    }
}
