// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Reporting.Barcode.QRCode
{
    /*/// <summary>
    /// Contains conversion support elements such as classes, interfaces and static methods.
    /// </summary>*/
    internal class SupportClass
    {
        /*******************************/
        /*/// <summary>
        /// Performs an unsigned bitwise right shift with the specified number
        /// </summary>
        /// <param name="number">Number to operate on</param>
        /// <param name="bits">Ammount of bits to shift</param>
        /// <returns>The resulting number from the shift operation</returns>*/
        public static int URShift (int number, int bits)
        {
            if (number >= 0)
            {
                return number >> bits;
            }
            else
            {
                return (number >> bits) + (2 << ~bits);
            }
        }

        /*/// <summary>
        /// Performs an unsigned bitwise right shift with the specified number
        /// </summary>
        /// <param name="number">Number to operate on</param>
        /// <param name="bits">Ammount of bits to shift</param>
        /// <returns>The resulting number from the shift operation</returns>*/
        public static int URShift (int number, long bits)
        {
            return URShift (number, (int)bits);
        }

        /*/// <summary>
        /// Performs an unsigned bitwise right shift with the specified number
        /// </summary>
        /// <param name="number">Number to operate on</param>
        /// <param name="bits">Ammount of bits to shift</param>
        /// <returns>The resulting number from the shift operation</returns>*/
        public static long URShift (long number, int bits)
        {
            if (number >= 0)
            {
                return number >> bits;
            }
            else
            {
                return (number >> bits) + (2L << ~bits);
            }
        }

        /*/// <summary>
        /// Performs an unsigned bitwise right shift with the specified number
        /// </summary>
        /// <param name="number">Number to operate on</param>
        /// <param name="bits">Ammount of bits to shift</param>
        /// <returns>The resulting number from the shift operation</returns>*/
        public static long URShift (long number, long bits)
        {
            return URShift (number, (int)bits);
        }

        /*******************************/
        /*/// <summary>
        /// This method returns the literal value received
        /// </summary>
        /// <param name="literal">The literal to return</param>
        /// <returns>The received value</returns>*/
        public static long Identity (long literal)
        {
            return literal;
        }

        /*/// <summary>
        /// This method returns the literal value received
        /// </summary>
        /// <param name="literal">The literal to return</param>
        /// <returns>The received value</returns>*/
        public static ulong Identity (ulong literal)
        {
            return literal;
        }

        /*/// <summary>
        /// This method returns the literal value received
        /// </summary>
        /// <param name="literal">The literal to return</param>
        /// <returns>The received value</returns>*/
        public static float Identity (float literal)
        {
            return literal;
        }

        /*/// <summary>
        /// This method returns the literal value received
        /// </summary>
        /// <param name="literal">The literal to return</param>
        /// <returns>The received value</returns>*/
        public static double Identity (double literal)
        {
            return literal;
        }


        /*******************************/
        /*/// <summary>
        /// Receives a byte array and returns it transformed in an sbyte array
        /// </summary>
        /// <param name="byteArray">Byte array to process</param>
        /// <returns>The transformed array</returns>*/
        public static sbyte[] ToSByteArray (byte[] byteArray)
        {
            sbyte[] sbyteArray = null;
            if (byteArray != null)
            {
                sbyteArray = new sbyte[byteArray.Length];
                for (var index = 0; index < byteArray.Length; index++)
                    sbyteArray[index] = (sbyte)byteArray[index];
            }

            return sbyteArray;
        }
    }
}
