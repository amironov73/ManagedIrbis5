// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* ProgressMixer.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

#nullable enable

namespace AM.Updating.Internal;

internal class ProgressMixer
{
    private readonly IProgress<double> _output;
    private readonly Dictionary<int, double> _splitTotals;

    private int _splitCount;

    public ProgressMixer (IProgress<double> output)
    {
        _output = output;
        _splitTotals = new Dictionary<int, double>();
    }

    public IProgress<double> Split (double multiplier)
    {
        var index = _splitCount++;
        return new Progress<double> (p =>
        {
            lock (_splitTotals)
            {
                _splitTotals[index] = multiplier * p;
                _output.Report (_splitTotals.Values.Sum());
            }
        });
    }
}
