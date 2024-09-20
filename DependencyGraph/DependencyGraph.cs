// Skeleton implementation written by Joe Zachary for CS 3500, September 2013.
// Version 1.1 (Fixed error in comment for RemoveDependency.)
// Version 1.2 - Daniel Kopta
// Version 1.3 - H. James de St. Germain Fall 2024
// (Clarified meaning of dependent and dependee.)
// (Clarified names in solution/project structure.)


/// <copyright file="DependencyGraph.cs" company="UofU-CS3500">
///   Copyright © 2024 UofU-CS3500. All rights reserved.
/// </copyright>
/// /// <summary>
/// Author:    Madeline Abio
/// Partner:   N/A
/// Date:      12/09/2024
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
///    This file contains the code for the Dependency Graph class.
/// </summary>
namespace DependencyGraph;

/// <summary>
///   <para>
///     (s1,t1) is an ordered pair of strings, meaning t1 depends on s1.
///     (in other words: s1 must be evaluated before t1.)
///   </para>
///   <para>
///     A DependencyGraph can be modeled as a set of ordered pairs of strings.
///     Two ordered pairs (s1,t1) and (s2,t2) are considered equal if and only
///     if s1 equals s2 and t1 equals t2.
///   </para>
///   <remarks>
///     Recall that sets never contain duplicates.
///     If an attempt is made to add an element to a set, and the element is already
///     in the set, the set remains unchanged.
///   </remarks>
///   <para>
///     Given a DependencyGraph DG:
///   </para>
///   <list type="number">
///     <item>
///       If s is a string, the set of all strings t such that (s,t) is in DG is called dependents(s).
///       (The set of things that depend on s.)
///     </item>
///     <item>
///       If s is a string, the set of all strings t such that (t,s) is in DG is called dependees(s).
///       (The set of things that s depends on.)
///     </item>
///   </list>
///   <para>
///      For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}.
///   </para>
///   <code>
///     dependents("a") = {"b", "c"}
///     dependents("b") = {"d"}
///     dependents("c") = {}
///     dependents("d") = {"d"}
///     dependees("a")  = {}
///     dependees("b")  = {"a"}
///     dependees("c")  = {"a"}
///     dependees("d")  = {"b", "d"}
///   </code>
/// </summary>
public class DependencyGraph
{

    private Dictionary<string, HashSet<string>> dependees; // dependent[dependee] -> set of all dependents of dependee 
    private Dictionary<string, HashSet<string>> dependents; // dependee[dependent] -> set of all dependees of dependent


    /// <summary>
    ///   Initializes a new instance of the <see cref="DependencyGraph"/> class.
    ///   The initial DependencyGraph is empty.
    /// </summary>
    public DependencyGraph()
    {
        dependents = new Dictionary<string, HashSet<string>>();
        dependees = new Dictionary<string, HashSet<string>>();

    }

    /// <summary>
    /// The number of ordered pairs in the DependencyGraph.
    /// </summary>
    public int Size
    {
        get { return dependees.Sum(kv => kv.Value.Count); }
    }

    /// <summary>
    ///   Reports whether the given node has dependents (i.e., other nodes depend on it).
    /// </summary>
    /// <param name="nodeName"> The name of the node.</param>
    /// <returns> true if the node has dependents. </returns>
    public bool HasDependents(string nodeName)
    {
        return GetDependents(nodeName).Any(); // returns true if there is any element in the interface, false else.
    }

    /// <summary>
    ///   Reports whether the given node has dependees (i.e., depends on one or more other nodes).
    /// </summary>
    /// <returns> true if the node has dependees.</returns>
    /// <param name="nodeName">The name of the node.</param>
    public bool HasDependees(string nodeName)
    {
        return GetDependees(nodeName).Any(); // returns true if there is any element in the interface, false else.
    }

    /// <summary>
    ///   <para>
    ///     Returns the dependents of the node with the given name.
    ///   </para>
    /// </summary>
    /// <param name="nodeName"> The node we are looking at.</param>
    /// <returns> The dependents of nodeName. </returns>
    public IEnumerable<string> GetDependents(string nodeName)
    {
        return dependents.ContainsKey(nodeName) ? dependents[nodeName] : new HashSet<string>(); // returns dependents if they exist, otherwise returns an empty set
    }

