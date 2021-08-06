// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Global
// ReSharper disable UnusedParameter.Local
// ReSharper disable UnusedType.Global

#region Using directives

using System;

using ManagedIrbis;

#endregion

namespace SomeApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var field = new Field(100);
            field.SubFields.Add(new SubField('a', "Matroskin"));
            field.SubFields.Add(new SubField('b', "2"));

            var person = new Person();
            person.FromField(field);
            Console.WriteLine(person);

            Console.WriteLine();
        }
    }
}
