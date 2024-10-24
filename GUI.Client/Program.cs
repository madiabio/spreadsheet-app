// <copyright file="Program.cs" company="UofU-CS3500">
// Copyright (c) 2024 UofU-CS3500. All rights reserved.
// </copyright>

var builder = Microsoft.AspNetCore.Components.WebAssembly.Hosting.WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddSingleton<CS3500.Spreadsheet.Spreadsheet>(); // FIXME: might need to be transient or scoped?

await builder.Build().RunAsync();
