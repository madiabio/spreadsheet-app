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


/// <summary>
/// This <see cref="TestClass"/> contains the tests for the <see cref="Spreadsheet"/> class.
/// </summary>
[TestClass]
public class SpreadsheetTests
{

    /// </returns>
    /// <summary>
    /// Checks the ordering of dependencies for the return value of <see cref="Spreadsheet.SetCellContents(string, Formula)"/>
    /// </summary>
    public void SpreadsheetConstructor_TestSetCellContentsDoubleReturn_ExpectedBehaviour()
    {
        Spreadsheet ss = new Spreadsheet();
        ss.SetCellContents("B1", new Formula("A1*2"));
        ss.SetCellContents("C1", new Formula("B1*2"));

        IList<string> results = ss.SetCellContents("A1", 1.1);

        IList<string> expected = new List<string> { "A1", "B1", "C1" };
        Assert.AreEqual(expected, results);

    }

    /// </returns>
    /// <summary>
    /// Checks the ordering of dependencies for the return value of <see cref="Spreadsheet.SetCellContents(string, Formula)"/>
    /// </summary>
    public void SpreadsheetConstructor_TestSetCellContentsStringReturn_ExpectedBehaviour()
    {
        // FIXME: CHECK THAT THIS IS CORRECT AND THAT D1 SHOULDN'T BE IN THE LIST.
        Spreadsheet ss = new Spreadsheet();
        ss.SetCellContents("B1", new Formula("A1*2"));
        ss.SetCellContents("C1", new Formula("B1*2"));

        IList<string> results = ss.SetCellContents("A1", "D1");

        IList<string> expected = new List<string> { "A1", "B1", "C1" };
        Assert.AreEqual(expected, results);

    }


    /// </returns>
    /// <summary>
    /// Checks the ordering of dependencies for the return value of <see cref="Spreadsheet.SetCellContents(string, Formula)"/>
    /// </summary>
    public void SpreadsheetConstructor_TestSetCellContentsFormulaReturn_ExpectedBehaviour()
    {
        // FIXME: CHECK THAT THIS IS CORRECT AND THAT D1 SHOULDN'T BE IN THE LIST.
        Spreadsheet ss = new Spreadsheet();
        ss.SetCellContents("B1", new Formula("A1*2"));
        ss.SetCellContents("C1", new Formula("B1*2"));

        IList<string> results = ss.SetCellContents("A1", new Formula("D1*2"));

        IList<string> expected = new List<string> { "A1", "B1", "C1" };
        Assert.AreEqual(expected, results);

    }



    /// <summary>
    /// Checks a <see cref="CircularException"/> is thrown when a circular dependency is attempted
    /// to be put into the graph.
    [TestMethod]
    [ExpectedException(typeof(CircularException))]
    public void SpreadsheetConstructor_TestCircularDependency_Invalid()
    {
        Spreadsheet ss = new Spreadsheet();

        ss.SetCellContents("A1", new Formula("B1*2"));
        ss.SetCellContents("B1", new Formula("C1*2"));
        ss.SetCellContents("C1", new Formula("A1*2"));
    }


    /// <summary>
    /// Checks that <see cref="Spreadsheet.GetNamesOfAllNonemptyCells()"/> works as expected
    /// when there ARE cells in the graph
    /// </summary>
    [TestMethod]
    public void SpreadsheetConstructor_TestGetNamesOfAllNonemptyCellsCellsInGraph_ExpectedBehaviour()
    {
        Spreadsheet ss = new Spreadsheet();
        double trueContents = 1.1;
        ss.SetCellContents("A1", trueContents);
        ss.SetCellContents("B1", trueContents);
        ss.SetCellContents("C1", trueContents);

        HashSet<string> expected = new HashSet<string> { "A1", "B1", "C1" };
        Assert.AreEqual(expected, ss.GetNamesOfAllNonemptyCells());
    }

