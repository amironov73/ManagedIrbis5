// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

#region Using directives

using System.Text;

using AM.SourceGeneration;

#endregion

namespace SourceGenerationTests;

internal partial class BinaryInclusionDemo
{
    // путь относительно корня проекта,
    // в нашем случае относительно C:/Projects/ManagedIrbis5/Source
    [BinaryInclusion ("Tests/SourceGenerationTests/Assets/somefile.bin")]
    internal partial byte[] GetBytes();

    public void DoInclusion()
    {
        var bytes = GetBytes();
        Console.WriteLine (Encoding.ASCII.GetString (bytes));
    }
}
