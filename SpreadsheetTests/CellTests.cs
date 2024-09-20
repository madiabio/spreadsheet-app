// <copyright file="CellTests.cs" company="UofU-CS3500">
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
///    This file contains the tests for the Cell class.
/// </summary>
namespace SpreadsheetTests;
using Spreadsheet;
using Formula;

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
        HashSet<string> trueContents = new HashSet<string>{"a", "b", "c"};

        cell.Contents = trueContents;
    }

    /// <summary>
    /// This test checks <see cref="Cell.Contents"/> will allow valid contents to be set
    /// when the contents are a string.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(InvalidContentsException))]
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
