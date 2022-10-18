// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* MultiRange.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

#nullable enable

namespace AM.Text;

public class MultiRange
{
    private readonly SortedList<CodepointRange, object?> _ranges = new ();
    public IReadOnlyList<CodepointRange> Ranges => (IReadOnlyList<CodepointRange>) _ranges;

    public MultiRange (params string[] ranges)
    {
        foreach (var range in ranges)
        {
            var item = new CodepointRange (range);
            _ranges.Add (item, null);
        }
    }

    public MultiRange (params CodepointRange[] ranges)
    {
        foreach (var range in ranges)
        {
            _ranges.Add (range, null);

        }
    }

    public MultiRange (IEnumerable<CodepointRange> ranges)
    {
        foreach (var range in ranges)
        {
            _ranges.Add (range, null);
        }
    }

    public bool Contains (Codepoint codepoint)
    {
        var index = _ranges.IndexOfKey (new CodepointRange (codepoint));
        if (index > 0)
        {
            return true;
        }
        // No match, value is complement of Count or next greatest index
        index = ~index;
        if (index == 0)
        {
            return false;
        }


        throw new NotImplementedException();

        // In case of range including this codepoint...
//        return _ranges[index - 1].Contains (codepoint)
        // and in case of range starting with this codepoint
        //             || index < _ranges.Count && _ranges[index].Contains(codepoint);
    }
}
