// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UseNameofExpression

/* HtmlCmdLine.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;

#endregion

#nullable enable

namespace HtmlAgilityPack;

internal class HtmlCmdLine
{
    #region Static Members

    internal static bool Help;

    #endregion

    #region Constructors

    static HtmlCmdLine()
    {
        Help = false;
        ParseArgs();
    }

    #endregion

    #region Internal Methods

    internal static string GetOption (string name, string def)
    {
        var p = def;
        var args = Environment.GetCommandLineArgs();
        for (var i = 1; i < args.Length; i++)
        {
            GetStringArg (args[i], name, ref p);
        }

        return p;
    }

    internal static string GetOption (int index, string def)
    {
        var p = def;
        var args = Environment.GetCommandLineArgs();
        var j = 0;
        for (var i = 1; i < args.Length; i++)
        {
            if (GetStringArg (args[i], ref p))
            {
                if (index == j)
                {
                    return p;
                }
                else
                {
                    p = def;
                }

                j++;
            }
        }

        return p;
    }

    internal static bool GetOption (string name, bool def)
    {
        var p = def;
        var args = Environment.GetCommandLineArgs();
        for (var i = 1; i < args.Length; i++)
        {
            GetBoolArg (args[i], name, ref p);
        }

        return p;
    }

    internal static int GetOption (string name, int def)
    {
        var p = def;
        var args = Environment.GetCommandLineArgs();
        for (var i = 1; i < args.Length; i++)
        {
            GetIntArg (args[i], name, ref p);
        }

        return p;
    }

    #endregion

    #region Private Methods

    private static void GetBoolArg (string Arg, string Name, ref bool ArgValue)
    {
        if (Arg.Length < (Name.Length + 1)) // -name is 1 more than name
        {
            return;
        }

        if (('/' != Arg[0]) && ('-' != Arg[0])) // not a param
        {
            return;
        }

        if (Arg.Substring (1, Name.Length).ToLowerInvariant() == Name.ToLowerInvariant())
        {
            ArgValue = true;
        }
    }

    private static void GetIntArg (string Arg, string Name, ref int ArgValue)
    {
        if (Arg.Length < (Name.Length + 3)) // -name:12 is 3 more than name
        {
            return;
        }

        if (('/' != Arg[0]) && ('-' != Arg[0])) // not a param
        {
            return;
        }

        if (Arg.Substring (1, Name.Length).ToLowerInvariant() == Name.ToLowerInvariant())
        {
            try
            {
                ArgValue = Convert.ToInt32 (Arg.Substring (Name.Length + 2, Arg.Length - Name.Length - 2));
            }
            catch (Exception exception)
            {
                Debug.WriteLine (exception.Message);
            }
        }
    }

    private static bool GetStringArg (string Arg, ref string ArgValue)
    {
        if (('/' == Arg[0]) || ('-' == Arg[0]))
        {
            return false;
        }

        ArgValue = Arg;
        return true;
    }

    private static void GetStringArg (string Arg, string Name, ref string ArgValue)
    {
        if (Arg.Length < (Name.Length + 3)) // -name:x is 3 more than name
        {
            return;
        }

        if (('/' != Arg[0]) && ('-' != Arg[0])) // not a param
        {
            return;
        }

        if (Arg.Substring (1, Name.Length).ToLowerInvariant() == Name.ToLowerInvariant())
        {
            ArgValue = Arg.Substring (Name.Length + 2, Arg.Length - Name.Length - 2);
        }
    }

    private static void ParseArgs()
    {
        var args = Environment.GetCommandLineArgs();
        for (var i = 1; i < args.Length; i++)
        {
            // help
            GetBoolArg (args[i], "?", ref Help);
            GetBoolArg (args[i], "h", ref Help);
            GetBoolArg (args[i], "help", ref Help);
        }
    }

    #endregion
}
