using AM.Text;

using static System.Console;

// ReSharper disable CheckNamespace
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

namespace CoreExamples
{
    static class TextNavigatorExamples
    {
        static void ReadFrom()
        {
            var text = "У попа (была) собака, он её любил.";
            var navigator = new ValueTextNavigator(text);

            navigator.SkipTo('(');
            var word = navigator.ReadFrom('(', ')');
            WriteLine(word.ToString());
        }

        static void ReadEscapedUntil()
        {
            var text = "У попа\\tбыла \\tсобака! Он её любил";
            var navigator = new ValueTextNavigator(text);

            var part = navigator.ReadEscapedUntil('\\', '!');
            WriteLine(part);
        }

        static void ReadWord()
        {
            var text = "У попа была собака, он её любил.";
            var navigator = new ValueTextNavigator(text);

            while (true)
            {
                var word = navigator.ReadWord();
                if (word.IsEmpty)
                {
                    break;
                }

                Write($"{word.ToString()}, ");

                if (!navigator.SkipNonWord())
                {
                    break;
                }
            }

            WriteLine();
        }

        public static void All()
        {
            ReadWord();
            ReadFrom();
            ReadEscapedUntil();
        }
    }
}
