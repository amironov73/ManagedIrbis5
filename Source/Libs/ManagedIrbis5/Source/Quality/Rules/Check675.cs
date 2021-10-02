// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo

/* Check675.cs -- УДК
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Quality.Rules
{
    /// <summary>
    /// УДК
    /// </summary>
    public sealed class Check675
        : QualityRule
    {
        #region Private members

        private void CheckField
            (
                Field field
            )
        {
            MustNotContainSubfields(field);

        } // method CheckField

        #endregion

        #region QualityRules members

        /// <inheritdoc cref="QualityRule.FieldSpec"/>
        public override string FieldSpec => "675";

        /// <inheritdoc cref="QualityRule.CheckRecord"/>
        public override RuleReport CheckRecord
            (
                RuleContext context
            )
        {
            BeginCheck(context);

            var fields = GetFields();
            if (fields.Length == 0)
            {
                AddDefect
                (
                    675,
                    5,
                    "Отсутствует УДК: поле 675"
                );
            }

            foreach (var field in fields)
            {
                CheckField(field);
            }


            return EndCheck();

        } // method CheckRecord

        #endregion

    } // class Check675

} // namespace ManagedIrbis.Quality.Rules
