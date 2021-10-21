// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* Technology.cs -- информация о создании и внесении модификаций в запись
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.IO;

using AM;
using AM.IO;
using AM.Runtime;

using ManagedIrbis.Mapping;

#endregion

#nullable enable

namespace ManagedIrbis.Fields
{
    /// <summary>
    /// Информация о создании и внесении изменений в библиографическую запись.
    /// Поле 907.
    /// </summary>
    public sealed class Technology
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Constants

        /// <summary>
        /// Известные коды подполей.
        /// </summary>
        public const string KnownCodes = "abc";

        /// <summary>
        /// Метка поля.
        /// </summary>
        public const int Tag = 907;

        #endregion

        #region Properties

        /// <summary>
        /// Этап работы, подполе C. См. <see cref="WorkPhase"/>.
        /// </summary>
        [SubField('c')]
        public string? Phase { get; set; }

        /// <summary>
        /// Дата, подполе A.
        /// </summary>
        [SubField('a')]
        public string? Date { get; set; }

        /// <summary>
        /// Ответственное лицо, ФИО.
        /// </summary>
        [SubField('b')]
        public string? Responsible { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Применение информации к полю.
        /// </summary>
        public void ApplyToField
            (
                Field field
            )
        {
            field
                .ApplySubField('a', Date)
                .ApplySubField('b', Responsible)
                .ApplySubField('c', Phase);

        } // method ApplyToField

        /// <summary>
        /// Получение даты последней модификации записи.
        /// </summary>
        public static string? GetLatestDate
            (
                Record record
            )
        {
            string? result = null;

            foreach (var field in record.Fields.GetField(Tag))
            {
                var candidate = field.GetFirstSubFieldValue('a');
                if (!string.IsNullOrEmpty(candidate))
                {
                    if (string.IsNullOrEmpty(result))
                    {
                        result = candidate;
                    }
                    else
                    {
                        result = string.CompareOrdinal(result, candidate) < 0
                            ? candidate
                            : result;
                    }
                }
            }

            return result;

        } // method GetLatestDate

        /// <summary>
        /// Разбор записи <see cref="Record"/>.
        /// </summary>
        public static Technology[] Parse
            (
                Record record,
                int tag = Tag
            )
        {
            var result = new List<Technology>();
            foreach (var field in record.Fields)
            {
                if (field.Tag == tag)
                {
                    var tech = Parse(field);
                    result.Add(tech);
                }
            }

            return result.ToArray();

        } // method Parse

        /// <summary>
        /// Разбор заданного поля <see cref="Field"/>.
        /// </summary>
        public static Technology Parse
            (
                Field field
            )
        {
            var result = new Technology
            {
                Date = field.GetFirstSubFieldValue('a'),
                Responsible = field.GetFirstSubFieldValue('b'),
                Phase = field.GetFirstSubFieldValue('c')
            };

            return result;

        } // method Parse

        /// <summary>
        /// Преобразование информации в поле.
        /// </summary>
        public Field ToField()
        {
            var result = new Field(Tag)
                .AddNonEmptySubField('a', Date)
                .AddNonEmptySubField('b', Responsible)
                .AddNonEmptySubField('c', Phase);

            return result;

        } // method ToField

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Date = reader.ReadNullableString();
            Responsible = reader.ReadNullableString();
            Phase = reader.ReadNullableString();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer
                .WriteNullable(Date)
                .WriteNullable(Responsible)
                .WriteNullable(Phase);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier = new Verifier<Technology>(this, throwOnError);

            verifier.Assert
                (
                    !string.IsNullOrEmpty(Date)
                    || !string.IsNullOrEmpty(Responsible)
                    || !string.IsNullOrEmpty(Phase)
                );

            return verifier.Result;
        }

        #endregion


        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString() => $"{Phase}: {Date}: {Responsible}";

        #endregion

    } // class Technology

} // namespace ManagedIrbis.Fields
