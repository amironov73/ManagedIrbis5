// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* ParserStateUtility.cs -- работа с состоянием разбора
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using Pidgin;

#endregion

namespace ParsingExperiments;

internal static class ParseStateUtility
{
    public static void DumpChar (this ref ParseState<char> state)
    {
        if (state.HasCurrent)
        {
            var chr = state.Current;
            if (chr >= ' ')
            {
                Console.WriteLine ($"'{chr}' => {(int) chr}");
            }
            else
            {
                Console.WriteLine ($"=> {(int) chr}");
            }
        }
        else
        {
            Console.WriteLine ("no current");
        }
    }

    public static void EatChar (this ref ParseState<char> state, char chr)
    {
        if (state.HasCurrent)
        {
            if (state.Current == chr)
            {
                state.Advance ();
            }
        }
    }

    public static char ReadChar (this ref ParseState<char> state, bool advance = true)
    {
        char result = '\0';
        if (state.HasCurrent)
        {
            result = state.Current;
            if (advance)
            {
                state.Advance ();
            }
        }

        return result;
    }
}
