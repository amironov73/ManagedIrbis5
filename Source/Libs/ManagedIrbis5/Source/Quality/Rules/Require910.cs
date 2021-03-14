// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo

/* Require910.cs -- сведения об экземплярах
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using ManagedIrbis.Fields;
using ManagedIrbis.Menus;

#endregion

#nullable enable

namespace ManagedIrbis.Quality.Rules
{
    /// <summary>
    /// Сведения об экземплярах.
    /// </summary>
    public sealed class Require910
        : QualityRule
    {
        #region Private members

        private MenuFile _statusMenu;
        private MenuFile _placeMenu;

        private void CheckField
            (
                Field field
            )
        {
            ExemplarInfo exemplar = ExemplarInfo.Parse(field);

            if (!CheckForMenu(_statusMenu, exemplar.Status))
            {
                AddDefect
                    (
                        field,
                        10,
                        "Статус экземпляра не из словаря"
                    );
            }
            if (!CheckForMenu(_placeMenu, exemplar.Place))
            {
                AddDefect
                    (
                        field,
                        10,
                        "Место хранения не из словаря"
                    );
            }
            if (string.IsNullOrEmpty(exemplar.Number))
            {
                AddDefect
                    (
                        field,
                        10,
                        "Не задан номер экземпляра"
                    );
            }
        }

        #endregion

        #region QualityRule members

        /// <inheritdoc />
        public override string FieldSpec
        {
            get { return "910"; }
        }

        /// <inheritdoc />
        public override RuleReport CheckRecord
            (
                RuleContext context
            )
        {
            BeginCheck(context);

            if (!IsBook())
            {
                goto DONE;
            }

            Field[] fields = GetFields();
            if (fields.Length == 0)
            {
                AddDefect
                    (
                        910,
                        10,
                        "Нет сведений об экземплярах: поле 910"
                    );
            }

            _statusMenu = CacheMenu("ste.mnu", _statusMenu);
            _placeMenu = CacheMenu("mhr.mnu", _placeMenu);

            foreach (Field field in fields)
            {
                CheckField(field);
            }

            MustBeUniqueSubfield
                (
                    fields,
                    'b'
                );

            DONE: return EndCheck();
        }

        #endregion

    } // class Require910

} // namespace ManagedIrbis.Quality.Rules
