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
