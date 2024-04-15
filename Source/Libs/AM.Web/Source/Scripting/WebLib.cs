// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConvertIfStatementToConditionalTernaryExpression
// ReSharper disable HeapView.BoxingAllocation
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* WebLib.cs -- библиотека функций для работы с веб
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

using AM.Scripting.Barsik;

using static AM.Scripting.Barsik.Builtins;

#endregion

namespace AM.Web.Scripting;

/// <summary>
/// Библиотека функций для работы с веб.
/// </summary>
public sealed class WebLib
    : IBarsikModule
{
    #region Properties


    /// <summary>
    /// Реестр стандартных функций.
    /// </summary>
    public static readonly Dictionary<string, FunctionDescriptor> Registry = new ()
    {
        { "save_page", new FunctionDescriptor ("save_page", SavePage) },
    };

    #endregion

    #region Public methods

    /// <summary>
    /// Раскодирование строки подключения либо пароля.
    /// </summary>
    public static dynamic? SavePage
        (
            Context context,
            dynamic?[] args
        )
    {
        if (Compute (context, args, 0) is string { Length: > 0 } url )
        {
            var uri = new Uri (url);
            var downloader = new PageDownloader();
            var fileName = Compute (context, args, 1) as string;
            if (string.IsNullOrEmpty (fileName))
            {
                downloader.DownloadAsync (uri).GetAwaiter().GetResult();
            }
            else
            {
                downloader.DownloadAsync (uri, fileName).GetAwaiter().GetResult();
            }
        }

        return null;
    }


    #endregion

    #region IBarsikModule members

    /// <inheritdoc cref="IBarsikModule.Description"/>
    public string Description => "WebLib";

    /// <inheritdoc cref="IBarsikModule.Version"/>
    public Version Version { get; } = new (1, 0);

    /// <inheritdoc cref="IBarsikModule.AttachModule"/>
    public bool AttachModule
        (
            Interpreter interpreter
        )
    {
        Sure.NotNull (interpreter);

        var context = interpreter.Context.ThrowIfNull();
        foreach (var descriptor in Registry)
        {
            context.Functions[descriptor.Key] = descriptor.Value;
        }

        var assembly = typeof (WebLib).Assembly;
        StdLib.LoadAssembly (context, new dynamic?[] { assembly.GetName().Name });
        // StdLib.Use (context, new dynamic?[] { "AM.Web" });

        return true;
    }

    /// <inheritdoc cref="IBarsikModule.DetachModule"/>
    public void DetachModule
        (
            Interpreter interpreter
        )
    {
        Sure.NotNull (interpreter);

        var context = interpreter.Context.ThrowIfNull();
        interpreter.ExternalCodeHandler = null;
        foreach (var descriptor in Registry)
        {
            context.Functions.Remove (descriptor.Key);
        }
    }

    #endregion


    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return Description;
    }

    #endregion
}
