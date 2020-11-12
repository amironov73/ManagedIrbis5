// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* RecordField.cs -- поле библиографической записи
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Поле библиографической записи.
    /// </summary>
    public class Field
    {
        #region Properties

        /// <summary>
        /// Метка поля.
        /// </summary>
        public int Tag { get; set; }

        /// <summary>
        /// Значение поля до первого разделителя.
        /// </summary>
        public string? Value { get; set; }

        /// <summary>
        /// Список подполей.
        /// </summary>
        public List<SubField> Subfields { get; } = new List<SubField>();

        #endregion

        #region Public methods

        /// <summary>
        /// Добавление подполя.
        /// </summary>
        public Field Add
            (
                char code,
                string? value
            )
        {
            var subfield = new SubField { Code = code, Value = value };
            Subfields.Add(subfield);

            return this;
        }

        /// <summary>
        /// Очистка подполей.
        /// </summary>
        public Field Clear()
        {
            Value = null;
            Subfields.Clear();

            return this;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            int length = 4 + (Value?.Length ?? 0)
                           + Subfields.Sum(sf => (sf.Value?.Length ?? 0) + 2);
            StringBuilder result = new StringBuilder(length);
            //result.Append(Tag.ToInvariantString())
            result.Append(Tag)
                .Append('#')
                .Append(Value);
            foreach (var subfield in Subfields)
            {
                result.Append(subfield);
            }

            return result.ToString();
        }

        #endregion
    }
}
