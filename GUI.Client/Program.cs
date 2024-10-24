// <copyright file="Program.cs" company="UofU-CS3500">
// Copyright (c) 2024 UofU-CS3500. All rights reserved.
// </copyright>
// Written by Joe Zachary for CS 3500, September 2013
// Update by Profs Kopta and de St. Germain
// - Updated return types
// - Updated documentation

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
//
//     The starter code for this file was written by profs Joe, Danny and Jim.
//     The code for this file was updated in partnership of Madi Abio and Sadie Bowen,
//         however, the original code was written by Madi Abio for Assignment 05.
// </para>
// </summary>
var builder = Microsoft.AspNetCore.Components.WebAssembly.Hosting.WebAssemblyHostBuilder.CreateDefault(args);

await builder.Build().RunAsync();
