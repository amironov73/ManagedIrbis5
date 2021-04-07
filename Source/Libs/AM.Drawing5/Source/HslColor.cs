﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* HslColor.cs -- color in HSL colorspace.
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;

#endregion

#nullable enable

namespace AM.Drawing
{
    /// <summary>
    /// Color in HSL colorspace.
    /// </summary>
    public struct HslColor
    {
        #region Constants

        /// <summary>
        /// Minimum value of <see cref="HslColor"/> component.
        /// </summary>
        public const float MinComponentValue = 0f;

        /// <summary>
        /// Maximum value of <see cref="HslColor"/> component.
        /// </summary>
        public const float MaxComponentValue = 1f;

        #endregion

        #region Properties

        private float _h;

        /// <summary>
        /// Gets or sets H component value of the <see cref="HslColor"/>.
        /// </summary>
        /// <value>H component value.</value>
        public float H
        {
            [DebuggerStepThrough]
            get
            {
                return _h;
            }
            set
            {
                _CheckComponent(value, "H");
                _h = value;
            }
        }

        private float _l;

        /// <summary>
        /// Gets or sets L component value of the <see cref="HslColor"/>.
        /// </summary>
        /// <value>L component value.</value>
        public float L
        {
            [DebuggerStepThrough]
            get
            {
                return _l;
            }
            set
            {
                _CheckComponent(value, "L");
                _l = value;
            }
        }

        private float _s;

        /// <summary>
        /// Gets or sets S component value of the <see cref="HslColor"/>.
        /// </summary>
        /// <value>S component value.</value>
        public float S
        {
            [DebuggerStepThrough]
            get
            {
                return _s;
            }
            set
            {
                _CheckComponent(value, "S");
                _s = value;
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="T:HslColor"/> class.
        /// </summary>
        /// <param name="h">The h.</param>
        /// <param name="s">The s.</param>
        /// <param name="l">The l.</param>
        public HslColor(float h, float s, float l)
        {
            _CheckComponent(h, "H");
            _CheckComponent(s, "S");
            _CheckComponent(l, "L");
            _h = h;
            _s = s;
            _l = l;
        }

        #endregion

        #region Private members

        private static void _CheckComponent(float value, string name)
        {
            if ((value < MinComponentValue)
                 || (value > MaxComponentValue))
            {
                throw new ArgumentOutOfRangeException(name);
            }
        }

        #endregion

        #region Object members

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">Another object to compare to.</param>
        /// <returns>
        /// <c>true</c> if obj and this instance are the same type
        /// and represent the same value; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals
            (
                object obj
            )
        {
            if (!(obj is HslColor))
            {
                return false;
            }
            var other = (HslColor)obj;

            // ReSharper disable CompareOfFloatsByEqualityOperator
            return (H == other.H) && (L == other.L) && (S == other.S); //-V3024
            // ReSharper restore CompareOfFloatsByEqualityOperator
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        public override int GetHashCode()
        {
            var result = H.GetHashCode();
            result = 29 * result + L.GetHashCode();
            result = 29 * result + S.GetHashCode();

            return result;
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents
        /// the current <see cref="T:System.Object" />.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents
        /// the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return string.Format("H: {0}; S: {1}; L: {2}", H, S, L);
        }

        #endregion

    } // struct HslColor

} // namespace AM.Drawing
