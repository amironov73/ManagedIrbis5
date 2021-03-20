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

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure
{
    /// <summary>
    /// Форматный выход.
    /// </summary>
    public static class FormatExit
    {
        #region Properties

        /// <summary>
        /// Registry.
        /// </summary>
        public static CaseInsensitiveDictionary<IFormatExit> Registry
        {
            get; private set;
        }

        #endregion

        #region Construction

        static FormatExit()
        {
            Registry = new CaseInsensitiveDictionary<IFormatExit>();

            Unifor unifor = new Unifor();
            Registry.Add("unifor", unifor);
            Registry.Add("uf", unifor);

            Umarci umarci = new Umarci();
            Registry.Add("umarci", umarci);
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
            if (!Registry.TryGetValue(name, out IFormatExit? format))
            {
                Magna.Error
                    (
                        "FormatExit::Execute: "
                        + "unknown name="
                        + name.ToVisibleString()
                    );

                throw new PftSemanticException("unknown format exit: " + name);
            }

            format.Execute
                (
                    context,
                    node,
                    expression
                );
        }

        #endregion
    }
}
