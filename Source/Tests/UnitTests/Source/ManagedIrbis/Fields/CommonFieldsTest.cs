using ManagedIrbis;

// ReSharper disable CheckNamespace
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable IdentifierTypo
// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable StringLiteralTypo

#nullable enable

namespace UnitTests.ManagedIrbis.Fields
{
    public class CommonFieldsTest
        : Common.CommonUnitTest
    {
        protected static Field Parse(int tag, string text)
        {
            var result = new Field { Tag = tag };
            result.DecodeBody(text);

            return result;
        }
    }
}
