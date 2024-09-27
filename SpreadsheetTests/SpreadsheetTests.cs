// <copyright file="SpreadsheetTests.cs" company="UofU-CS3500">
// Copyright (c) 2024 UofU-CS3500. All rights reserved.
// </copyright>
/// <summary>
/// Author:    Madeline Abio
/// Partner:   N/A
/// Date:      20/09/2024
/// Course:    CS 3500, University of Utah, School of Computing
/// Copyright: CS 3500 and Madeline Abio - This work may not 
///            be copied for use in Academic Coursework.
///
/// I, Madeline Abio, certify that I wrote this code from scratch and
/// did not copy it in part or whole from another source.  All 
/// references used in the completion of the assignments are cited 
/// in my README file.
///
/// File Contents
///
///    This file contains the tests for the Spreadsheet class.
/// </summary>
namespace CS3500.SpreadsheetTests;
using CS3500.Spreadsheet;
using CS3500.Formula;
[TestClass]
public class SpreadsheetTests
{
    /// <summary>
    /// This test checks  <see cref="Spreadsheet.SetCellContents(string, double)"/> will not work when an incorrect 
    /// cell name is passed in.
    /// </summary>


    /// <summary>
    /// <para>
    /// This test checks the <see cref="Spreadsheet.SetCellContents(string, string)"/> method works as expected
    /// by setting the contents of a cell when the contents is a string, then checking the contents of that cell.
    /// </para>
    /// </summary>
    [TestMethod]
    public void SpreadsheetConstructor_TestSetCellContentsString_ExpectedBehvaiour()
    {
        Spreadsheet ss = new Spreadsheet();
        string trueContents = "test";

        ss.SetCellContents("A1", trueContents);

        Assert.AreEqual(trueContents, ss.GetCellContents("A1"));

    }

    /// <summary>
    /// <para>
    /// This test checks the <see cref="Spreadsheet.SetCellContents(string, double)"/> method works as expected
    /// by setting the contents of a cell with a double, then checking the contents of that cell.
    /// </para>
    /// </summary>
    [TestMethod]
    public void SpreadsheetConstructor_TestSetCellContentsDouble_ExpectedBehvaiour()
    {
        Spreadsheet ss = new Spreadsheet();
        double trueContents = 1.1;

        ss.SetCellContents("A1", trueContents);

        Assert.AreEqual(trueContents, ss.GetCellContents("A1"));
    }

    /// <summary>
    /// <para>
    /// This test checks the <see cref="Spreadsheet.SetCellContents(string, Formula)"/> method works as expected
    /// by setting the contents of a cell with a <see cref="Formula"/>, then checking the contents of that cell.
    /// </para>
    /// </summary>
    [TestMethod]
    public void SpreadsheetConstructor_TestSetCellContentsFormula_ExpectedBehvaiour()
    {
        Spreadsheet ss = new Spreadsheet();
        Formula trueContents = new Formula("1+1");

        ss.SetCellContents("A1", trueContents);

        Assert.AreEqual(trueContents, ss.GetCellContents("A1"));

    }

    /// <summary>
    /// <para>
    /// This test checks the <see cref="Spreadsheet.SetCellContents(string, string)"/> method works as expected
    /// by setting the contents of a cell to an empty string, then checking the contents of that cell.
    /// </para>
    /// </summary>
    [TestMethod]
    public void SpreadsheetConstructor_TestSetCellContentsEmptyStringReturnsEmptyString_ExpectedBehvaiour()
    {
        Spreadsheet ss = new Spreadsheet();
        string trueContents = string.Empty;

        ss.SetCellContents("A1", trueContents);

        Assert.AreEqual(trueContents, ss.GetCellContents("A1"));

    }

    /// <summary>
    /// <para>
    /// This test checks the <see cref="Spreadsheet.SetCellContents(string, string)"/> method works as expected
    /// by setting the contents of a cell to an empty string, then checking the contents of that cell.
    /// </para>
    /// </summary>
    [TestMethod]
    public void SpreadsheetConstructor_TestSetCellContentsEmptyStringNotInGraph_ExpectedBehvaiour()
    {
        Spreadsheet ss = new Spreadsheet();
        string trueContents = string.Empty;

        ss.SetCellContents("A1", trueContents);

        bool condition = ss.GetNamesOfAllNonemptyCells().Contains("A1"); // A1 should not be in this set, making the bool false.
        Assert.IsFalse(condition);
    }




}
