// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* DirectiveNode.cs -- директива
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;
using System.Reflection;

using AM.Text;

#endregion

#nullable enable

namespace AM.Scripting.Barsik;

/// <summary>
/// Директива
/// </summary>
internal sealed class DirectiveNode
    : StatementNode
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public DirectiveNode
        (
            string line
        )
    {
        Sure.NotNullNorEmpty (line);

        var navigator = new ValueTextNavigator (line);
        _code = navigator.ReadUntil (' ', '\t').ToString();
        navigator.SkipWhitespace();
        _argument = navigator.GetRemainingText().EmptyToNull();
    }

    #endregion

    #region Private members

    private readonly string _code;
    private readonly string? _argument;

    /// <summary>
    /// Загрузка указанной сборки.
    /// </summary>
    private static void Reference
        (
            Context context,
            string assembly
        )
    {
        Sure.NotNullNorEmpty (assembly);

        assembly = assembly.Trim();
        Sure.NotNullNorEmpty (assembly);

        try
        {
            Assembly loaded;

            if (File.Exists (assembly))
            {
                var fullPath = Path.GetFullPath (assembly);
                loaded = Assembly.LoadFile (fullPath);
            }
            else
            {
                loaded = Assembly.Load (assembly);
            }

            context.Output.WriteLine ($"Assembly loaded: {loaded.GetName()}");
        }
        catch (Exception exception)
        {
            context.Error.WriteLine (exception.Message);
        }
    }

    /// <summary>
    /// Загрузка и исполнение скрипта из указанного файла.
    /// </summary>
    private static void LoadScript
        (
            Context context,
            string scriptPath
        )
    {
        Sure.NotNullNorEmpty (scriptPath);

        var sourceCode = File.ReadAllText (scriptPath);
        var program = Grammar.ParseProgram (sourceCode);
        program.Execute (context);
    }

    /// <summary>
    /// Подключение пространства имен.
    /// </summary>
    private static void UsingNamespace
        (
            Context context,
            string? name
        )
    {
        if (!string.IsNullOrEmpty (name))
        {
            name = name.Trim();
        }

        if (string.IsNullOrEmpty (name))
        {
            context.DumpNamespaces();
        }
        else
        {
            context.Namespaces[name] = null;
        }
    }

    #endregion

    #region StatementNode members

    /// <inheritdoc cref="StatementNode.Execute"/>
    public override void Execute
        (
            Context context
        )
    {
        PreExecute (context);

        switch (_code)
        {
            case "r":
                Reference (context, _argument.ThrowIfNullOrEmpty ());
                break;

            case "l":
                LoadScript (context, _argument.ThrowIfNullOrEmpty ());
                break;

            case "u":
                UsingNamespace (context, _argument);
                break;

            default:
                context.Error.WriteLine ($"Unknown directive: {_code} {_argument}");
                break;
        }
    }

    #endregion
}
