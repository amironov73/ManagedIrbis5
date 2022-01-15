// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* Context.cs -- контекст исполнения скрипта
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directive

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using AM.Collections;
using AM.IO;

#endregion

#nullable enable

namespace AM.Scripting.Barsik;

/// <summary>
/// Контекст исполнения скрипта.
/// </summary>
public sealed class Context
{
    #region Properties

    /// <summary>
    /// Интерпретатор, к которому привязан контекст.
    /// </summary>
    public Interpreter? Interpreter { get; internal set; }

    /// <summary>
    /// Родительский контекст.
    /// </summary>
    public Context? Parent { get; }

    /// <summary>
    /// Переменные.
    /// </summary>
    public Dictionary<string, dynamic?> Variables { get; }

    /// <summary>
    /// Дефайны.
    /// </summary>
    public Dictionary<string, dynamic?> Defines { get; }

    /// <summary>
    /// Точки останова.
    /// </summary>
    public Dictionary<StatementNode, Breakpoint> Breakpoints { get; }

    /// <summary>
    /// Отладчик.
    /// </summary>
    public IBarsikDebugger? Debugger { get; set; }

    /// <summary>
    /// Стандартный входной поток.
    /// </summary>
    public TextReader Input { get; }

    /// <summary>
    /// Стандартный выходной поток.
    /// </summary>
    public TextWriter Output { get; private set; }

    /// <summary>
    /// Выходной поток ошибок.
    /// </summary>
    public TextWriter Error { get; }

    /// <summary>
    /// Функции.
    /// </summary>
    public Dictionary<string, FunctionDescriptor> Functions { get; }

    /// <summary>
    /// Используемые пространства имен.
    /// </summary>
    public Dictionary<string, object?> Namespaces { get; }

    /// <summary>
    /// Обработчик внешнего кода.
    /// </summary>
    public ExternalCodeHandler? ExternalCodeHandler { get; set; }

    /// <summary>
    /// Загруженные модули.
    /// </summary>
    public List<IBarsikModule> Modules { get; }

    /// <summary>
    /// Загруженные сборки (чтобы не писать assembly-qualified type name).
    /// </summary>
    public Dictionary<string, Assembly> Assemblies { get; }

    /// <summary>
    /// Произвольные пользовательские данные, свяазанные с данным контекстом.
    /// </summary>
    public BarsikDictionary Auxiliary { get; }

