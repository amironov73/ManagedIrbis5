// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* ObjectTimeStamps.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Runtime.CompilerServices;

#endregion

#nullable enable

namespace AM.Events;

//
// Заимствовано из проекта DebounceMonitoring
//
// https://github.com/SIDOVSKY/DebounceMonitoring
//
// copyright Vadim Sedov
//

internal class ObjectTimeStamps
{
    private ConditionalWeakTable<object, Ref<DateTime>>? _manyObjectStamps;
    private WeakReference? _singleRef;
    private DateTime _singleRefStamp;

    public ref DateTime GetOrAddRef
        (
            object obj
        )
    {
        if (_manyObjectStamps is not null)
            return ref _manyObjectStamps.GetValue(obj, createValueCallback: _ => new()).Value;

        if (_singleRef?.Target is not object singleObject)
        {
            _singleRef = new WeakReference(singleObject = obj);
        }

        if (singleObject == obj)
            return ref _singleRefStamp;

        var newStamp = new Ref<DateTime>();

        // Transform into the multi-object mode
        _manyObjectStamps = new ConditionalWeakTable<object, Ref<DateTime>>();
        _manyObjectStamps.Add(singleObject, new Ref<DateTime>(_singleRefStamp));
        _manyObjectStamps.Add(obj, newStamp);

        _singleRef = null;

        return ref newStamp.Value;
    }
}
