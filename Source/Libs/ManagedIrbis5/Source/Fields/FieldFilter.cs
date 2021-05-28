// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable ConvertClosureToMethodGroup
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Local
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedParameter.Local
// ReSharper disable UseStringInterpolation

/* FieldFilter.cs -- динамическая фильтрация полей записи
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

using AM;

using ManagedIrbis.Pft;

#endregion

#nullable enable

namespace ManagedIrbis.Fields
{
    /// <summary>
    /// Динамическая фильтрация полей записи.
    /// </summary>
    public sealed class FieldFilter
    {
        #region Properties

        /// <summary>
        /// Провайдер.
        /// </summary>
        public ISyncProvider Provider { get; }

        /// <summary>
        /// Форматтер.
        /// </summary>
        public PftFormatter Formatter { get; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public FieldFilter
            (
                ISyncProvider provider,
                string format
            )
        {
            Provider = provider;

            Formatter = new PftFormatter();
            Formatter.SetProvider(provider);
            SetProgram(format);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Whether all fields satisfy the condition.
        /// </summary>
        public bool AllFields
            (
                IEnumerable<Field> fields
            )
        {
            var result = false;

            foreach (var field in fields)
            {
                result = CheckField(field);
                if (!result)
                {
                    break;
                }
            }

            return result;

        } // method AllFields

        /// <summary>
        /// Whether any field satisfy the condition.
        /// </summary>
        public bool AnyField
            (
                IEnumerable<Field> fields
            )
        {
            var result = false;

            foreach (var field in fields)
            {
                result = CheckField(field);
                if (result)
                {
                    break;
                }
            }

            return result;

        } // method AnyField

        /// <summary>
        /// Check the field.
        /// </summary>
        public bool CheckField
            (
                Field field
            )
        {
            var record = new Record();
            var copy = field.Clone();
            record.Fields.Add(copy);

            Formatter.Context.AlternativeRecord = field.Record;
            string text = Formatter.FormatRecord(record);
            bool result = text.SameString("1");

            return result;

        } // method CheckField

        /// <summary>
        /// Filter records.
        /// </summary>
        public Field[] FilterFields
            (
                IEnumerable<Field> fields
            )
        {
            var result = new List<Field>();

            foreach (var field in fields)
            {
                if (CheckField(field))
                {
                    result.Add(field);
                }
            }

            return result.ToArray();

        } // method FilterFields

        /// <summary>
        /// Filter records by field specification.
        /// </summary>
        public IEnumerable<Record> FilterRecords
            (
                IEnumerable<Record> records
            )
        {
            foreach (var record in records)
            {
                if (AnyField(record.Fields))
                {
                    yield return record;
                }
            }

        } // method FilterRecords

        /// <summary>
        /// Find first satisfying field.
        /// </summary>
        public Field? First
            (
                IEnumerable<Field> fields
            )
        {
            Field? result = null;

            foreach (var field in fields)
            {
                if (CheckField(field))
                {
                    result = field;
                    break;
                }
            }

            return result;

        } // method First

        /// <summary>
        /// Find last satisfying field.
        /// </summary>
        public Field? Last
            (
                IEnumerable<Field> fields
            )
        {
            Field? result = null;

            foreach (var field in fields)
            {
                if (CheckField(field))
                {
                    result = field;
                }
            }

            return result;

        } // method Last

        /// <summary>
        /// Set filter program.
        /// </summary>
        public void SetProgram
            (
                string format
            )
        {
            var text = $"if {format} then '1' else '0' fi";
            Formatter.ParseProgram(text);

        } // method SetProgram

        #endregion

    } // class FieldFilter

} // namespace ManagedIrbis.Fields