    /// <summary>
    /// Опциональный префикс, используемый, например, в операторе "new"
    /// при инициализации свойств свежесозданного объекта.
    /// </summary>
    public string? Prefix { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public Context
        (
            TextReader input,
            TextWriter output,
            TextWriter error,
            Context? parent = null
        )
    {
        Parent = parent;
        Variables = new ();
        Defines = new ();
        Input = input;
        Output = output;
        Error = error;
        Functions = new ();
        Breakpoints = new ();
        Namespaces = new ();
        Modules = new ();
        Assemblies = new ();
        Auxiliary = new ();
    }

    #endregion

    #region Private members

    /// <summary>
    /// Делаем контекст внимательным к выводу текста.
    /// </summary>
    internal void MakeAttentive()
    {
        if (Output is not AttentiveWriter)
        {
            Output = new AttentiveWriter (Output);
        }
    }

    /// <summary>
    /// Конструирование типа, если необходимо.
    /// </summary>
    private Type ConstructType
        (
            Type mainType,
            Type[]? typeArguments
        )
    {
        if (typeArguments is null)
        {
            return mainType;
        }

        if (!mainType.IsGenericType)
        {
            // TODO более осмысленная реакция
            return mainType;
        }

        return mainType.MakeGenericType (typeArguments);
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Подключение модуля.
    /// </summary>
    public Context AttachModule
        (
            IBarsikModule instance
        )
    {
        Sure.NotNull (instance);

        // отыскиваем корневой контекст
        var rootContext = this;
        while (rootContext.Parent is not null)
        {
            rootContext = rootContext.Parent;
        }

        if (instance.AttachModule (rootContext))
        {
            // Output.WriteLine ($"Module loaded: {instance}");
            Modules.Add (instance);
        }

        return this;
    }

    /// <summary>
    /// Создание контекста-потомка.
    /// </summary>
    public Context CreateChild ()
    {
        return new Context
            (
                Input,
                Output,
                Error,
                this
            )
            {
                Interpreter = Interpreter
            };
    }

    /// <summary>
    /// Дамп пространств имен.
    /// </summary>
    public void DumpNamespaces()
    {
        var keys = Namespaces.Keys.ToArray();
        if (keys.IsNullOrEmpty())
        {
            Output.WriteLine ("(no namespaces)");
            return;
        }

        Array.Sort (keys);
        foreach (var key in keys)
        {
            Output.WriteLine (key);
        }
    }

    /// <summary>
    /// Дамп переменных.
    /// </summary>
    public void DumpVariables()
    {
        var keys = Variables.Keys.ToArray();

        Array.Sort (keys);
        foreach (var key in keys)
        {
            var value = Variables[key];
            Output.WriteLine
                (
                    value is null
                        ? $"{key}: (null)"
                        : $"{key}: {value.GetType().Name} = {value}"
                );
        }
    }

    /// <summary>
    /// Получение типа по его имени.
    /// </summary>
    public Type? FindType
        (
            AtomNode node,
            IEnumerable<AtomNode>? typeArgs = null
        )
    {
        Sure.NotNull (node);

        string[]? args = null;
        if (typeArgs is not null)
        {
            var list = new List<string>();
            foreach (var argNode in typeArgs)
            {
                if (argNode is VariableNode varNode)
                {
                    if (TryGetVariable (varNode.Name, out var varValue))
                    {
                        if (varValue is Type alreadyType)
                        {
                            list.Add (alreadyType.AssemblyQualifiedName.ThrowIfNullOrEmpty ());
                        }
                        else if (varValue is string strValue && !string.IsNullOrEmpty (strValue))
                        {
                            list.Add (strValue);
                        }
                        else
                        {
                            throw new BarsikException();
                        }
                    }
                    else
                    {
                        list.Add (varNode.Name.ThrowIfNullOrEmpty ());
                    }
                }
                else
                {
                    var one = (argNode.Compute (this) as string).ThrowIfNullOrEmpty();
                    list.Add (one);
                }
            }

            args = list.ToArray();
        }

        if (node is VariableNode variableNode)
        {
            if (TryGetVariable (variableNode.Name, out var variableValue))
            {
                if (variableValue is Type alreadyHaveType)
                {
                    return alreadyHaveType;
                }

                if (variableValue is string typeName1)
                {
                    return FindType (typeName1, args);
                }
            }

            return FindType (variableNode.Name);
        }

        var value = node.Compute (this);
        if (value is string typeName2)
        {
            return FindType (typeName2, args);
        }

        var typeName3 = BarsikUtility.ToString (value);

        return FindType (typeName3, args);
    }

    /// <summary>
    /// Получение типа по его имени.
    /// </summary>
    public Type? FindType
        (
            string name,
            string[]? arguments = null
        )
    {
        Sure.NotNullNorEmpty (name);

        switch (name)
        {
            case "bool": return typeof (bool);
            case "byte": return typeof (byte);
            case "sbyte": return typeof (sbyte);
            case "short": return typeof (short);
            case "char": return typeof (char);
            case "ushort": return typeof (ushort);
            case "int": return typeof (int);
            case "uint": return typeof (uint);
            case "long": return typeof (long);
            case "ulong": return typeof (ulong);
            case "decimal": return typeof (decimal);
            case "float": return typeof (float);
            case "double": return typeof (double);
            case "object": return typeof (object);
            case "string": return typeof (string);

            // наши псевдо-типы
            case "list": return typeof (BarsikList);
            case "dict": return typeof (BarsikDictionary);
        }

        Type[]? typeArguments = null;
        if (arguments is not null)
        {
            var list = new List<Type>();
            foreach (var oneArgument in arguments)
            {
                var foundType = FindType (oneArgument);
                if (foundType is null)
                {
                    // TODO более осмысленная реакция на ошибку
                    return null;
                }

                list.Add (foundType);
            }

            typeArguments = list.ToArray();
        }

        // if (typeArguments is not null && !name.Contains ('`'))
        // {
        //     // TODO разбирать на имя типа и сборку
        //     name += $"`{typeArguments.Length}";
        // }

        var result = Type.GetType (name, false);
        if (result is not null)
        {
            return ConstructType (result, typeArguments);
        }

        var topContext = GetTopContext();
        if (!name.Contains ('.'))
        {
            // это не полное имя, так что попробуем приписать к нему
            // различные пространства имен
            foreach (var ns in topContext.Namespaces.Keys)
            {
                var fullName = ns + "." + name;
                result = Type.GetType (fullName, false);
                if (result is not null)
                {
                    return ConstructType (result, typeArguments);
                }
            }
        }

        if (!name.Contains (','))
        {
            // это не assembly-qualified name, так что попробуем
            // приписать к нему загруженные нами сборки
            foreach (var asm in topContext.Assemblies.Values)
            {
                var asmName = asm.GetName().Name;
                var fullName = name + ", " + asmName;
                result = Type.GetType (fullName, false);
                if (result is not null)
                {
                    return ConstructType (result, typeArguments);
                }

                if (!name.Contains ('.'))
                {
                    // это не полное имя, так что попробуем приписать к нему
                    // различные пространства имен
                    foreach (var ns in topContext.Namespaces.Keys)
                    {
                        fullName = ns + "." + name + ", " + asmName;
                        result = Type.GetType (fullName, false);
                        if (result is not null)
                        {
                            return ConstructType (result, typeArguments);
                        }
                    }
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Поиск функции с указанным именем.
    /// Отличается тем, что не швыряется исключениями.
    /// </summary>
    public bool FindFunction
        (
            string name,
            out FunctionDescriptor? result
        )
    {
        Sure.NotNullNorEmpty (name);

        if (Functions.TryGetValue (name, out result))
        {
            return true;
        }

        for (var context = Parent; context is not null; context = context.Parent)
        {
            if (context.Functions.TryGetValue (name, out result))
            {
                return true;
            }
        }

        if (Builtins.Registry.TryGetValue (name, out result))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Поиск функции в текущем и в родительском контекстах.
    /// </summary>
    public FunctionDescriptor GetFunction
        (
            string name
        )
    {
        Sure.NotNullNorEmpty (name);

        if (!FindFunction (name, out var result))
        {
            throw new Exception ($"Function {name} not found");
        }

        return result!;
    }

    /// <summary>
    /// Получение топового контекста, используемого как свалка
    /// для регистрации модулей и сборок.
    /// </summary>
    public Context GetTopContext()
    {
        var result = this;

        while (result.Parent is not null)
        {
            result = result.Parent;
        }

        return result;
    }

    /// <summary>
    /// Загрузка сборки.
    /// </summary>
    public Assembly? LoadAssembly
        (
            string name
        )
    {
        Sure.NotNullNorEmpty (name);

        name = name.Trim();
        if (string.IsNullOrEmpty (name))
        {
            throw new BarsikException();
        }

        if (Assemblies.ContainsKey (name))
        {
            // уже загружено, пропускаем
            return Assemblies [name];
        }

        var allAssemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach (var asm in allAssemblies)
        {
            var asmName = asm.GetName().Name;
            if (asmName.SameString (name))
            {
                Assemblies.Add (name, asm);
                return asm;
            }
        }

        Assembly? result = null;
        if (File.Exists (name))
        {
            var fullPath = Path.GetFullPath (name);
            result = Assembly.LoadFile (fullPath);
        }
        else
        {
            try
            {
                result = Assembly.Load (name);
            }
            catch
            {
                // просто съедаем исключение
            }
        }

        if (result is not null)
        {
            Assemblies.Add (name, result);
            return result;
        }

        var extension = Path.GetExtension (name);
        if (string.IsNullOrEmpty (extension))
        {
            name += ".dll";
        }
        else
        {
            if (extension.ToLowerInvariant () != ".dll")
            {
                name += ".dll";
            }
        }

        var loadPath = Environment.GetEnvironmentVariable ("BARSIK_PATH");
        if (!string.IsNullOrEmpty (loadPath))
        {
            var parts = loadPath.Split
                (
                    Path.PathSeparator,
                    StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries
                );
            foreach (var part in parts)
            {
                var fullPath = Path.GetFullPath (Path.Combine (part, name));
                if (File.Exists (fullPath))
                {
                    result = Assembly.LoadFile (fullPath);
                    Assemblies.Add (name, result);

                    return result;
                }
            }
        }

        return result;
    }

    /// <summary>
    /// Загрузка модуля.
    /// </summary>
    public void LoadModule
        (
            string moduleName
        )
    {
        // TODO поиск по путям

        Sure.NotNullNorEmpty (moduleName);

        moduleName = moduleName.Trim();

        Assembly? assembly;
        if (File.Exists (moduleName))
        {
            var fullPath = Path.GetFullPath (moduleName);
            assembly = Assembly.LoadFile (fullPath);
        }
        else
        {
            assembly = Assembly.Load (moduleName);
        }

        var types = assembly.GetTypes()
            .Where (type => type.IsAssignableTo (typeof (IBarsikModule)))
            .ToArray();
        foreach (var type in types)
        {
            var alreadyHave = false;
            foreach (var module in Modules)
            {
                if (module.GetType() == type)
                {
                    alreadyHave = true;
                    break;
                }
            }

            if (alreadyHave)
            {
                continue;
            }

            var instance = (IBarsikModule?) Activator.CreateInstance (type);
            if (instance is not null)
            {
                AttachModule(instance);
            }
        }
    }

    /// <summary>
    /// Вывод на печать значения AST-узла.
    /// </summary>
    public void Print
        (
            AtomNode node
        )
    {
        var value = node.Compute (this);
        BarsikUtility.PrintObject (Output, value);
    }

    /// <summary>
    /// Удаление переменной с указанным именем.
    /// Если такой переменной не существует,
    /// то ничего не происходит.
    /// </summary>
    public void RemoveVariable
        (
            string variableName
        )
    {
        Sure.NotNullNorEmpty (variableName);

        for (var ctx = this; ctx is not null; ctx = ctx.Parent)
        {
            if (ctx.Variables.Remove (variableName))
            {
                return;
            }
        }
    }

    /// <summary>
    /// Установка значения переменной
    /// (с сохранением ее места в контексте).
    /// </summary>
    public void SetVariable
        (
            string name,
            dynamic? value
        )
    {
        Sure.NotNullNorEmpty (name);

        if (Defines.ContainsKey (name))
        {
            throw new BarsikException ($"Can't redefine {name}");
        }

        for (var context = Parent; context is not null; context = context.Parent)
        {
            if (Defines.ContainsKey (name))
            {
                throw new BarsikException ($"Can't redefine {name}");
            }
        }

        if (Variables.ContainsKey (name))
        {
            Variables[name] = value;
            return;
        }

        for (var context = Parent; context is not null; context = context.Parent)
        {
            if (context.Variables.ContainsKey (name))
            {
                context.Variables[name] = value;
                return;
            }
        }

        Variables[name] = value;
    }

    /// <summary>
    /// Поиск переменной в текущем и в родительских контекстах.
    /// </summary>
    public bool TryGetVariable
        (
            string name,
            out dynamic? value
        )
    {
        Sure.NotNullNorEmpty (name);

        if (Defines.TryGetValue (name, out value)
            || Variables.TryGetValue (name, out value))
        {
            return true;
        }

        for (var context = Parent; context is not null; context = context.Parent)
        {
            if (context.TryGetVariable (name, out value)
                || context.Variables.TryGetValue (name, out value))
            {
                return true;
            }
        }

        return false;
    }

    #endregion
}
