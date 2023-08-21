// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConvertClosureToMethodGroup
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo

/* Program.cs -- точка входа в программу
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Text;

using AM.StableDiffusion.Automatic;

#endregion

#nullable enable

namespace SDcli;

/// <summary>
/// Вся логика программы в одном классе.
/// </summary>
internal static class Program
{
    /// <summary>
    /// Точка входа в программу.
    /// </summary>
    private static int Main
        (
            string[] args
        )
    {
        Encoding.RegisterProvider (CodePagesEncodingProvider.Instance);

        if (args.Length < 1)
        {
            return 0;
        }

        try
        {
            return args[0] switch
            {
                "txt2img" => Txt2Img (args),
                _ => ShowHelp()
            };
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine (exception);

            return -1;
        }
    }

    private static AutomaticClient CreateClient
        (
            string[] args
        )
    {
        return new AutomaticClient();
    }

    private static int ShowHelp()
    {
        return 0;
    }

    private static int Txt2Img
        (
            string[] args
        )
    {
        var client = CreateClient (args);
        var request = new TextToImageRequest
        {
            Prompt = "a photo of a beautiful 18 years old ukrainian girl posing outdoors",
            SamplerName = "DPM++ 2M Karras",
            Steps = 20,
            Iterations = 1,
            BatchSize = 1,
            CfgScale = 7,
            Width = 512,
            Height = 768
        };

        var response = client.TextToImageAsync (request).GetAwaiter().GetResult();
        client.SaveImages (response?.Images);

        return 0;
    }
}
