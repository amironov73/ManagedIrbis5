using System.Reflection;
using System.Runtime.CompilerServices;

if (args.Length != 3)
{
    Console.WriteLine ("MethodRunner <assembly> <class> <method>");
    return 1;
}

var assemblyPath = args[0];
var className = args[1];
var methodName = args[2];
var assembly = Assembly.LoadFrom (assemblyPath); // Load и LoadFile не работают!

var type = assembly.GetType (className);
if (type is null)
{
    Console.WriteLine ("Can't find class");
    return 2;
}

var method = type.GetMethod
    (
        methodName,
        BindingFlags.Static
        |BindingFlags.Public
        |BindingFlags.NonPublic
    );
if (method is null)
{
    Console.WriteLine ("Can't find method");
    return 3;
}

RuntimeHelpers.PrepareMethod (method.MethodHandle);

//Console.WriteLine("Now you can attach debugger");
//Console.ReadLine();

method.Invoke (null, null);

return 0;