    /// <summary>
    ///   <para>
    ///     Returns the dependees of the node with the given name.
    ///   </para>
    /// </summary>
    /// <param name="nodeName"> The node we are looking at.</param>
    /// <returns> The dependees of nodeName. </returns>
    public IEnumerable<string> GetDependees(string nodeName)
    {
        return dependees.ContainsKey(nodeName) ? dependees[nodeName] : new HashSet<string>(); // returns dependees if they exist, otherwise returns an empty set
    }

    /// <summary>
    /// <para> 
    ///   Adds the ordered pair (dependee, dependent), if it doesn't already exist (otherwise nothing happens).
    /// </para>
    /// <para>
    ///   This can be thought of as: dependee must be evaluated before dependent.
    /// </para>
    /// </summary>
    /// <param name="dependee"> The name of the node that must be evaluated first. </param>
    /// <param name="dependent"> The name of the node that cannot be evaluated until after the other node has been. </param>
    public void AddDependency(string dependee, string dependent)
    {
        // If the dependent isn't in the graph at all, add it.
        if (!dependents.ContainsKey(dependee))
        {
            dependents[dependee] = new HashSet<string>();
        }


        // If the dependee isnt in the graph at all, add it.
        if (!dependees.ContainsKey(dependent))
        {
            dependees[dependent] = new HashSet<string>();
        }

        // Add the dependent to the dependee's dependents
        dependees[dependent].Add(dependee);

        // Add the dependee to the dependent's dependees
        dependents[dependee].Add(dependent);

    }


    /// <summary>
    ///   <para>
    ///     Removes the ordered pair (dependee, dependent), if it exists (otherwise nothing happens).
    ///   </para>
    /// </summary>
    /// <param name="dependee"> The name of the node that must be evaluated first. </param>
    /// <param name="dependent"> The name of the node that cannot be evaluated until the other node has been. </param>
    public void RemoveDependency(string dependee, string dependent)
    {
        if (dependents.ContainsKey(dependee))
        {
            dependents[dependee].Remove(dependent);
        }

        if (dependees.ContainsKey(dependent))
        {
            dependees[dependent].Remove(dependee);
        }

    }

    /// <summary>
    ///   Removes all existing ordered pairs of the form (nodeName, *).  Then, for each
    ///   t in newDependents, adds the ordered pair (nodeName, t).
    /// </summary>
    /// <param name="nodeName"> The name of the node who's dependents are being replaced. </param>
    /// <param name="newDependents"> The new dependents for nodeName. </param>
    public void ReplaceDependents(string nodeName, IEnumerable<string> newDependents)
    {

        IEnumerable<string> oldDependents = GetDependents(nodeName); // get the set of dependents from the node.

        // REMOVE OPERATIONS:
        foreach (string oldDependent in oldDependents)
        {
            RemoveDependency(nodeName, oldDependent);
        }

        // ADD OPERATIONS:
        foreach (string newDependent in newDependents) // add the new dependencies
        {
            AddDependency(nodeName, newDependent);
        }


    }


    /// <summary>
    ///   <para>
    ///     Removes all existing ordered pairs of the form (*, nodeName).  Then, for each
    ///     t in newDependees, adds the ordered pair (t, nodeName).
    ///   </para>
    /// </summary>
    /// <param name="nodeName"> The name of the node who's dependees are being replaced. </param>
    /// <param name="newDependees"> The new dependees for nodeName. Could be empty.</param>
    public void ReplaceDependees(string nodeName, IEnumerable<string> newDependees)
    {
        IEnumerable<string> oldDependees = GetDependees(nodeName); // get setof dependees from node.

        // REMOVE OPERATIONS:
        foreach (string oldDependee in oldDependees)
        {
            RemoveDependency(oldDependee, nodeName);
        }

        // ADD OPERATIONS:
        foreach (string newDependee in newDependees)
        {
            AddDependency(newDependee, nodeName);
        }
    }
}
