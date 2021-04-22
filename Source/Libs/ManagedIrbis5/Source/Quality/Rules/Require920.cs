// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo

/* Require920.cs -- рабочий лист
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Quality.Rules
{
    /// <summary>
    /// Рабочий лист
    /// </summary>
    public sealed class Require920
        : QualityRule
    {
        #region Private members

        private static readonly string[] _goodWorksheets =
        {
            "PAZK",
            "SPEC",
            "PVK",
            "NJ",
            "NJK",
            "NJP",
            "ASP"
        };

        #endregion

        #region QualityRule members

        /// <inheritdoc />
        public override string FieldSpec => "920";

        /// <inheritdoc />
        public override RuleReport CheckRecord
            (
                RuleContext context
            )
        {
            BeginCheck(context);

            var worksheet = Worksheet;
            if (string.IsNullOrEmpty(worksheet))
            {
                AddDefect
                    (
                        920,
                        20,
                        "Отсутствует поле 920: Рабочий лист"
                    );
            }

            var fields = GetFields();
            if (fields.Length > 1)
            {
                AddDefect
                    (
                        920,
                        20,
                        "Повторяется поле 920: Рабочий лист"
                    );
            }
            foreach (var field in fields)
            {
                MustNotContainSubfields
                    (
                        field
                    );

                worksheet = field.Value.ToString();
                if (!worksheet.IsOneOf(_goodWorksheets))
                {
                    AddDefect
                        (
                            field,
                            20,
                            "Неожиданный рабочий лист"
                        );
                }
            }

            return EndCheck();
        }

        #endregion

    } // class Require920

} // namespace ManagedIrbis.Quality.Rules
