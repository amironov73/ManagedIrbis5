﻿// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Text;

#nullable enable

namespace UnitTests.AM.Text
{
    [TestClass]
    public class RichTextStripperTest
    {
        private void _TestStrip
            (
                string? source,
                string? expected
            )
        {
            var actual = RichTextStripper.StripRichTextFormat(source).DosToUnix();

            Assert.AreEqual
                (
                    expected,
                    actual
                );
        }

        [TestMethod]
        public void RichTextStripper_StripRichTextFormat()
        {
            _TestStrip(null, null);
            _TestStrip(string.Empty, string.Empty);
            _TestStrip(" ", " ");
            _TestStrip("{}", "");
            _TestStrip(@"{\rtf1 }", "");
            _TestStrip("У попа была собака", "У попа была собака");
            _TestStrip
                (
                    @"{\rtf1\ansi\deff0
{\colortbl;\red0\green0\blue0;\red255\green0\blue0;}
This line is the default color\line
\cf2
This line is red\line
\cf1
This line is the default color
}",
                    "\n\nThis line is the default color\n\n\nThis line is red\n\n\nThis line is the default color\n"
                );

            _TestStrip
                (
                    @"{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fswiss\fcharset0 Arial;}
{\f1\fnil\fprq1\fcharset0 Courier New;}{\f2\fswiss\fprq2\fcharset0 Arial;}}
{\colortbl ;\red0\green128\blue0;\red0\green0\blue0;}
{\*\generator Msftedit 5.41.21.2508;}
\viewkind4\uc1\pard\f0\fs20 The \i Greek \i0 word for psyche is spelled \cf1\f1\u968?\u965?\u967?\u942?\cf2\f2 . The Greek letters are encoded in Unicode.\par
These characters are from the extended \b ASCII \b0 character set (Windows code page 1252):  \'e2\'e4\u1233?\'e5\cf0\par }",
                    "\n\n\n\nThe Greek word for psyche is spelled ψυχή. The Greek letters are encoded in Unicode.\n\nThese characters are from the extended ASCII character set (Windows code page 1252):  âäӑå\n"
                );
        }
    }
}
