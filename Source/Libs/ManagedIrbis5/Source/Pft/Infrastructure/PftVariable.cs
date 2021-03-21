// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftVariable.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure
{
    /// <summary>
    ///
    /// </summary>
    public sealed class PftVariable
    {
        #region Events

        ///// <summary>
        ///// Вызывается непосредственно перед считыванием значения.
        ///// </summary>
        //public event EventHandler<PftDebugEventArgs> BeforeReading;

        ///// <summary>
        ///// Вызывается непосредственно после модификации.
        ///// </summary>
        //public event EventHandler<PftDebugEventArgs> AfterModification;

        #endregion

        #region Properties

        /// <summary>
        /// Имя переменной.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Признак числовой переменной.
        /// </summary>
        public bool IsNumeric { get; set; }

        /// <summary>
        /// Числовое значение.
        /// </summary>
        public double NumericValue { get; set; }

        /// <summary>
        /// Строковое значение.
        /// </summary>
        public string StringValue { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftVariable()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftVariable
            (
                string name,
                bool isNumeric
            )
        {
            Name = name;
            IsNumeric = isNumeric;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftVariable
            (
                string name,
                double numericValue
            )
        {
            Name = name;
            IsNumeric = true;
            NumericValue = numericValue;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftVariable
            (
                string name,
                string stringValue
            )
        {
            Name = name;
            StringValue = stringValue;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            result.Append(Name.ToVisibleString());
            result.Append(": ");
            if (IsNumeric)
            {
                result.Append(NumericValue.ToInvariantString());
            }
            else
            {
                if (ReferenceEquals(StringValue, null))
                {
                    result.Append("(null)");
                }
                else
                {
                    result.Append('\"');
                    result.Append(StringValue);
                    result.Append('\"');
                }
            }

            return result.ToString();
        }

        #endregion
    }
}
