// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable StringLiteralTypo

using System;
using System.IO;
using System.Linq;

#nullable enable

namespace DrawingTests
{
    public class CommonUnitTest
    {
        protected static void ShowDifference
            (
                string expected,
                string actual
            )
        {
            int index = 0;
            while (index < expected.Length && index < actual.Length)
            {
                if (expected[index] != actual[index])
                {
                    break;
                }

                ++index;
            }

            if (index == expected.Length && index == actual.Length)
            {
                return;
            }

            if (expected.Length != actual.Length)
            {
                Console.WriteLine($"Expected length={expected.Length}, actual length={actual.Length}");
            }
            Console.WriteLine($"Difference at index {index}");
            Console.WriteLine($"Expected: {expected.Substring(index)}");
            Console.WriteLine($"Actual  : {actual.Substring(index)}");
        }
    }
}
