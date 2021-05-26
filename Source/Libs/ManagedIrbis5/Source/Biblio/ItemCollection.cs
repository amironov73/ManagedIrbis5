// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* ItemCollection.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Linq;
using System.Text;

using AM;
using AM.Collections;
using AM.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Biblio
{
    /// <summary>
    ///
    /// </summary>
    public sealed class ItemCollection
        : NonNullCollection<BiblioItem>,
        IVerifiable
    {
        #region Properties

        #endregion

        #region Private members

        private static void ReadDigit
            (
                TextNavigator navigator,
                StringBuilder text
            )
        {
            var c = navigator.PeekChar();
            if (char.IsDigit(c))
            {
                navigator.ReadChar();
                text.Append(c);
            }
        }

        private static string? _TrimOrder
            (
                string? text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            var result = new StringBuilder();
            var navigator = new TextNavigator(text);
            ReadDigit(navigator, result);
            ReadDigit(navigator, result);
            ReadDigit(navigator, result);
            ReadDigit(navigator, result);

            while (navigator.IsDigit())
            {
                navigator.ReadChar();
            }

            result.Append(navigator.GetRemainingText());

            return result.ToString();
        }

        private static bool _IsOfficial
            (
                Record record
            )
        {
            // Официальные документы имеют характер n или 67

            var character = new[]
            {
                record.FM(900, 'c'),
                record.FM(900, '2'),
                record.FM(900, '3'),
                record.FM(900, '4'),
                record.FM(900, '5'),
                record.FM(900, '6')
            };

            return character.Contains("n")
                   || character.Contains("N")
                   || character.Contains("67");
        }

        private static bool _IsForeign
            (
                Record record
            )
        {
            // У иностранных книг язык не rus
            // Если язык не указан, считаем, что rus

            var language = record.FM(101);
            if (language.IsEmpty())
            {
                language = "rus";
            }

            return !language.SameString("rus");
        }

        private static int _Comparison
            (
                BiblioItem x,
                BiblioItem y
            )
        {
            var xrec = x.Record;
            var yrec = y.Record;
            if (!ReferenceEquals(xrec, null) && !ReferenceEquals(yrec, null))
            {
                // Поднимаем официальные документы

                var xup = _IsOfficial(xrec);
                var yup = _IsOfficial(yrec);
                if (xup != yup)
                {
                    return xup ? -1 : 1;
                }

                // Опускаем иностранные документы

                var xdown = _IsForeign(xrec);
                var ydown = _IsForeign(yrec);
                if (xdown != ydown)
                {
                    return xdown ? 1 : -1;
                }
            }

            return NumberText.Compare
                (
                    x.Order,
                    y.Order
                );
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Sort items by <see cref="BiblioItem.Order"/> field.
        /// </summary>
        public void SortByOrder()
        {
            var list = this.ToList();
            foreach (var item in list)
            {
                item.Order = _TrimOrder(item.Order);
            }
            list.Sort(_Comparison);
            Clear();
            AddRange(list);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier
                = new Verifier<ItemCollection>(this, throwOnError);

            foreach (var item in this)
            {
                verifier.VerifySubObject(item, "item");
            }

            return verifier.Result;
        }

        #endregion

        #region Object members

        #endregion
    }
}
