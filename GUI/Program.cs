// <copyright file="Program.cs" company="UofU-CS3500">
// Copyright (c) 2024 UofU-CS3500. All rights reserved.
// </copyright>

// <summary>
// <para>
//     Authors/Partnership:    Madeline Abio & Sadie Bowen
//     Date:      20/09/2024
//     Course:    CS 3500, University of Utah, School of Computing
//     Copyright: CS 3500 and Madeline Abio - This work may not
//            be copied for use in Academic Coursework.
// </para>
//
// <para>
//     We, Madeline Abio & Sadie Bowen, certify that I wrote this code from scratch and
//     did not copy it in part or whole from another source.  All
//     references used in the completion of the assignments are cited
//     in my README file.
// </para>
// </summary>
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if ( app.Environment.IsDevelopment() )
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler( "/Error", createScopeForErrors: true );

    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAntiforgery();

app.MapRazorComponents<GUI.Components.App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies( typeof( GUI.Client._Imports ).Assembly );

app.Run();
