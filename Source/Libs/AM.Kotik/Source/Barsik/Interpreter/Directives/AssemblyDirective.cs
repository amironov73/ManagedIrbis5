// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* AssemblyDirective.cs -- вывод списка загруженных сборок
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Linq;

#endregion

#nullable enable

namespace AM.Kotik.Barsik.Directives;

/// <summary>
/// Директива: вывод списка загруженных сборок.
/// </summary>
public sealed class AssemblyDirective
    : DirectiveBase
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public AssemblyDirective()
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
        if (string.IsNullOrEmpty (argument))
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .OrderBy (asm => asm.FullName)
                .ToArray();

            foreach (var assembly in assemblies)
            {
                context.Commmon.Output?.WriteLine (assembly.FullName);
            }
        }
        else
        {
            if (argument.StartsWith ("/"))
            {
                // TODO это какая-то суб-команда
            }
            else
            {
                var asm = context.LoadAssembly (argument);
                if (asm is null)
                {
                    context.Commmon.Error?.WriteLine ($"Faield to load assembly: {argument}");
                }
                else
                {
                    context.Commmon.Output?.WriteLine ($"Loaded: {asm}");
                }
            }
        }
    }

    #endregion
}
