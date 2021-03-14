// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo

/* Require900.cs -- коды
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.Quality.Rules
{
    /// <summary>
    /// Коды
    /// </summary>
    public sealed class Require900
        : QualityRule
    {
        #region QualityRule members

        /// <inheritdoc />
        public override string FieldSpec
        {
            get { return "900"; }
        }

        /// <inheritdoc />
        public override RuleReport CheckRecord
            (
                RuleContext context
            )
        {
            BeginCheck(context);

            return EndCheck();
        }

        #endregion

    } // class Require900

} // namespace ManagedIrbis.Quality.Rules
