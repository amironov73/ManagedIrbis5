using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM.Runtime;

namespace SourceGenerationTests
{
    internal partial class Part
        : IHandmadeSerializable
    {
        public string? Name { get; set; }
        public string? Description { get; set; }

        public void RestoreFromStream(BinaryReader reader)
        {
            throw new NotImplementedException();
        }

        [Serializer]
        public partial void SaveToStream (BinaryWriter writer);
    }
}
