using ManagedIrbis;

// ReSharper disable CheckNamespace
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable IdentifierTypo
// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable StringLiteralTypo

#nullable enable

namespace UnitTests.ManagedIrbis.Magazines
{
    public class CommonMagazineTest
    {
        protected static Field Parse(int tag, string text)
        {
            var result = new Field { Tag = tag };
            result.Decode(text);

            return result;
        }
    }
}
