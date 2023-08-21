// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* Program.cs -- точка входа в программу
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Text;

using AM.StableDiffusion.Automatic;

#endregion

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
                "checkpoints"
                    or "list-checkpoints" => Checkpoints (args),
                "create-embedding" => CreateEmbedding (args),
                "embeddings" => Embeddings (args),
                "get-checkpoint"
                    or "current-checkpoint" => GetCheckpoint (args),
                "get-options" => GetOptions (args),
                "interrupt" => Interrupt (args),
                "memory" => Memory (args),
                "progress" => Progress (args),
                "samplers" => Samplers (args),
                "set-checkpoint" => SetCheckpoint (args),
                "skip" => Skip (args),
                "txt2img" => Txt2Img (args),
                "vaes" => Vaes (args),
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
        var baseUrl = AutomaticClient.DefaultUrl;
        var outputPath = "output";
        for (var i = 0; i < args.Length; i++)
        {
            var arg = args[i];
            switch (arg)
            {
                case "--url":
                    baseUrl = args[++i];
                    break;

                case "--output":
                    outputPath = args[++i];
                    break;
            }
        }

        var result = new AutomaticClient (baseUrl)
        {
            OutputPath = outputPath
        };

        return result;
    }

    private static int ShowHelp()
    {
        // TODO implement

        return 0;
    }

    private static int Checkpoints
        (
            string[] args
        )
    {
        var client = CreateClient (args);
        var response = client.ListCheckpointsAsync().GetAwaiter().GetResult();
        if (response is not null)
        {
            Console.WriteLine (response);
        }

        return 0;
    }

    private static int CreateEmbedding
        (
            string[] args
        )
    {
        var client = CreateClient (args);
        var request = CreateEmbeddingRequest.FromCommandLine (args);
        client.CreateEmbeddingAsync (request).GetAwaiter().GetResult();

        return 0;
    }

    private static int Embeddings
        (
            string[] args
        )
    {
        var client = CreateClient (args);
        var response = client.ListEmbeddingsAsync().GetAwaiter().GetResult();
        if (response?.Loaded is { } embeddings)
        {
            foreach (var embedding in embeddings)
            {
                Console.WriteLine (embedding);
            }
        }

        return 0;
    }

    private static int GetCheckpoint
        (
            string[] args
        )
    {
        var client = CreateClient (args);
        var response = client.GetCurrentCheckpointAsync().GetAwaiter().GetResult();
        if (response is not null)
        {
            Console.WriteLine (response);
        }

        return 0;
    }

    private static int GetOptions
        (
            string[] args
        )
    {
        var client = CreateClient (args);
        var options = client.GetOptionsAsync().GetAwaiter().GetResult();
        Console.WriteLine (options);

        return 0;
    }

    private static int Interrupt
        (
            string[] args
        )
    {
        var client = CreateClient (args);
        client.InterruptAsync().GetAwaiter().GetResult();

        return 0;
    }

    private static int Memory
        (
            string[] args
        )
    {
        var client = CreateClient (args);
        var memory = client.GetMemoryStatAsync().GetAwaiter().GetResult();
        Console.WriteLine (memory);

        return 0;
    }

    private static int Progress
        (
            string[] args
        )
    {
        var client = CreateClient (args);
        var progress = client.GetProgressAsync().GetAwaiter().GetResult();
        Console.WriteLine (progress);

        return 0;
    }

    private static int Samplers
        (
            string[] args
        )
    {
        var client = CreateClient (args);
        var response = client.ListSamplersAsync().GetAwaiter().GetResult();
        if (response is not null)
        {
            foreach (var sampler in response)
            {
                Console.WriteLine (sampler);
            }
        }

        return 0;
    }

    private static int SetCheckpoint
        (
            string[] args
        )
    {
        string? checkpointName = null;
        foreach (var name in args)
        {
            checkpointName = name;
        }

        if (string.IsNullOrWhiteSpace (checkpointName))
        {
            Console.WriteLine ("No checkpoint name specified");
            return 0;
        }

        var client = CreateClient (args);
        client.SetModelAsync(checkpointName).GetAwaiter().GetResult();

        return 0;
    }


    private static int Skip
        (
            string[] args
        )
    {
        var client = CreateClient (args);
        client.SkipAsync().GetAwaiter().GetResult();

        return 0;
    }

    private static int Txt2Img
        (
            string[] args
        )
    {
        var client = CreateClient (args);
        var request = TextToImageRequest.FromCommandLine (args);
        var checkpoint = request.Checkpoint;
        if (!string.IsNullOrEmpty (checkpoint))
        {
            client.SetModelAsync (checkpoint).GetAwaiter().GetResult();
        }

        var response = client.TextToImageAsync (request).GetAwaiter().GetResult();
        client.SaveImages (response?.Images);

        return 0;
    }

    private static int Vaes
        (
            string[] args
        )
    {
        var client = CreateClient (args);
        var response = client.ListVaesAsync().GetAwaiter().GetResult();
        if (response is not null)
        {
            foreach (var vae in response)
            {
                Console.WriteLine (vae);
            }
        }

        return 0;
    }
}
