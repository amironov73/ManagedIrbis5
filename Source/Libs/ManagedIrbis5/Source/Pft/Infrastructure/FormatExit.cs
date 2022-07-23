// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* FormatExit.cs -- форматный выход
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;
using AM.Collections;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure;

/// <summary>
/// Форматный выход.
/// </summary>
public static class FormatExit
{
    #region Properties

    /// <summary>
    /// Реестр форматных выходов.
    /// </summary>
    public static CaseInsensitiveDictionary<IFormatExit> Registry { get; }

    #endregion

    #region Construction

    static FormatExit()
    {
        Registry = new CaseInsensitiveDictionary<IFormatExit>();

        var unifor = new Unifor();
        Registry.Add ("unifor", unifor);
        Registry.Add ("uf", unifor);

        var umarci = new Umarci();
        Registry.Add ("umarci", umarci);
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Execute the expression on the given context.
    /// </summary>
    public static void Execute
        (
            PftContext context,
            PftNode? node,
            string name,
            string? expression
        )
    {
        Sure.NotNull (context);
        Sure.NotNullNorEmpty (name);

        if (!Registry.TryGetValue (name, out var formatExit))
        {
            Magna.Logger.LogError
                (
                    nameof (FormatExit) + "::" + nameof (Execute)
                    + ": unknown name {Name}",
                    name
                );

            throw new PftSemanticException ("unknown format exit: " + name);
        }

        formatExit.Execute
            (
                context,
                node,
                expression
            );
    }

    #endregion
}
