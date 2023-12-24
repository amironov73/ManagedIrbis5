using AM.StableDiffusion;

if (args.Length != 3)
{
    Console.Error.WriteLine ("Usage: ReplaceText <path-with-pattern> <oldText> <newText>");
    return 1;
}

try
{
    var path = Path.GetDirectoryName (args[0]) ?? ".";
    var pattern = Path.GetFileName (args[0]);
    var oldText = args[1];
    var newText = args[2];

    var replacer = new TextReplacer();
    replacer.Replace (path, pattern, oldText, newText);
}
catch (Exception exception)
{
    Console.Error.WriteLine (exception.ToString());
    return 1;
}

return 0;
