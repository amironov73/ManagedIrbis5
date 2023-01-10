// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* AssemblyCollection.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections;

#endregion

#nullable enable

namespace AM.Reporting.Code;

internal class AssemblyCollection
    : CollectionBase
{
    public AssemblyDescriptor this [int index]
    {
        get => (AssemblyDescriptor) List[index]!;
        set => List[index] = value;
    }

    public void AddRange
        (
            AssemblyDescriptor[] range
        )
    {
        Sure.NotNull (range);

        foreach (var t in range)
        {
            Add (t);
        }
    }

    public int Add
        (
            AssemblyDescriptor? value
        )
    {
        if (value is null)
        {
            return -1;
        }

        return List.Add (value);
    }

    public void Insert
        (
            int index,
            AssemblyDescriptor? value
        )
    {
        if (value is not null)
        {
            List.Insert (index, value);
        }
    }

    public void Remove
        (
            AssemblyDescriptor value
        )
    {
        Sure.NotNull (value);

        List.Remove (value);
    }

    public int IndexOf
        (
            AssemblyDescriptor value
        )
    {
        Sure.NotNull (value);

        return List.IndexOf (value);
    }

    public bool Contains
        (
            AssemblyDescriptor value
        )
    {
        Sure.NotNull (value);

        return List.Contains (value);
    }
}
