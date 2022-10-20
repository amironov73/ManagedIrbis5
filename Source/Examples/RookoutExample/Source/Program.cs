// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable LocalizableElement

/* Program.cs -- пример приложения, предназначенного для отладки Rookout
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Diagnostics;

#endregion

RookoutUtility.SetupRookout();

var x = 1;
var y = 2;
var z = x + y;

Console.WriteLine ($"{x} + {y} = {z}");

Console.WriteLine ("Press <ENTER> to stop the application");
Console.ReadLine();
