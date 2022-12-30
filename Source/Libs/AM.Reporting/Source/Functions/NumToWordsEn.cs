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
using System.Text;
using System.Collections;
using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Reporting.Functions
{
    internal class NumToWordsEn : NumToWordsBase
    {
        private static Dictionary<string, CurrencyInfo> currencyList;

        private static string[] fixedWords =
        {
            "", "one", "two", "three", "four", "five", "six",
            "seven", "eight", "nine", "ten", "eleven",
            "twelve", "thirteen", "fourteen", "fifteen",
            "sixteen", "seventeen", "eighteen", "nineteen"
        };

        private static string[] tens =
        {
            "", "ten", "twenty", "thirty", "forty", "fifty",
            "sixty", "seventy", "eighty", "ninety"
        };

        private static string[] hunds =
        {
            "", "one hundred", "two hundred", "three hundred", "four hundred",
            "five hundred", "six hundred", "seven hundred", "eight hundred", "nine hundred"
        };

        private static WordInfo thousands = new WordInfo ("thousand");
        private static WordInfo millions = new WordInfo ("million");
        private static WordInfo milliards = new WordInfo ("billion");
        private static WordInfo trillions = new WordInfo ("trillion");

        protected override string GetFixedWords (bool male, long value)
        {
            return fixedWords[value];
        }

        protected override string GetTen (bool male, long value)
        {
            return tens[value];
        }

        protected override string GetHund (bool male, long value)
        {
            return hunds[value / 100];
        }

        protected override WordInfo GetThousands()
        {
            return thousands;
        }

        protected override WordInfo GetMillions()
        {
            return millions;
        }

        protected override WordInfo GetMilliards()
        {
            return milliards;
        }

        protected override WordInfo GetTrillions()
        {
            return trillions;
        }

        protected override CurrencyInfo GetCurrency (string currencyName)
        {
            currencyName = currencyName.ToUpper();
            if (currencyName == "CAD")
            {
                currencyName = "USD";
            }

            return currencyList[currencyName];
        }

        protected override string GetZero()
        {
            return "zero";
        }

        protected override string GetMinus()
        {
            return "minus";
        }

        protected override string GetDecimalSeparator()
        {
            return "and ";
        }

        protected override string Get10_1Separator()
        {
            return "-";
        }

        protected override string Get100_10Separator()
        {
            return " and ";
        }

        static NumToWordsEn()
        {
            currencyList = new Dictionary<string, CurrencyInfo>();
            currencyList.Add ("USD", new CurrencyInfo (
                new WordInfo ("dollar", "dollars"),
                new WordInfo ("cent", "cents")));
            currencyList.Add ("EUR", new CurrencyInfo (
                new WordInfo ("euro", "euros"),
                new WordInfo ("cent", "cents")));
            currencyList.Add ("GBP", new CurrencyInfo (
                new WordInfo ("pound", "pounds"),
                new WordInfo ("penny", "pence")));
        }
    }
}
