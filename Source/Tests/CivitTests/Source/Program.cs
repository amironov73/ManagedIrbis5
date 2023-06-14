// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

#region Using directives

using System;
using System.Threading;

using AM.StableDiffusion.CivitAI;

#endregion

#nullable enable

namespace CivitTests;

internal static class Program
{
    private static void Main()
    {
        var client = new CivitClient();

        // var creators = client.GetCreators (limit: 5, query: "amironov");
        // if (creators?.Items is {} creatorItems)
        // {
        //     foreach (var creator in creatorItems)
        //     {
        //         Console.WriteLine (creator);
        //     }
        // }

        // var images = client.GetImages (postId: 297562);
        // if (images?.Items is { } imagesItems)
        // {
        //     foreach (var image in imagesItems)
        //     {
        //         Console.WriteLine (image);
        //         client.SaveImage (image, directoryToSave: "images");
        //     }
        // }
        //
        // var models = client.GetModels (username: "amironov73762");
        // if (models?.Items is { } modelsItems)
        // {
        //     foreach (var model in modelsItems)
        //     {
        //         Console.WriteLine (model);
        //         client.SaveModel (model, withImage: true, directoryToSave: "models");
        //     }
        // }

        // var tags = client.GetTag ("school");
        // if (tags?.Items is { } tagsItems)
        // {
        //     foreach (var tag in tagsItems)
        //     {
        //         Console.WriteLine (tag.Name);
        //         Console.WriteLine();
        //         var tagModels = client.GetModels (tag: tag.Name);
        //         if (tagModels?.Items is { } tagModelItems)
        //         {
        //             foreach (var tagModel in tagModelItems)
        //             {
        //                 Console.WriteLine (tagModel);
        //             }
        //         }
        //
        //         Console.WriteLine();
        //     }
        // }

        var deliberate = client.GetModel ("deliberate");
        if (deliberate is not null)
        {
            var images = client.EnumerateImages (modelId: deliberate.Id);
            foreach (var image in images)
            {
                Console.WriteLine (image.Id);
                client.SaveImage (image, "images");
                Thread.Sleep (50);
            }
        }
    }
}
