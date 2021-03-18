// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* FormatExit.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;
using AM.Collections;




#endregion

namespace ManagedIrbis.Pft.Infrastructure
{
    /// <summary>
    /// Umarci.
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
                [CanBeNull] PftNode node,
                string name,
                [CanBeNull] string expression
            )
        {
            Code.NotNull(context, "context");
            Code.NotNullNorEmpty(name, "name");

            IFormatExit format;
            if (!Registry.TryGetValue(name, out format))
            {
                Log.Error
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
