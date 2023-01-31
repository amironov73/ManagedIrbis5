// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConvertIfStatementToConditionalTernaryExpression
// ReSharper disable HeapView.BoxingAllocation
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* StdLib.cs -- стандартная библиотека
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text;

using AM.Configuration;
using AM.IO;
using AM.Kotik.Barsik.Ast;
using AM.Text;

using Microsoft.Extensions.Caching.Memory;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using static AM.Kotik.Barsik.Builtins;

#endregion

#nullable enable

namespace AM.Kotik.Barsik;

/// <summary>
/// Стандартная библиотека (включая небезопасные функции).
/// </summary>
public sealed class StdLib
    : IBarsikModule
{
    #region Constants

    /// <summary>
    /// Имя дефайна, хранящего кэш.
    /// </summary>
    public const string CacheDefineName = "$cache";

    #endregion

    #region Properties

    /// <summary>
    /// Реестр стандартных функций.
    /// </summary>
    public static readonly Dictionary<string, FunctionDescriptor> Registry = new ()
    {
        { "array", new FunctionDescriptor ("array", Array_) },
        // { "bon_decode", new FunctionDescriptor ("bon_decode", BonDecode) },
        { "call", new FunctionDescriptor ("call", CallAnyMethod) },
        { "chdir", new FunctionDescriptor ("chdir", ChangeDirectory) },
        { "combine_path", new FunctionDescriptor ("combine_path", CombinePath) },
        { "dir_exists", new FunctionDescriptor ("dir_exists", DirectoryExists) },
        { "dirname", new FunctionDescriptor ("dirname", DirectoryName) },
        { "eval", new FunctionDescriptor ("eval", Evaluate) },
        { "exec", new FunctionDescriptor ("exec", Execute) },
        { "fflush", new FunctionDescriptor ("flush", Fflush) },
        { "fgets", new FunctionDescriptor ("fgets", Fgets) },
        { "file_exists", new FunctionDescriptor ("file_exists", FileExists) },
        { "file_get_contents", new FunctionDescriptor ("file_get_contents", FileGetContents) },
        { "file_put_contents", new FunctionDescriptor ("file_put_contents", FilePutContents) },
        { "fopen", new FunctionDescriptor ("fopen", Fopen) },
        { "fputs", new FunctionDescriptor ("fputs", Fputs) },
        { "fread", new FunctionDescriptor ("fread", Fread) },
        { "feek", new FunctionDescriptor ("fseek", Fseek) },
        { "ftell", new FunctionDescriptor ("ftell", Ftell) },
        { "fullpath", new FunctionDescriptor ("fullpath", FullPath) },
        { "fwrite", new FunctionDescriptor ("fwrite", Fwrite) },
        { "get_cache", new FunctionDescriptor ("get_cache", GetCache) },
        { "getcwd", new FunctionDescriptor ("getcwd", GetCurrentDirectory) },
        { "host", new FunctionDescriptor ("host", Host) },
        { "require", new FunctionDescriptor ("require", Include) },
        { "join", new FunctionDescriptor ("join", Join) },
        { "json_decode", new FunctionDescriptor ("json_decode", JsonDecode) },
        { "json_encode", new FunctionDescriptor ("json_encode", JsonEncode) },
        { "load", new FunctionDescriptor ("load", LoadAssembly) },
        { "module", new FunctionDescriptor ("module", LoadModule) },
        { "number_text", new FunctionDescriptor ("number_text", NumberText_) },
        { "protect", new FunctionDescriptor ("protect", Protect) },
        { "put_cache", new FunctionDescriptor ("put_cache", PutCache) },
        { "readdir", new FunctionDescriptor ("readdir", ReadDirectory) },
        { "remove", new FunctionDescriptor ("remove", RemoveFile) },
        { "rename", new FunctionDescriptor ("rename", RenameFile) },
        { "subscribe", new FunctionDescriptor ("subscribe", Subscribe) },
        { "system", new FunctionDescriptor ("system", System_) },
        { "tmpfile", new FunctionDescriptor ("tmpfile", TemporaryFile) },
        { "type", new FunctionDescriptor ("type", Type_) },
        { "unprotect", new FunctionDescriptor ("unprotect", Unrotect) },
        { "unsubscribe", new FunctionDescriptor ("unsubscribe", Unsubscribe) },
        { "use", new FunctionDescriptor ("use", Use) },
    };

    #endregion

    #region Private members

    /// <summary>
    /// Отыскиваем кэш.
    /// Ругаемся, если не находим или находим что-то не то.
    /// </summary>
    private static bool TryGetCache
        (
            Context context,
            out IMemoryCache cache,
            bool verbose = true
        )
    {
        cache = null!;

        if (!context.TryGetVariable (CacheDefineName, out var value))
        {
            if (verbose)
            {
                context.Error.WriteLine ($"Variable {CacheDefineName} not found");
            }
            return false;
        }

        if (value is IMemoryCache memoryCache)
        {
            cache = memoryCache;
            return true;
        }

        if (verbose)
        {
            context.Error.WriteLine ($"Bad value of {CacheDefineName}: {value}");
        }

        return false;
    }

    private static bool _Include
        (
            Context context,
            string fileName
        )
    {
        var topContext = context.GetTopContext();
        var interpreter = topContext.Interpreter.ThrowIfNull();
        var tokenizer = interpreter.Tokenizer;
        if (!topContext._inclusions.TryGetValue (fileName, out var program))
        {
            if (!File.Exists (fileName))
            {
                return false;
            }

            var sourceCode = File.ReadAllText (fileName);
            program = interpreter.Grammar.ParseProgram (sourceCode, tokenizer);
            topContext._inclusions[fileName] = program;
        }

        // TODO отрабатывать ExitException
        interpreter.Execute (program, context);

        return true;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Создание массива.
    /// </summary>
    public static dynamic? Array_
        (
            Context context,
            dynamic?[] args
        )
    {
        if (args.Length == 0)
        {
            return Array.Empty<object>();
        }

        var length = Convert.ToInt32 (Compute (context, args, 0));
        if (length <= 0)
        {
            return Array.Empty<object>();
        }

        var type = typeof (object);
        var typeName = Compute (context, args, 1) as string;
        if (!string.IsNullOrEmpty (typeName))
        {
            type = context.FindType (typeName);
            if (type is null)
            {
                context.Error.WriteLine ($"Can't find type {typeName}");
                return null;
            }
        }

        return Array.CreateInstance (type, length);
    }

    // /// <summary>
    // /// Декодирование BON-объекта.
    // /// </summary>
    // public static dynamic? BonDecode
    //     (
    //         Context context,
    //         dynamic?[] args
    //     )
    // {
    //     var bon = ComputeAll (context, args);
    //     if (string.IsNullOrWhiteSpace (bon))
    //     {
    //         return null;
    //     }
    //
    //     var interpreter = context.Interpreter;
    //     if (interpreter is null)
    //     {
    //         context.Error.WriteLine ("Interpreter is null");
    //         return null;
    //     }
    //
    //     dynamic? value;
    //     var program = Grammar.ParseProgram (bon);
    //     if (program.Statements.Count == 1)
    //     {
    //         var expression = interpreter.Evaluate (bon);
    //         value = expression.Compute (context);
    //     }
    //     else
    //     {
    //         // изготавливаем укороченный вариант программы
    //         var shortProgram = new ProgramNode
    //             (
    //                 program.Statements.SkipLast (1)
    //             );
    //         var last = program.Statements.Last() as ExpressionNode;
    //         if (last is null)
    //         {
    //             // последний стейтмент должен быть выражением
    //             context.Error.WriteLine ("Last statement must be expression");
    //             return null;
    //         }
    //
    //         interpreter.Execute (shortProgram, context);
    //         value = last.Expression.Compute (context);
    //     }
    //
    //     return value;
    // }

    /// <summary>
    /// Динамический вызов произвольного метода.
    /// </summary>
    public static dynamic? CallAnyMethod
        (
            Context context,
            dynamic?[] args
        )
    {
        if (args.Length < 2)
        {
            return null;
        }

        // первый аргумент - экземпляр или класс
        var target = Compute (context, args, 0);
        if (target is null)
        {
            return null;
        }

        if (target is string typeName)
        {
            target = context.FindType (typeName);
            if (target is null)
            {
                context.Error.WriteLine ($"Can't find type {typeName}");
                return null;
            }
        }

        var methodName = Compute (context, args, 1) as string;
        if (string.IsNullOrEmpty (methodName))
        {
            return null;
        }

        var preparedArgs = new List<object?>();
        for (var i = 2; i < args.Length; i++)
        {
            var one = Compute (context, args, i);
            preparedArgs.Add (one);
        }

        return KotikUtility.CallAnyMethod (context, target, methodName, preparedArgs.ToArray());
    }

    /// <summary>
    /// Изменение текущей директории.
    /// </summary>
    public static dynamic? ChangeDirectory
        (
            Context context,
            dynamic?[] args
        )
    {
        var path = ComputeAll (context, args);
        if (string.IsNullOrWhiteSpace (path))
        {
            return null;
        }

        path = path.Trim();
        try
        {
            Environment.CurrentDirectory = path;
        }
        catch (Exception exception)
        {
            context.Error.WriteLine ($"Error changing directory: {exception.Message}");
        }

        return Environment.CurrentDirectory;
    }

    /// <summary>
    /// Создание пути по его компонентам.
    /// </summary>
    public static dynamic? CombinePath
        (
            Context context,
            dynamic?[] args
        )
    {
        var components = new List<string>();
        for (var i = 0; i < args.Length; i++)
        {
            var name = Compute (context, args, i) as string;
            if (!string.IsNullOrWhiteSpace (name))
            {
                name = name.Trim();
                if (!string.IsNullOrEmpty (name))
                {
                    components.Add (name);
                }
            }
        }

        if (components.Count == 0)
        {
            return null;
        }

        return Path.Combine (components.ToArray());
    }

    /// <summary>
    /// Проверка существования папки.
    /// </summary>
    public static dynamic DirectoryExists
        (
            Context context,
            dynamic?[] args
        )
    {
        var fileName = Compute (context, args, 0) as string;
        if (string.IsNullOrEmpty (fileName))
        {
            return false;
        }

        return Directory.Exists (fileName);
    }

    /// <summary>
    /// Проверка существования папки.
    /// </summary>
    public static dynamic? DirectoryName
        (
            Context context,
            dynamic?[] args
        )
    {
        var fileName = Compute (context, args, 0) as string;
        if (string.IsNullOrEmpty (fileName))
        {
            return null;
        }

        return Path.GetDirectoryName (fileName);
    }

    /// <summary>
    /// Вычисление значения выражения.
    /// </summary>
    public static dynamic? Evaluate
        (
            Context context,
            dynamic?[] args
        )
    {
        try
        {
            var sourceCode = ComputeAll (context, args);
            if (string.IsNullOrWhiteSpace (sourceCode))
            {
                return null;
            }

            var topContext = context.GetTopContext();
            var interpreter = topContext.Interpreter.ThrowIfNull();
            var tokenizer = interpreter.Tokenizer;
            var expression = interpreter.Grammar.ParseExpression (sourceCode, tokenizer);
            var result = expression.Compute (context);

            return result;
        }
        catch (Exception exception)
        {
            context.Error.WriteLine (exception.Message);
        }

        return null;
    }

    /// <summary>
    /// Динамическое исполнение скрипта.
    /// </summary>
    public static dynamic? Execute //-V3009
        (
            Context context,
            dynamic?[] args
        )
    {
        try
        {
            var sourceCode = ComputeAll (context, args);
            if (string.IsNullOrWhiteSpace (sourceCode))
            {
                return null;
            }

            var topContext = context.GetTopContext();
            var interpreter = topContext.Interpreter.ThrowIfNull();
            var tokenizer = interpreter.Tokenizer;
            var program = interpreter.Grammar.ParseProgram (sourceCode, tokenizer);
            foreach (var statement in program.Statements)
            {
                statement.Execute (context);
            }
        }
        catch (Exception exception)
        {
            context.Error.WriteLine (exception.Message);
        }

        return null;
    }

    /// <summary>
    /// Сброс кэша на диск.
    /// </summary>
    public static dynamic? Fflush
        (
            Context context,
            dynamic?[] args
        )
    {
        if (Compute (context, args, 0) is Stream stream)
        {
            stream.Flush();
        }

        return null;
    }

    /// <summary>
    /// Чтение строки из потока.
    /// </summary>
    public static dynamic? Fgets
        (
            Context context,
            dynamic?[] args
        )
    {
        if (Compute (context, args, 0) is TextReader reader)
        {
            return reader.ReadLine();
        }

        return null;
    }

    /// <summary>
    /// Проверка существования файла.
    /// </summary>
    public static dynamic FileExists
        (
            Context context,
            dynamic?[] args
        )
    {
        var fileName = Compute (context, args, 0) as string;
        if (string.IsNullOrEmpty (fileName))
        {
            return false;
        }

        return File.Exists (fileName);
    }

    /// <summary>
    /// Чтение всего файла как строки.
    /// </summary>
    public static dynamic? FileGetContents
        (
            Context context,
            dynamic?[] args
        )
    {
        var fileName = Compute (context, args, 0) as string;
        if (string.IsNullOrEmpty (fileName))
        {
            return null;
        }

        try
        {
            if (fileName.StartsWith ("http:") || fileName.StartsWith ("https:"))
            {
                var client = new HttpClient();

                return client.GetByteArrayAsync (fileName).GetAwaiter().GetResult();
            }

            var encodingName = Compute (context, args, 1) as string;
            if (!string.IsNullOrEmpty (encodingName))
            {
                var encoding = Encoding.GetEncoding (encodingName);

                return File.ReadAllText (fileName, encoding);
            }

            return File.ReadAllText (fileName);
        }
        catch (Exception exception)
        {
            context.Error.WriteLine ($"Error reading file {fileName}: {exception.Message}");
        }

        return null;
    }

    /// <summary>
    /// Запись всего файла как одной большой строки.
    /// </summary>
    public static dynamic? FilePutContents //-V3009
        (
            Context context,
            dynamic?[] args
        )
    {
        var fileName = Compute (context, args, 0) as string;
        if (string.IsNullOrEmpty (fileName))
        {
            return null;
        }

        var contents = Compute (context, args, 1) as string;

        try
        {
            var encodingName = Compute (context, args, 2) as string;
            if (!string.IsNullOrEmpty (encodingName))
            {
                var encoding = Encoding.GetEncoding (encodingName);

                File.WriteAllText (fileName, contents, encoding);
            }
            else
            {
                File.WriteAllText (fileName, contents);
            }
        }
        catch (Exception exception)
        {
            context.Error.WriteLine ($"Error writing file {fileName}: {exception.Message}");
        }

        return null;
    }

    /// <summary>
    /// Открытие файла.
    /// </summary>
    public static dynamic? Fopen
        (
            Context context,
            dynamic?[] args
        )
    {
        var fileName = Compute (context, args, 0) as string;
        var mode = Compute (context, args, 1) as string;
        if (string.IsNullOrEmpty (fileName) || string.IsNullOrEmpty (mode))
        {
            return null;
        }

        var encoding = Compute (context, args, 2) as Encoding ?? Encoding.UTF8;
        if (mode.Contains ('t'))
        {
            // текстовый режим
            if (mode.Contains ('r'))
            {
                return new StreamReader (fileName, encoding);
            }

            var append = mode.Contains ('a');

            return new StreamWriter (fileName, append, encoding);
        }

        if (mode.Contains ('+'))
        {
            return File.Open (fileName, FileMode.Open, FileAccess.ReadWrite);
        }

        if (mode.Contains ('r'))
        {
            return File.OpenRead (fileName);
        }

        return File.OpenWrite (fileName);
    }

    /// <summary>
    /// Запись строки в поток.
    /// </summary>
    public static dynamic? Fputs
        (
            Context context,
            dynamic?[] args
        )
    {
        if (Compute (context, args, 0) is TextWriter writer
            && Compute (context, args, 1) is string text)
        {
            writer.Write (text);
        }

        return null;
    }

    /// <summary>
    /// Чтение из файла.
    /// </summary>
    public static dynamic? Fread
        (
            Context context,
            dynamic?[] args
        )
    {
        if (Compute (context, args, 0) is Stream stream
            && Compute (context, args, 1) is int count and > 0)
        {
            var buffer = new byte[count];
            var read = stream.Read (buffer, 0, count);
            if (read < 0)
            {
                return null;
            }

            if (read == 0)
            {
                return Array.Empty<byte>();
            }

            var result = new byte[read];
            Array.Copy (buffer, result, read);
        }

        return null;
    }

    /// <summary>
    /// Позиционирование в файле.
    /// </summary>
    public static dynamic? Fseek
        (
            Context context,
            dynamic?[] args
        )
    {
        if (Compute (context, args, 0) is Stream stream
            && Compute (context, args, 1) is long offset
            && Compute (context, args, 2) is int origin)
        {
            return stream.Seek (offset, (SeekOrigin) origin);
        }

        return null;
    }

    /// <summary>
    /// Получение текущей позиции в файле.
    /// </summary>
    public static dynamic? Ftell
        (
            Context context,
            dynamic?[] args
        )
    {
        if (Compute (context, args, 0) is Stream stream)
        {
            return stream.Position;
        }

        return null;
    }


    /// <summary>
    /// Доступ к хосту.
    /// </summary>
    public static dynamic? FullPath
        (
            Context context,
            dynamic?[] args
        )
    {
        var fileName = ComputeAll (context, args);
        if (string.IsNullOrEmpty (fileName))
        {
            return null;
        }

        return Path.GetFullPath (fileName);
    }

    /// <summary>
    /// Запись в файл.
    /// </summary>
    public static dynamic? Fwrite
        (
            Context context,
            dynamic?[] args
        )
    {
        if (Compute (context, args, 0) is Stream stream
            && Compute (context, args, 1) is ICollection<byte> bytes)
        {
            if (bytes is byte[] array)
            {
                stream.Write (array, 0, array.Length);
            }
            else
            {
                var buffer = new byte[bytes.Count];
                bytes.CopyTo (buffer, 0);
                stream.Write (buffer, 0, buffer.Length);
            }
        }

        return null;
    }

    /// <summary>
    /// Получение значения из кэша.
    /// </summary>
    public static dynamic? GetCache
        (
            Context context,
            dynamic?[] args
        )
    {
        if (!TryGetCache (context, out var cache))
        {
            return null;
        }

        if (Compute (context, args, 0) is { } key)
        {
            return cache.Get (key);
        }

        return null;
    }

    /// <summary>
    /// Получение текущей директории.
    /// </summary>
    public static dynamic GetCurrentDirectory
        (
            Context context,
            dynamic?[] args
        )
    {
        return Environment.CurrentDirectory;
    }

    /// <summary>
    /// Доступ к хосту.
    /// </summary>
    public static dynamic Host
        (
            Context context,
            dynamic?[] args
        )
    {
        return Magna.Host;
    }

    /// <summary>
    /// Загрузка указанного скрипта.
    /// </summary>
    public static dynamic? Include //-V3009
        (
            Context context,
            dynamic?[] args
        )
    {
        for (var i = 0; i < args.Length; i++)
        {
            var name = Compute (context, args, i) as string;
            if (!string.IsNullOrWhiteSpace (name))
            {
                name = name.Trim();
                var extension = Path.GetExtension (name);
                if (string.IsNullOrEmpty (extension))
                {
                    name += ".barsik";
                }

                if (!_Include (context, name)
                    && !Path.IsPathRooted (name))
                {
                    var interpreter = context.GetTopContext().Interpreter.ThrowIfNull();
                    foreach (var part in interpreter.Pathes)
                    {
                        var fullPath = Path.Combine (part, name);
                        if (_Include (context, fullPath))
                        {
                            return null;
                        }
                    }
                }

                // пытаемся загрузить файл рядом со скриптом
                if (context.TryGetVariable ("__DIR__", out var scriptDir))
                {
                    var fullPath = Path.Combine (scriptDir, name);
                    if (_Include (context, fullPath))
                    {
                        return null;
                    }
                }

            }
        }

        return null;
    }

    /// <summary>
    /// Декодирование JSON.
    /// </summary>
    public static dynamic? Join
        (
            Context context,
            dynamic?[] args
        )
    {
        if (args.Length < 2)
        {
            return null;
        }

        var separator = KotikUtility.ToString (Compute (context, args, 0));
        var list = new List<string>();
        for (var i = 1; i < args.Length; i++)
        {
            var value = Compute (context, args, i);
            if (value is string s)
            {
                list.Add (s);
            }
            else if (value is IList l)
            {
                foreach (var one in l)
                {
                    list.Add (KotikUtility.ToString (one));
                }
            }
            else
            {
                list.Add (KotikUtility.ToString (value));
            }
        }

        return string.Join (separator, list);
    }

    /// <summary>
    /// Декодирование JSON.
    /// </summary>
    public static dynamic? JsonDecode
        (
            Context context,
            dynamic?[] args
        )
    {
        var json = ComputeAll (context, args);
        if (string.IsNullOrWhiteSpace (json))
        {
            return null;
        }

        return JObject.Parse (json);
    }

    /// <summary>
    /// Коодирование JSON.
    /// </summary>
    public static dynamic JsonEncode
        (
            Context context,
            dynamic?[] args
        )
    {
        var obj = Compute (context, args, 0);

        return JsonConvert.SerializeObject (obj);
    }

    /// <summary>
    /// Загрузка указанной сборки.
    /// </summary>
    public static dynamic? LoadAssembly
        (
            Context context,
            dynamic?[] args
        )
    {
        var topContext = context.GetTopContext();
        Assembly? result = null;
        for (var i = 0; i < args.Length; i++)
        {
            var name = Compute (context, args, i) as string;
            if (!string.IsNullOrWhiteSpace (name))
            {
                result = topContext.LoadAssembly (name);
            }
        }

        return result;
    }

    /// <summary>
    /// Загрузка указанного модуля.
    /// </summary>
    public static dynamic? LoadModule
        (
            Context context,
            dynamic?[] args
        )
    {
        for (var i = 0; i < args.Length; i++)
        {
            var name = Compute (context, args, i) as string;
            if (!string.IsNullOrWhiteSpace (name))
            {
                name = name.Trim();
                context.LoadModule (name);
            }
        }

        return null;
    }

    /// <summary>
    /// Преобразование в смесь текста с числами.
    /// </summary>
    public static dynamic? NumberText_
        (
            Context context,
            dynamic?[] args
        )
    {
        var value = Compute (context, args, 0);
        if (value is string text)
        {
            return new NumberText (text);
        }
        else if (value is int int32)
        {
            return new NumberText().AppendChunk (int32);
        }
        else if (value is long int64)
        {
            return new NumberText().AppendChunk (int64);
        }

        return null;
    }

    /// <summary>
    /// Простейшая защита чувствительных данных вроде строк подключения.
    /// </summary>
    public static dynamic? Protect
        (
            Context context,
            dynamic?[] args
        )
    {
        var value = Compute (context, args, 0);
        if (value is not null)
        {
            var rawText = value.ToString();
            if (!string.IsNullOrEmpty (rawText))
            {
                return ConfigurationUtility.Protect (rawText);
            }
        }

        return null;
    }

    /// <summary>
    /// Помещение значения в кэш.
    /// </summary>
    public static dynamic? PutCache
        (
            Context context,
            dynamic?[] args
        )
    {
        if (args.Length < 2)
        {
            return null;
        }

        if (!TryGetCache (context, out var cache))
        {
            return null;
        }

        if (Compute (context, args, 0) is { } key)
        {
            var value = Compute (context, args, 1);
            if (Compute (context, args, 2) is int expiration)
            {
                cache.Set (key, value, TimeSpan.FromMilliseconds (expiration));
            }
            else
            {
                cache.Set (key, value);
            }
        }

        return null;
    }

    /// <summary>
    /// Чтение списка файлов в указанной директории.
    /// </summary>
    public static dynamic ReadDirectory
        (
            Context context,
            dynamic?[] args
        )
    {
        var path = Compute (context, args, 0) as string ?? ".";
        var pattern = (Compute (context, args, 1) as string).EmptyToNull();

        return pattern is null
            ? Directory.GetFileSystemEntries (path)
            : Directory.GetFileSystemEntries (path, pattern);
    }

    /// <summary>
    /// Удаление файлов.
    /// </summary>
    public static dynamic? RemoveFile
        (
            Context context,
            dynamic?[] args
        )
    {
        for (var i = 0; i < args.Length; i++)
        {
            var path = Compute (context, args, i) as string;
            if (!string.IsNullOrEmpty (path))
            {
                try
                {
                    File.Delete (path);
                }
                catch (Exception exception)
                {
                    context.Error.WriteLine ($"Error removing file {path}: {exception.Message}");
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Переименование файла.
    /// </summary>
    public static dynamic? RenameFile
        (
            Context context,
            dynamic?[] args
        )
    {
        var oldName = Compute (context, args, 0) as string;
        var newName = Compute (context, args, 1) as string;
        // var overwrite = KotikUtility.ToBoolean (Compute (context, args, 2));
        if (string.IsNullOrEmpty (oldName) || string.IsNullOrEmpty (newName))
        {
            return null;
        }

        try
        {
            if (!FileUtility.TryMove (oldName, newName))
            {
                throw new IOException ($"Can't move {oldName} to {newName}");
            }
        }
        catch (Exception exception)
        {
            context.Error.WriteLine ($"Error renaming {oldName} to {newName}: {exception.Message}");
            return 1;
        }

        return 0;
    }

    /// <summary>
    /// Подписка на событие объекта.
    /// </summary>
    public static dynamic Subscribe
        (
            Context context,
            dynamic?[] args
        )
    {
        if (args.Length < 3)
        {
            throw new BarsikException ("Can't subscribe");
        }

        var target = Compute (context, args, 0);
        if (target is null)
        {
            throw new BarsikException ("Can't subscribe");
        }

        var eventName = Compute (context, args, 1) as string;
        if (string.IsNullOrEmpty (eventName))
        {
            throw new BarsikException ("Can't subscribe");
        }

        var handler = Compute (context, args, 2);
        if (handler is LambdaNode lambda)
        {
            // лямбда-функция
            return new EventPad (context, target, eventName, lambda.Adapter);
        }

        if (handler is FunctionDescriptor descriptor)
        {
            // свободная функция
            return new EventPad (context, target, eventName, descriptor.CallPoint);
        }

        throw new BarsikException ("Can't subscribe");
    }

    /// <summary>
    /// Выполнение внешней программы и получение ее выходного потока.
    /// </summary>
    public static dynamic? System_
        (
            Context context,
            dynamic?[] args
        )
    {
        var command = (string?) Compute (context, args, 0);
        if (string.IsNullOrWhiteSpace (command))
        {
            return null;
        }

        command = command.Trim();
        var startInfo = new ProcessStartInfo (command)
        {
            UseShellExecute = false,
            RedirectStandardOutput = true
        };
        for (var i = 1; i < args.Length; i++)
        {
            var value = (string?) Compute (context, args, i);
            if (!string.IsNullOrWhiteSpace (value))
            {
                startInfo.ArgumentList.Add (value.Trim());
            }
        }

        var process = Process.Start (startInfo);
        if (process is not null)
        {
            var result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            return result;
        }

        return null;
    }

    /// <summary>
    /// Выдача типа по его имени.
    /// </summary>
    public static dynamic TemporaryFile
        (
            Context context,
            dynamic?[] args
        )
    {
        return Path.GetTempFileName();
    }

    /// <summary>
    /// Выдача типа по его имени.
    /// </summary>
    public static dynamic? Type_
        (
            Context context,
            dynamic?[] args
        )
    {
        // TODO поддержка конструкций вроде type ("System.Collections.Generic.List`1", int)
        // т. е. с именами типов без кавычек

        var typeName = Compute (context, args, 0) as string;
        if (string.IsNullOrEmpty (typeName))
        {
            return null;
        }

        if (args.Length < 2)
        {
            return context.FindType (typeName);
        }

        var typeArguments = new List<string>();
        for (var i = 1; i < args.Length; i++)
        {
            var one = Compute (context, args, i) as string;
            if (!string.IsNullOrEmpty (one))
            {
                typeArguments.Add (one);
            }
        }

        return context.FindType (typeName, typeArguments.ToArray());
    }

    /// <summary>
    /// Расшифровка чувствительных данных.
    /// </summary>
    public static dynamic? Unrotect
        (
            Context context,
            dynamic?[] args
        )
    {
        var value = Compute (context, args, 0);
        if (value is not null)
        {
            var encrypted = value.ToString();
            if (!string.IsNullOrEmpty (encrypted))
            {
                return ConfigurationUtility.Unprotect (encrypted);
            }
        }

        return null;
    }

    /// <summary>
    /// Отписка от нативного события.
    /// </summary>
    public static dynamic? Unsubscribe
        (
            Context context,
            dynamic?[] args
        )
    {
        foreach (var one in args)
        {
            if (one is EventPad pad)
            {
                pad.Unsubscribe();
            }
        }

        return null;
    }

    /// <summary>
    /// Подключение/отключение пространств имен.
    /// </summary>
    public static dynamic Use
        (
            Context context,
            dynamic?[] args
        )
    {
        var topContext = context.GetTopContext();
        for (var i = 0; i < args.Length; i++)
        {
            var name = Compute (context, args, i) as string;
            if (!string.IsNullOrWhiteSpace (name))
            {
                name = name.Trim();
                if (!string.IsNullOrEmpty (name))
                {
                    if (name.StartsWith ('-'))
                    {
                        name = name.Substring (1);
                        if (!string.IsNullOrEmpty (name))
                        {
                            topContext.Namespaces.Remove (name);
                        }
                    }
                    else
                    {
                        topContext.Namespaces[name] = null;
                    }
                }
            }
        }

        return context.Namespaces.Keys;
    }

    #endregion

    #region IBarsikModule members

    /// <inheritdoc cref="IBarsikModule.Description"/>
    public string Description => "Standard library for Barsik";

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
        foreach (var descriptor in Registry)
        {
            context.Functions.Remove (descriptor.Key);
        }
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => Description;

    #endregion
}