    /// <summary>
    /// Checks that <see cref="Spreadsheet.GetNamesOfAllNonemptyCells()"/> works as expected
    /// when cells are added and then removed from the graph.
    /// </summary>
    [TestMethod]
    public void SpreadsheetConstructor_TestGetNamesOfAllNonemptyCellsAddedAndRemoved_ExpectedBehaviour()
    {
        Spreadsheet ss = new Spreadsheet();
        double trueContents = 1.1;
        ss.SetCellContents("A1", trueContents);
        ss.SetCellContents("B1", trueContents);
        ss.SetCellContents("C1", trueContents);


        ss.SetCellContents("A1", string.Empty);
        ss.SetCellContents("B1", string.Empty);
        ss.SetCellContents("C1", string.Empty);

        HashSet<string> expected = new HashSet<string> { };
        Assert.AreEqual(expected, ss.GetNamesOfAllNonemptyCells());
    }


    /// <summary>
    /// This test checks <see cref="Spreadsheet.GetCellContents(string)"/> will
    /// return an empty string if the cell is not in the graph (the cell is an empty cell)
    /// </summary>
    [TestMethod]
    public void SpreadsheetConstructor_TestGetCellContentsNoContents_Valid()
    {
        Spreadsheet ss = new Spreadsheet();
        Assert.AreEqual(string.Empty, ss.GetCellContents("A1"));
    }


    /// <summary>
    /// This test checks  <see cref="Spreadsheet.SetCellContents(string, double)"/> will not work when an incorrect 
    /// cell name is passed in.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(InvalidNameException))]
    public void SpreadsheetConstructor_TestSetCellContentsDouble_InvalidCellName()
    {
        Spreadsheet ss = new Spreadsheet();
        double trueContents = 1.1;

        ss.SetCellContents("1A1A", trueContents);

    }

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


/// <summary>
/// This <see cref="TestClass"/> contains the tests for the <see cref="Cell"/> class.
/// </summary>
[TestClass]
public class CellTests
{

    /// <summary>
    /// This test checks <see cref="Cell.Contents"/> will not allow invalid contents to be set
    /// by ensuring it throws a <see cref="InvalidContentsException"/> when a HashSet is attempted
    /// to be used as the contents.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(InvalidContentsException))]
    public void CellConstructor_TestContents_Invalid()
    {
        Cell cell = new Cell();
        HashSet<string> trueContents = new HashSet<string> { "a", "b", "c" };

        cell.Contents = trueContents;
    }

    /// <summary>
    /// This test checks <see cref="Cell.Contents"/> will allow valid contents to be set
    /// when the contents are a string.
    /// </summary>
    [TestMethod]
    public void CellConstructor_TestContentsString_Valid()
    {
        Cell cell = new Cell();
        string trueContents = "a";

        cell.Contents = trueContents;
    }



    /// <summary>
    /// This test checks <see cref="Cell"/> will allow an valid name to be set
    /// when the name uses lowercase characters.
    /// </summary>
    [TestMethod]
    public void CellConstructor_TestNameLowercase_Valid()
    {
        Cell cell = new Cell();
        cell.Name = "abc1";

    }

    /// <summary>
    /// This test checks <see cref="Cell"/> will allow an valid name to be set
    /// when the name uses uppercase characters.
    /// </summary>
    [TestMethod]
    public void CellConstructor_TestNameUppercase_Valid()
    {
        Cell cell = new Cell();
        cell.Name = "ABC1";

    }

    /// <summary>
    /// This test checks <see cref="Cell"/> will not allow an invalid name to be set
    /// by ensuring it throws a <see cref="InvalidNameException"/> when a number
    /// is the prefix of the name.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(InvalidNameException))]
    public void CellConstructor_TestNameNumberFirst_Invalid()
    {
        Cell cell = new Cell();
        cell.Name = "1ABC";

    }

    /// <summary>
    /// This test checks <see cref="Cell"/> will not allow an invalid name to be set
    /// by ensuring it throws a <see cref="InvalidNameException"/> when a number is
    /// in between some characters in the name.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(InvalidNameException))]
    public void CellConstructor_TestNameNumberInMiddle_Invalid()
    {
        Cell cell = new Cell();
        cell.Name = "A1BC";

    }


}
