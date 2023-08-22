// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* AutomaticModule.cs -- модуль для Барсика
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;

using AM.Scripting.Barsik;

using JetBrains.Annotations;

using static AM.Scripting.Barsik.Builtins;

#endregion

#nullable enable

namespace AM.StableDiffusion.Automatic;

/// <summary>
/// Модуль для Барсика
/// </summary>
[PublicAPI]
public sealed class AutomaticModule
    : IBarsikModule
{
    #region Constants

    /// <summary>
    /// Имя дефайна, хранящего текущее подключение.
    /// </summary>
    public const string ConnectionDefineName = "automatic";

    #endregion

    #region Properties

    /// <summary>
    /// Реестр экспортированных функций.
    /// </summary>
    public static readonly Dictionary<string, FunctionDescriptor> Registry = new ()
    {
        { "change_checkpoint", new FunctionDescriptor ("change_checkpoint", ChangeCheckpoint) },
        { "connect", new FunctionDescriptor ("connect", Connect) },
        { "current_checkpoint", new FunctionDescriptor ("current_checkpoint", GetCurrentCheckpoint) },
        { "get_options", new FunctionDescriptor ("get_options", GetOptions) },
        { "get_progress", new FunctionDescriptor ("get_progress", GetProgress) },
        { "interrupt", new FunctionDescriptor ("interrupt", Interrupt) },
        { "list_checkpoints", new FunctionDescriptor ("list_checkpoints", ListCheckpoints) },
        { "list_embeddings", new FunctionDescriptor ("list_embeddings", ListEmbeddings) },
        { "list_samplers", new FunctionDescriptor ("list_samplers", ListSamplers) },
        { "memory_stat", new FunctionDescriptor ("memory_stat", GetMemoryStat) },
        { "skip", new FunctionDescriptor ("skip", Skip) },
        { "text_to_image", new FunctionDescriptor ("text_to_image", TextToImage) },
        { "train_embedding", new FunctionDescriptor ("train_embedding", TrainEmbedding) },

    };

    #endregion

    #region Private members

    private static dynamic CreateConnection
        (
            Context context,
            string? connectionString
        )
    {
        var result = string.IsNullOrEmpty (connectionString)
            ? new AutomaticClient()
            : new AutomaticClient (connectionString);
        context.SetVariable (ConnectionDefineName, result);

        return result;
    }

    /// <summary>
    /// Отыскиваем текущее подключение к Automatic.
    /// Ругаемся, если не находим или находим что-то не то.
    /// </summary>
    internal static bool TryGetConnection
        (
            Context context,
            out AutomaticClient connection,
            bool verbose = true
        )
    {
        connection = null!;

        if (!context.TryGetVariable (ConnectionDefineName, out var value))
        {
            if (verbose)
            {
                context.Error.WriteLine ($"Variable {ConnectionDefineName} not found");
            }

            return false;
        }

        if (value is AutomaticClient client)
        {
            connection = client;
            return true;
        }

        if (verbose)
        {
            context.Error.WriteLine ($"Bad value of {ConnectionDefineName}: {value}");
        }

        return false;
    }

    #endregion

    #region Public methods

    public static dynamic? ChangeCheckpoint
        (
            Context context,
            dynamic?[] args
        )
    {
        var newCheckpoint = Compute (context, args, 0) as string;
        if (string.IsNullOrEmpty (newCheckpoint))
        {
            return null;
        }

        if (!TryGetConnection (context, out var connection))
        {
            return null;
        }

        connection.SetModelAsync (newCheckpoint).GetAwaiter().GetResult();

        return newCheckpoint;
    }

    /// <summary>
    /// Подключение к Automatic1111.
    /// </summary>
    public static dynamic Connect
        (
            Context context,
            dynamic?[] args
        )
    {
        if (!TryGetConnection (context, out var connection, false))
        {
            // если соединения ещё нет, создаем его
            var connectionString = Compute (context, args, 0) as string;
            connection = CreateConnection (context, connectionString);
        }

        return true;
    }

    /// <summary>
    /// Получение имени текущего чекпоинта.
    /// </summary>
    public static dynamic? GetCurrentCheckpoint
        (
            Context context,
            dynamic?[] args
        )
    {
        if (!TryGetConnection (context, out var connection))
        {
            return null;
        }

        return connection.GetCurrentCheckpointAsync().GetAwaiter().GetResult();
    }

    /// <summary>
    /// Получение информации о расходовании памяти.
    /// </summary>
    public static dynamic? GetMemoryStat
        (
            Context context,
            dynamic?[] args
        )
    {
        if (!TryGetConnection (context, out var connection))
        {
            return null;
        }

        return connection.GetMemoryStatAsync().GetAwaiter().GetResult();
    }

    /// <summary>
    /// Получение текущих опций.
    /// </summary>
    public static dynamic? GetOptions
        (
            Context context,
            dynamic?[] args
        )
    {
        if (!TryGetConnection (context, out var connection))
        {
            return null;
        }

        return connection.GetOptionsAsync().GetAwaiter().GetResult();
    }

    /// <summary>
    /// Получение прогресса текущей операции.
    /// </summary>
    public static dynamic? GetProgress
        (
            Context context,
            dynamic?[] args
        )
    {
        if (!TryGetConnection (context, out var connection))
        {
            return null;
        }

        return connection.GetProgressAsync().GetAwaiter().GetResult();
    }

    /// <summary>
    /// Прерывание текуще операции.
    /// </summary>
    public static dynamic? Interrupt
        (
            Context context,
            dynamic?[] args
        )
    {
        if (!TryGetConnection (context, out var connection))
        {
            return null;
        }

        connection.InterruptAsync().GetAwaiter().GetResult();

        return null;
    }

    /// <summary>
    /// Получение списка текстовых инверсий.
    /// </summary>
    public static dynamic? ListEmbeddings
        (
            Context context,
            dynamic?[] args
        )
    {
        if (!TryGetConnection (context, out var connection))
        {
            return null;
        }

        return connection.ListEmbeddingsAsync().GetAwaiter().GetResult();
    }

    /// <summary>
    /// Получение списка чекпоинтов.
    /// </summary>
    public static dynamic? ListCheckpoints
        (
            Context context,
            dynamic?[] args
        )
    {
        if (!TryGetConnection (context, out var connection))
        {
            return null;
        }

        return connection.ListCheckpointsAsync().GetAwaiter().GetResult();
    }

    /// <summary>
    /// Получение списка сэмплеров.
    /// </summary>
    public static dynamic? ListSamplers
        (
            Context context,
            dynamic?[] args
        )
    {
        if (!TryGetConnection (context, out var connection))
        {
            return null;
        }

        return connection.ListSamplersAsync().GetAwaiter().GetResult();
    }

    /// <summary>
    /// Пропуск текущей генерации.
    /// </summary>
    public static dynamic? Skip
        (
            Context context,
            dynamic?[] args
        )
    {
        if (!TryGetConnection (context, out var connection))
        {
            return null;
        }

        connection.SkipAsync().GetAwaiter().GetResult();

        return null;
    }

    /// <summary>
    /// Генерация изображения по тексту.
    /// </summary>
    public static dynamic? TextToImage
        (
            Context context,
            dynamic?[] args
        )
    {
        if (Compute (context, args, 0) is not TextToImageRequest payload)
        {
            return null;
        }

        if (!TryGetConnection (context, out var connection))
        {
            return null;
        }

        var response = connection.TextToImageAsync (payload).GetAwaiter().GetResult();
        if (response is null)
        {
            return response;
        }

        return AutomaticClient.TextToBytes (response.Images?.FirstOrDefault());
    }

    /// <summary>
    /// Тренировка текстовой инверсии.
    /// </summary>
    public static dynamic? TrainEmbedding
        (
            Context context,
            dynamic?[] args
        )
    {
        if (Compute (context, args, 0) is not TrainEmbeddingRequest payload)
        {
            return null;
        }

        if (!TryGetConnection (context, out var connection))
        {
            return null;
        }

        return connection.TrainEmbeddingAsync (payload).GetAwaiter().GetResult();
    }


    #endregion

    #region IBarsikModule members

    /// <inheritdoc cref="IBarsikModule.Description"/>
    public string Description => "Automatic1111";

    /// <inheritdoc cref="IBarsikModule.Version"/>
    public Version Version { get; } = new (1, 0);

    /// <inheritdoc cref="IBarsikModule.AttachModule"/>
    public bool AttachModule
        (
            Interpreter interpreter
        )
    {
        Sure.NotNull (interpreter);

        var context = interpreter.Context.ThrowIfNull();
        foreach (var descriptor in Registry)
        {
            context.Functions[descriptor.Key] = descriptor.Value;
        }

        var assembly = typeof (AutomaticModule).Assembly;
        StdLib.LoadAssembly (context, new dynamic?[] { assembly.GetName().Name });
        StdLib.Use (context, new dynamic?[] { "Automatic1111" });

        return true;
    }

    /// <inheritdoc cref="IBarsikModule.DetachModule"/>
    public void DetachModule
        (
            Interpreter interpreter
        )
    {
        Sure.NotNull (interpreter);

        var context = interpreter.Context.ThrowIfNull();
        interpreter.ExternalCodeHandler = null;
        foreach (var descriptor in Registry)
        {
            context.Functions.Remove (descriptor.Key);
        }
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return Description;
    }

    #endregion
}
