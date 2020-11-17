// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* RussianStringComparer.cs -- сравнивает строки согласно русской локали
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Globalization;

#endregion

#nullable enable

namespace AM.Globalization
{
    /// <summary>
    /// Сравнивает строки согласно русской локали.
    /// </summary>
    public class RussianStringComparer
        : StringComparer
    {
        #region Properties

        ///<summary>
        /// Consider YO letter?
        ///</summary>
        public bool ConsiderYo { get; private set; }

        ///<summary>
        /// Ignore case?
        ///</summary>
        public bool IgnoreCase { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="considerYo">if set to <c>true</c>
        /// [consider yo].</param>
        /// <param name="ignoreCase">if set to <c>true</c>
        /// [ignore case].</param>
        public RussianStringComparer
            (
                bool considerYo,
                bool ignoreCase
            )
        {
            ConsiderYo = considerYo;
            IgnoreCase = ignoreCase;

            var russianCulture = BuiltinCultures.Russian;

            var options = ignoreCase
                ? CompareOptions.IgnoreCase
                : CompareOptions.None;

            _innerComparer = (left, right)
                => russianCulture.CompareInfo.Compare
                    (
                        left,
                        right,
                        options
                    );
        }

        #endregion

        #region Private members

        private readonly Func<string?, string?, int> _innerComparer;

        private string? _Replace
            (
                string? str
            )
        {
            if (ReferenceEquals(str, null))
            {
                return null;
            }

            if (ConsiderYo)
            {
                str = str
                    .Replace('ё', 'е')
                    .Replace('Ё', 'Е');
            }

            return str;
        }

        #endregion

        #region StringComparer members

        ///<inheritdoc/>
        public override int Compare
            (
                string? x,
                string? y
            )
        {
            var xCopy = _Replace(x);
            var yCopy = _Replace(y);

            return _innerComparer
                (
                    xCopy,
                    yCopy
                 );
        }

        ///<inheritdoc/>
        public override bool Equals
            (
                string? x,
                string? y
            )
        {
            var xCopy = _Replace(x);
            var yCopy = _Replace(y);

            return _innerComparer
                (
                    xCopy,
                    yCopy
                 ) == 0;
        }

        ///<inheritdoc/>
        public override int GetHashCode
            (
                string obj
            )
        {
            var objCopy = _Replace(obj);

            if (IgnoreCase
                && !ReferenceEquals(objCopy, null))
            {
                objCopy = objCopy.ToUpper();
            }

            return ReferenceEquals(objCopy, null)
                ? 0
                : objCopy.GetHashCode();
        }

        #endregion
    }
}
