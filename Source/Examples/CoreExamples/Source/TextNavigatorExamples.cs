// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

#region Using directives

using AM.Text;
using static System.Console;

#endregion

#nullable enable

namespace CoreExamples;

static class TextNavigatorExamples
{
    static void ReadFrom()
    {
        var text = "У попа (была) собака, он её любил.";
        var navigator = new ValueTextNavigator (text);

        navigator.SkipTo ('(');
        var word = navigator.ReadFrom ('(', ')');
        WriteLine (word.ToString());
    }

    static void ReadEscapedUntil()
    {
        var text = "У попа\\tбыла \\tсобака! Он её любил";
        var navigator = new ValueTextNavigator (text);

        var part = navigator.ReadEscapedUntil ('\\', '!');
        WriteLine (part);
    }

    static void ReadWord()
    {
        var text = "У попа была собака, он её любил.";
        var navigator = new ValueTextNavigator (text);

        while (true)
        {
            var word = navigator.ReadWord();
            if (word.IsEmpty)
            {
                break;
            }

            Write ($"{word.ToString()}, ");

            if (!navigator.SkipNonWord())
            {
                break;
            }
        }

        WriteLine();
    }

    public static void All()
    {
        ReadWord();
        ReadFrom();
        ReadEscapedUntil();
    }
}
