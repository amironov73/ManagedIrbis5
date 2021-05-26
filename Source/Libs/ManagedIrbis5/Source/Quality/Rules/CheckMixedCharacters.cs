// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo

/* CheckMixedCharacters.cs -- проверка на смешение символов.
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Text.RegularExpressions;

using AM.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Quality.Rules
{
    /// <summary>
    /// Проверка на смешение символов.
    /// </summary>
    public sealed class CheckMixedCharacters
        : QualityRule
    {
        #region Private members

        private readonly Regex _mixRegex = new Regex(@"\w+");

        private List<string> CheckText
            (
                string? text
            )
        {
            var result = new List<string>();

            if (!string.IsNullOrEmpty(text))
            {
                var match = _mixRegex.Match(text);
                while (match.Success)
                {
                    var classes = CharacterClassifier
                        .DetectCharacterClasses
                        (
                            match.Value
                        );
                    if (CharacterClassifier.IsBothCyrillicAndLatin(classes))
                    {
                        result.Add(match.Value);
                    }
                }
            }

            return result;
        }

        private static string FormatDefect
            (
                List<string> list
            )
        {
            var word = list.Count == 1
                ? "слове"
                : "словах";
            return string.Format
                (
                    "Смешение кириллицы и латиницы в {0}: {1}",
                    word,
                    string.Join(", ", list.ToArray())
                );
        }

        private void CheckField
            (
                Field field
            )
        {
            var result = CheckText(field.Value);
            if (result.Count != 0)
            {
                AddDefect
                    (
                        field,
                        15,
                        FormatDefect(result)
                    );
            }
        }

        private void CheckSubField
            (
                Field field,
                SubField subField
            )
        {
            var result = CheckText(subField.Value);
            if (result.Count != 0)
            {
                AddDefect
                    (
                        field,
                        subField,
                        15,
                        FormatDefect(result)
                    );
            }
        }

        #endregion

        #region QualityRule members

        /// <inheritdoc />
        public override string FieldSpec
        {
            get { return "*"; }
        }

        /// <inheritdoc />
        public override RuleReport CheckRecord
            (
                RuleContext context
            )
        {
            BeginCheck(context);

            var fields = GetFields();
            foreach (var field in fields)
            {
                CheckField
                    (
                        field
                    );
                foreach (var subField in field.Subfields)
                {
                    CheckSubField
                        (
                            field,
                            subField
                        );
                }

            }

            return EndCheck();
        }

        #endregion

    } // class CheckMixedCharacters

} // namespace ManagedIrbis.Quality.Rules
