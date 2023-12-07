// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo

/* Program.cs -- настройка и запуск Web-приложения
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics;
using System.Runtime.InteropServices;

using Gatekeeper2024;

#endregion

var builder = WebApplication.CreateBuilder (args);

// // Add services to the container.
// // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

// *******************************************************************
// демонизация

// под отладчиком запускаемся как обычное консольное приложение
var needDemonize = !Debugger.IsAttached;

// при явном указании на запуск в консоли тоже запускаемся как обычно
if (args.Length != 0)
{
    var command = args[0].ToLowerInvariant();
    if (command == "console")
    {
        needDemonize = false;
    }

    if (needDemonize)
    {
        if (RuntimeInformation.IsOSPlatform (OSPlatform.Linux))
        {
            Utility.UseSystemd (builder);
        }
        else if (RuntimeInformation.IsOSPlatform (OSPlatform.Windows))
        {
            Utility.UseWindowsService (builder);
        }
        else
        {
            throw new Exception ("Operating system not supported");
        }
    }
}

// *******************************************************************

var app = builder.Build();

// // Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

// app.UseHttpsRedirection();

var ui = new UserInterface (app);
app.MapGet ("/", context => ui.HandleRequest (context));

var handler = new SigurHandler (app);
app.MapPost ("/auth", context => handler.HandleRequest (context));

app.Run();
