// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable LocalizableElement

/* Program.cs -- simple text encryptor
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Security;

#endregion

#nullable enable

if (args.Length != 2)
{
    Console.WriteLine ("USAGE: Encryptor <text> <password>");
    return 0;
}

var secretText = args[0];
var password = args[1];
var encryptedText = SecurityUtility.Encrypt (secretText, password);
Console.WriteLine (encryptedText);

return 0;
