/// <copyright file="DependencyGraphTests.cs" company="UofU-CS3500">
///   Copyright © 2024 UofU-CS3500. All rights reserved.
/// </copyright>
/// <summary>
/// Author:    Madeline Abio
/// Partner:   N/A
/// Date:      09/12/2024
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
///    This file contains tests written for the DependencyGraph class.
/// </summary>

namespace CS3500.DependencyGraph;

using CS3500.DependencyGraph;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;

/// <summary>
///   This is a test class for DependencyGraphTest and is intended
///   to contain all DependencyGraphTest Unit Tests
/// </summary>
[TestClass]
public class DependencyGraphExampleStressTests
{
    /// <summary>
    /// This function tests that creating a dependency graph leads to non-null object creation
    /// </summary>
    [TestMethod]
    public void DependencyGraphConstructor_TestNewDependencyGraphIsNotNull_ExpectedBehaviour()
    {
        DependencyGraph dg = new();
        Assert.IsNotNull(dg);
    }

    /// <summary>
    /// Checks that dependents in the DependencyGraph are updated accordingly when
    /// added using <see cref="DependencyGraph.AddDependency(string, string)"/> and 
    /// checked using <see cref="DependencyGraph.GetDependents(string)"/>.
    /// </summary>
    [TestMethod]
    public void DependencyGraphConstructor_TestAddDependencyUpdatesDependents_ExpectedBehaviour()
    {
        DependencyGraph dg = new();
        (string, string) relationship = ("a", "b");

        HashSet<string> dependents = new HashSet<string> { relationship.Item2 };
        dg.AddDependency(relationship.Item1, relationship.Item2);

        Assert.IsTrue(dependents.SetEquals(new HashSet<string>(dg.GetDependents(relationship.Item1))));
    }

    /// <summary>
    /// Checks that dependees in the DependencyGraph are updated accordingly when
    /// added using <see cref="DependencyGraph.AddDependency(string, string)"/> and 
    /// checked using <see cref="DependencyGraph.GetDependees(string)"/>.
    /// </summary>
    [TestMethod]
    public void DependencyGraphConstructor_TestAddDependencyUpdatesDependees_ExpectedBehaviour()
    {
        DependencyGraph dg = new();
        (string, string) relationship = ("a", "b");
        
        HashSet<string> dependees = new HashSet<string> { relationship.Item1 };
        dg.AddDependency(relationship.Item1, relationship.Item2);

        Assert.IsTrue(dependees.SetEquals(new HashSet<string>(dg.GetDependees(relationship.Item2))));
    }

    /// <summary>
    /// Checks that dependents of a node in the DependencyGraph are updated accordingly when
    /// a relationship1 is added to the graph using <see cref="DependencyGraph.AddDependency(string, string)"/>, 
    /// then removed using <see cref="DependencyGraph.RemoveDependency(string, string)"/> and
    /// finally the dependents of the node are checked using <see cref="DependencyGraph.GetDependents(string)"/>.
    /// </summary>
    [TestMethod]
    public void DependencyGraphConstructor_TestRemoveDependentUpdatesDependents_ExpectedBehaviour()
    {
        DependencyGraph dg = new();
        (string, string) relationship = ("a", "b");

        HashSet<string> dependents = new HashSet<string> { relationship.Item2 };
        dg.AddDependency(relationship.Item1, relationship.Item2);

        dependents.Remove(relationship.Item2);
        dg.RemoveDependency(relationship.Item1, relationship.Item2);

        Assert.IsTrue(dependents.SetEquals(new HashSet<string>(dg.GetDependents(relationship.Item1))));

    }

    /// <summary>
    /// Checks that dependees of a node in the DependencyGraph are removed accordingly when
    /// a relationship1 to the graph is added using <see cref="DependencyGraph.AddDependency(string, string)"/>, 
    /// then removed using <see cref="DependencyGraph.RemoveDependency(string, string)"/> and
    /// finally checked using <see cref="DependencyGraph.GetDependees(string)"/>.
    /// </summary>
    [TestMethod]
    public void DependencyGraphConstructor_TestRemoveDependentUpdatesDependees_ExpectedBehaviour()
    {
        DependencyGraph dg = new();
        (string, string) relationship = ("a", "b");

        HashSet<string> dependees = new HashSet<string> { relationship.Item1 };
        dg.AddDependency(relationship.Item1, relationship.Item2);

        dependees.Remove(relationship.Item1);
        dg.RemoveDependency(relationship.Item1, relationship.Item2);

        Assert.IsTrue(dependees.SetEquals(new HashSet<string>(dg.GetDependents(relationship.Item1))));
    }

