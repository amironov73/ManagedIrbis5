// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* ListAssembliesDirective.cs -- вывод списка загруженных сборок
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

#endregion

#nullable enable

namespace AM.Kotik.Barsik.Directives;

/// <summary>
/// Директива: вывод списка загруженных сборок.
/// </summary>
public sealed class ListAssembliesDirective
    : DirectiveBase
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ListAssembliesDirective()
        : base ("asm")
    {
        // пустое тело метода
    }

    #endregion

    #region DirectiveBase members

    /// <inheritdoc cref="DirectiveBase.Execute"/>
    public override void Execute
        (
            Context context,
            string? argument
        )
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies()
            .OrderBy (asm => asm.FullName)
            .ToArray();
        
        foreach (var assembly in assemblies)
        {
            context.Output.WriteLine (assembly.FullName);
        }
    }

    #endregion
}
