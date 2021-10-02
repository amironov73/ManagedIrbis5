// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo

/* Require463.cs -- издание, в котором опубликована статья
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Quality.Rules
{
    /// <summary>
    /// Издание, в котором опубликована статья.
    /// </summary>
    public sealed class Require463
        : QualityRule
    {
        #region Private members

        private void CheckField
            (
                Field field
            )
        {
        } // method CheckField

        #endregion

        #region QualityRule members

        /// <inheritdoc cref="QualityRule.FieldSpec"/>
        public override string FieldSpec => "463";

        /// <inheritdoc cref="QualityRule.CheckRecord"/>
        public override RuleReport CheckRecord
            (
                RuleContext context
            )
        {
            BeginCheck(context);

            var fields = GetFields();
            foreach (var field in fields)
            {
                CheckField(field);
            }

            return EndCheck();

        } // method CheckRecord

        #endregion

    } // class Require463

} // namespace ManagedIrbis.Quality.Rules
