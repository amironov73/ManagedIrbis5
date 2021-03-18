// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftHtmlFormatter.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives




using ManagedIrbis.Pft.Infrastructure;


#endregion

namespace ManagedIrbis.Pft
{
    /// <summary>
    ///
    /// </summary>

    public class PftHtmlFormatter
        : PftFormatter
    {
        #region Properties

        /// <summary>
        /// Text separator.
        /// </summary>
        public PftTextSeparator Separator { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftHtmlFormatter()
        {
            Separator = new PftTextSeparator();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftHtmlFormatter
            (
                PftContext context
            )
            : base(context)
        {
            Separator = new PftTextSeparator();
        }

        #endregion

        #region PftFormatter members

        /// <inheritdoc cref="PftFormatter.ParseProgram" />
        public override void ParseProgram
            (
                string source
            )
        {
            Code.NotNull(source, "source");

            if (Separator.SeparateText(source))
            {
                Log.Error
                    (
                        "PftHtmlFormatter::ParseProgram: "
                        + "can't separate text"
                    );

                throw new PftSyntaxException();
            }

            string prepared = Separator.Accumulator;

            base.ParseProgram(prepared);
        }

        #endregion
    }
}