    /// <summary>
    /// Checks that <see cref="DependencyGraph.Size"/> returns the correct sizes
    /// for DepedencyGraph.
    /// </summary>
    [TestMethod]
    public void DependencyGraphConstructor_TestSizeReturnsCorrectSize_ExpectedBehaviour()
    {
        DependencyGraph dg = new();
        (string, string) relationship1 = ("a", "b");
        dg.AddDependency(relationship1.Item1, relationship1.Item2);
        Assert.AreEqual(1, dg.Size);

        (string, string) relationship2 = ("b", "c");
        dg.AddDependency(relationship2.Item1, relationship2.Item2);
        Assert.AreEqual(2, dg.Size);

        dg.RemoveDependency(relationship1.Item1, relationship1.Item2);
        Assert.AreEqual(1, dg.Size);

        dg.RemoveDependency(relationship2.Item1, relationship2.Item2);
        Assert.AreEqual(0, dg.Size);

    }

    /// <summary>
    /// Tests <see cref="DependencyGraph.HasDependents(string)"/> works as expected by 
    /// checking the boolean is returned correctly.
    /// </summary>
    [TestMethod]
    public void DependencyGraphConstructor_TestHasDependents_ExpectedBehaviour()
    {
        DependencyGraph dg = new();
        (string, string) relationship = ("a", "b");

        dg.AddDependency(relationship.Item1, relationship.Item2);
        
        Assert.IsTrue(dg.HasDependents(relationship.Item1));
        Assert.IsFalse(dg.HasDependents(relationship.Item2));
    }


    /// <summary>
    /// Tests to see if <see cref="DependencyGraph.HasDependees(string)"/> works as
    /// expected by checking the boolean is returned correctly.
    /// </summary>
    [TestMethod]
    public void DependencyGraphConstructor_TestHasDependees_ExpectedBehaviour()
    {
        DependencyGraph dg = new();
        (string, string) relationship = ("a", "b");

        dg.AddDependency(relationship.Item1, relationship.Item2);

        Assert.IsTrue(dg.HasDependees(relationship.Item2));
        Assert.IsFalse(dg.HasDependees(relationship.Item1));
    }


    /// <summary>
    /// Tests that <see cref="DependencyGraph.HasDependees(string)"/> updates 
    /// when relationships are added/removed.
    /// </summary>
    [TestMethod]
    public void DependencyGraphConstructor_TestHasDependentsUpdates_ExpectedBehaviour()
    {
        DependencyGraph dg = new();


        (string, string) relationship1 = ("a", "b");
        dg.AddDependency(relationship1.Item1, relationship1.Item2);

        (string, string) relationship2 = ("b", "c");
        dg.AddDependency(relationship2.Item1, relationship2.Item2);

        (string, string) relationship3 = ("c", "a");
        dg.AddDependency(relationship3.Item1, relationship3.Item2);


        dg.RemoveDependency(relationship1.Item1, relationship1.Item2);
        Assert.IsFalse(dg.HasDependents(relationship1.Item1));
    }


    /// <summary>
    /// Tests that <see cref="DependencyGraph.HasDependees(string)"/> updates 
    /// when relationships are added/removed.
    /// </summary>
    [TestMethod]
    public void DependencyGraphConstructor_TestHasDependeesUpdates_ExpectedBehaviour()
    {
        DependencyGraph dg = new();


        (string, string) relationship1 = ("a", "b");
        dg.AddDependency(relationship1.Item1, relationship1.Item2);

        (string, string) relationship2 = ("b", "c");
        dg.AddDependency(relationship2.Item1, relationship2.Item2);

        (string, string) relationship3 = ("c", "a");
        dg.AddDependency(relationship3.Item1, relationship3.Item2);


        dg.RemoveDependency(relationship1.Item1, relationship1.Item2);
        Assert.IsFalse(dg.HasDependees(relationship2.Item1));
    }

    
    /// <summary>
    /// Tests that <see cref="DependencyGraph.ReplaceDependents(string, IEnumerable{string})"/>
    /// works as expected
    /// </summary>
    [TestMethod]
    public void DependencyGraphConstructor_TestReplaceDependents_ExpectedBehaviour()
    {
        DependencyGraph dg = new();
        (string, string) relationship1 = ("a", "b");
        (string, string) relationship2 = ("a", "c");
        (string, string) relationship3 = ("a", "d");

        dg.AddDependency(relationship1.Item1, relationship1.Item2);
        dg.AddDependency(relationship2.Item1, relationship2.Item2);
        dg.AddDependency(relationship3.Item1, relationship3.Item2);

        ISet<string> oldDependents = new HashSet<string> { "b", "c", "d" };
        Assert.IsTrue(oldDependents.SetEquals(new HashSet<string>(dg.GetDependents(relationship1.Item1))));


        ISet<string> newDependents = new HashSet<string> { "e", "f", "g" };
        dg.ReplaceDependents(relationship1.Item1, newDependents); // replace all dependents of a with trueNewDependents set. 
        
        Assert.IsTrue(newDependents.SetEquals(new HashSet<string>(dg.GetDependents(relationship1.Item1))));

    }

