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

// Based on RSDN RusCurrency class

using System;
using System.Collections.Generic;
using System.Text;

#endregion

#nullable enable

namespace AM.Reporting.Functions
{
    internal abstract class NumToWordsBase
    {
        #region Private Methods

        private string Str (decimal value, WordInfo senior, WordInfo junior)
        {
            var minus = false;
            if (value < 0)
            {
                value = -value;
                minus = true;
            }

            var n = (long)value;
            var remainder = (int)((value - n + 0.005m) * 100);
            if (remainder >= 100)
            {
                n++;
                remainder = 0;
            }

            var r = new StringBuilder();

            Str (n, senior, r);

            if (minus)
            {
                r.Insert (0, GetMinus() + " ");
            }

            if (junior != null)
            {
                r.Append (GetDecimalSeparator() + remainder.ToString ("00 "));
                r.Append (Case (remainder, junior));
            }

            r[0] = char.ToUpper (r[0]);
            return r.ToString();
        }

        #endregion

        #region Protected Methods

        protected abstract string GetFixedWords (bool male, long value);
        protected abstract string GetTen (bool male, long value);
        protected abstract string GetHund (bool male, long value);
        protected abstract WordInfo GetThousands();
        protected abstract WordInfo GetMillions();
        protected abstract WordInfo GetMilliards();
        protected abstract WordInfo GetTrillions();
        protected abstract CurrencyInfo GetCurrency (string currencyName);
        protected abstract string GetZero();
        protected abstract string GetMinus();

        protected virtual void Str (long value, WordInfo senior, StringBuilder result)
        {
            if (value == 0)
            {
                result.Append (GetZero() + " " + senior.many + " ");
            }
            else
            {
                if (value % 1000 != 0)
                {
                    result.Append (Str1000 (value, senior, 1));
                }
                else
                {
                    result.Append (" " + senior.many + " ");
                }

                value /= 1000;
                result.Insert (0, Str1000 (value, GetThousands(), 2));

                value /= 1000;
                result.Insert (0, Str1000 (value, GetMillions(), 3));

                value /= 1000;
                result.Insert (0, Str1000 (value, GetMilliards(), 4));

                value /= 1000;
                result.Insert (0, Str1000 (value, GetTrillions(), 5));
                result.Replace ("  ", " ");
            }
        }

        protected virtual string Str1000 (long value, WordInfo info, int counter)
        {
            var val = value % 1000;
            if (val == 0)
            {
                return "";
            }

            var r = new StringBuilder();

            // add hundred
            var hund = GetHund (info.male, val);
            if (hund != "")
            {
                r.Append (hund);
            }

            // decide whether to use the 100-10 separator or not
            var sep100_10 = Get100_10Separator();
            if (value < 1000 && hund == "")
            {
                sep100_10 = "";
            }

            val = val % 100;
            if (val < GetFixedWordsCount())
            {
                // val is less than fixed words count (usually 20), get fixed words
                var frac20 = GetFixedWords (info.male, val);
                if (frac20 != "")
                {
                    r.Append (sep100_10 + frac20);
                }
            }
            else
            {
                // val is greater than fixed words count (usually 20), get tens separately
                var ten = GetTen (info.male, val / 10);
                var frac10 = GetFixedWords (info.male, val % 10);

                // decide whether to use 10-1 separator or not
                if (ten != "" && frac10 != "")
                {
                    r.Append (sep100_10 + ten + Get10_1Separator() + frac10);
                }
                else if (ten != "")
                {
                    r.Append (sep100_10 + ten);
                }
                else if (frac10 != "")
                {
                    r.Append (sep100_10 + frac10);
                }
            }

            // add currency/group word
            r.Append (" ");
            r.Append (Case (value, info));

            // make the result starting with letter and ending with space
            if (r.Length != 0)
            {
                r.Append (" ");
            }

            return r.ToString().TrimStart (' ');
        }

        protected virtual int GetFixedWordsCount()
        {
            return 20;
        }

        protected virtual string GetDecimalSeparator()
        {
            return "";
        }

        protected virtual string Get10_1Separator()
        {
            return " ";
        }

        protected virtual string Get100_10Separator()
        {
            return " ";
        }

        protected virtual string Case (long value, WordInfo info)
        {
            if (value % 100 == 1)
            {
                return info.one;
            }

            return info.many;
        }

        #endregion

        #region Public Methods

        public string ConvertCurrency (decimal value, string currencyName)
        {
            try
            {
                var currency = GetCurrency (currencyName);
                return Str (value, currency.senior, currency.junior);
            }
            catch (KeyNotFoundException e)
            {
                throw new Exception (
                    $"Currency \"{currencyName}\" is not defined in the \"{GetType().Name}\" converter.");
            }
            catch (NotImplementedException e)
            {
                throw new Exception ("Method " + e.TargetSite.ToString() + " wasn't implemented");
            }
            catch (Exception e)
            {
                throw new Exception ("There is an exception - " + e.ToString());
            }
        }

        public string ConvertNumber (decimal value, bool male, string one, string two, string many)
        {
            return Str (value, new WordInfo (male, one, two, many), null);
        }

        public string ConvertNumber (decimal value, bool male,
            string seniorOne, string seniorTwo, string seniorMany,
            string juniorOne, string juniorTwo, string juniorMany)
        {
            return Str (value,
                new WordInfo (male, seniorOne, seniorTwo, seniorMany),
                new WordInfo (male, juniorOne, juniorTwo, juniorMany));
        }

        #endregion
    }

    internal class WordInfo
    {
        public bool male;
        public string one;
        public string two;
        public string many;

        public WordInfo (bool male, string one, string two, string many)
        {
            this.male = male;
            this.one = one;
            this.two = two;
            this.many = many;
        }

        public WordInfo (string one, string many)
            : this (true, one, many, many)
        {
        }

        public WordInfo (string all)
            : this (true, all, all, all)
        {
        }
    }

    internal class CurrencyInfo
    {
        public WordInfo senior;
        public WordInfo junior;

        public CurrencyInfo (WordInfo senior, WordInfo junior)
        {
            this.senior = senior;
            this.junior = junior;
        }
    }
}
