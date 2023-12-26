// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* Program.cs -- точка входа в программу
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Globalization;

using AM.StableDiffusion;

using ResizeImages;

#endregion

var resizer = new ImageResizer
{
    Progress = new ConsoleReporter()
};
var positional = resizer.ParseCommandLine (args);

if (positional.Length != 4)
{
    Console.Error.WriteLine ("Usage: ResizeImages [options] <source> <destination> <maxWidth> <maxHeight>");
    return 1;
}

try
{
    var sourceDirectory = positional[0];
    var destinationDirectory = positional[1];
    var newWidth = int.Parse (positional[2], NumberStyles.Integer, CultureInfo.InvariantCulture);
    var newHeight = int.Parse (positional[3], NumberStyles.Integer, CultureInfo.InvariantCulture);

    resizer.ResizeImages
        (
            sourceDirectory,
            destinationDirectory,
            newWidth,
            newHeight
        );
    Console.WriteLine();
}
catch (Exception exception)
{
    Console.Error.WriteLine();
    Console.Error.WriteLine (exception.ToString());
    Console.Error.WriteLine();
    return 1;
}

return 0;