    /// <summary>
    /// Tests that <see cref="DependencyGraph.ReplaceDependees(string, IEnumerable{string})"/>
    /// works as expected 
    /// </summary>
    [TestMethod]
    public void DependencyGraphConstructor_TestReplaceDependees_ExpectedBehaviour()
    {
        DependencyGraph dg = new();
        (string, string) relationship1 = ("b", "a");
        (string, string) relationship2 = ("c", "a");
        (string, string) relationship3 = ("d", "a");

        dg.AddDependency(relationship1.Item1, relationship1.Item2);
        dg.AddDependency(relationship2.Item1, relationship2.Item2);
        dg.AddDependency(relationship3.Item1, relationship3.Item2);

        ISet<string> trueOldDependees = new HashSet<string> { "b", "c", "d" };
        HashSet<string> oldDependees = new HashSet<string>(dg.GetDependees(relationship1.Item2));
        Assert.IsTrue(trueOldDependees.SetEquals(oldDependees)); 

        ISet<string> trueNewDependees = new HashSet<string> { "e", "f", "g" };
        dg.ReplaceDependees(relationship1.Item2, trueNewDependees); // replace all dependees of a with trueNewDependents set. 
        HashSet <string> newDependees = new HashSet<string> (dg.GetDependees(relationship1.Item2));
        Assert.IsTrue(trueNewDependees.SetEquals(newDependees));
    }





    /// <summary>
    ///   This piece of code stress-tests the DependencyGraph implementation by
    ///   making a large number of nodes (200), making them all dependent on eachother,
    ///   then removing 1/4 of the dependencies, adding 1/2 of the dependencies back,
    ///   then removing 1/3 of them.
    ///   
    ///   Finally the test checks that the dependents are correct by comparing the 
    ///   set of dependents (dependents[i]) for node i against the method
    ///   .GetDependents(i), and does the same for dependencies.
    ///   
    ///    This method creates copies of the dependents and dependees for each node
    ///    that are used to compare at the end against the dg way of storing these values.
    /// </summary>
    [TestMethod]
    [Timeout( 2000 )]  // FIXME: 2 second run time limit <--  remove this comment
    public void StressTest( )
    {
        DependencyGraph dg = new();

        // A bunch of strings to use
        const int SIZE = 200;
        string[] letters = new string[SIZE];
        for ( int i = 0; i < SIZE; i++ )
        {
            letters[i] = string.Empty + ( (char) ( 'a' + i ) ); 
        }

        // The correct answers
        // this creates a hashset of strings which are each an ASCII character.
        // Note, there's 126 ASCII chars but SIZE=200, so some will be 'reapeated',
        // but since its a HashSet it won't actually be.
        HashSet<string>[] dependents = new HashSet<string>[SIZE]; 
        HashSet<string>[] dependees = new HashSet<string>[SIZE];
        for ( int i = 0; i < SIZE; i++ )
        {
            dependents[i] = [];
            dependees[i] = [];
        }

        // Add a bunch of dependencies
        // this code adds dependencies to each letter in letters
        for ( int i = 0; i < SIZE; i++) 
        {
            for ( int j = i + 1; j < SIZE; j++ )
            {
                dg.AddDependency( letters[i], letters[j] );
                dependents[i].Add( letters[j] );
                dependees[j].Add( letters[i] );
            }
        }

        // Remove a bunch of dependencies
        for ( int i = 0; i < SIZE; i++ )
        {
            for ( int j = i + 4; j < SIZE; j += 4 )
            {
                dg.RemoveDependency( letters[i], letters[j] );
                dependents[i].Remove( letters[j] );
                dependees[j].Remove( letters[i] );
            }
        }

        // Add some back
        for ( int i = 0; i < SIZE; i++ )
        {
            for ( int j = i + 1; j < SIZE; j += 2 )
            {
                dg.AddDependency( letters[i], letters[j] );
                dependents[i].Add( letters[j] );
                dependees[j].Add( letters[i] );
            }
        }

        // Remove some more
        for ( int i = 0; i < SIZE; i += 2 )
        {
            for ( int j = i + 3; j < SIZE; j += 3 )
            {
                dg.RemoveDependency( letters[i], letters[j] );
                dependents[i].Remove( letters[j] );
                dependees[j].Remove( letters[i] );
            }
        }

        // Make sure everything is right
        for ( int i = 0; i < SIZE; i++ )
        {
            Assert.IsTrue( dependents[i].SetEquals( new HashSet<string>( dg.GetDependents( letters[i] ) ) ) );
            Assert.IsTrue( dependees[i].SetEquals( new HashSet<string>( dg.GetDependees( letters[i] ) ) ) );
        }


        // Tests replace depenedents
        HashSet<string> trueNewDependents = new HashSet<string>();
        for (int i = 0; i < SIZE; i += 1) 
        {
            trueNewDependents.Add(letters[i]); // add a bunch of letters to the set of new dependents
        }

        for (int i = 0; i < SIZE; i++)
        {
            dg.ReplaceDependents(letters[i], trueNewDependents); // replace dependents w/ new dependents into the graph
            dependents[i] = trueNewDependents; // adjust dictionary so we can check
            
            HashSet<string> newDependents = new HashSet<string>(dg.GetDependents(letters[i])); // delaring this for debugging purposes
            Assert.IsTrue(dependents[i].SetEquals(newDependents)); // check truth
        }
       

    }
}