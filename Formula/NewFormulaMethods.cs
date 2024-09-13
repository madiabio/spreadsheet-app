// <copyright file="Formula_PS4.cs" company="UofU-CS3500">
// Copyright (c) 2024 UofU-CS3500. All rights reserved.
// </copyright>
// <authors>
//   Solution by: Joe Zachary, Daniel Kopta, Jim de St. Germain, Travis Martin
// </authors>

// Add these methods to your Formula class.

/// <summary>
///   <para>
///     Reports whether f1 == f2, using the notion of equality from the <see cref="Equals"/> method.
///   </para>
/// </summary>
/// <param name="f1"> The first of two formula objects. </param>
/// <param name="f2"> The second of two formula objects. </param>
/// <returns> true if the two formulas are the same.</returns>
public static bool operator ==( Formula f1, Formula f2 )
{
    // FIXME: Write this method
}

/// <summary>
///   <para>
///     Reports whether f1 != f2, using the notion of equality from the <see cref="Equals"/> method.
///   </para>
/// </summary>
/// <param name="f1"> The first of two formula objects. </param>
/// <param name="f2"> The second of two formula objects. </param>
/// <returns> true if the two formulas are not equal to each other.</returns>
public static bool operator !=( Formula f1, Formula f2 )
{
    // FIXME: Write this method
}

/// <summary>
///   <para>
///     Determines if two formula objects represent the same formula.
///   </para>
///   <para>
///     By definition, if the parameter is null or does not reference 
///     a Formula Object then return false.
///   </para>
///   <para>
///     Two Formulas are considered equal if their canonical string representations
///     (as defined by ToString) are equal.  
///   </para>
/// </summary>
/// <param name="obj"> The other object.</param>
/// <returns>
///   True if the two objects represent the same formula.
/// </returns>
public override bool Equals( object? obj )
{
    // FIXME: write this method
    return true;
}

/// <summary>
///   <para>
///     Evaluates this Formula, using the lookup delegate to determine the values of
///     variables.
///   </para>
///   <remarks>
///     When the lookup method is called, it will always be passed a Normalized (capitalized)
///     variable name.  The lookup method will throw an ArgumentException if there is
///     not a definition for that variable token.
///   </remarks>
///   <para>
///     If no undefined variables or divisions by zero are encountered when evaluating
///     this Formula, the numeric value of the formula is returned.  Otherwise, a 
///     FormulaError is returned (with a meaningful explanation as the Reason property).
///   </para>
///   <para>
///     This method should never throw an exception.
///   </para>
/// </summary>
/// <param name="lookup">
///   <para>
///     Given a variable symbol as its parameter, lookup returns the variable's (double) value
///     (if it has one) or throws an ArgumentException (otherwise).  This method should expect 
///     variable names to be capitalized.
///   </para>
/// </param>
/// <returns> Either a double or a formula error, based on evaluating the formula.</returns>
public object Evaluate( Lookup lookup )
{
       // FIXME: Implement the required algorithm here.
}

/// <summary>
///   <para>
///     Returns a hash code for this Formula.  If f1.Equals(f2), then it must be the
///     case that f1.GetHashCode() == f2.GetHashCode().  Ideally, the probability that two
///     randomly-generated unequal Formulas have the same hash code should be extremely small.
///   </para>
/// </summary>
/// <returns> The hashcode for the object. </returns>
public override int GetHashCode( )
{
    // FIXME: Implement the required algorithm here.
}

// FIXME: add this code to your Formula Project as well!

/// <summary>
/// Used as a possible return value of the Formula.Evaluate method.
/// </summary>
public class FormulaError
{
    /// <summary>
    ///   Initializes a new instance of the <see cref="FormulaError"/> class.
    ///   <para>
    ///     Constructs a FormulaError containing the explanatory reason.
    ///   </para>
    /// </summary>
    /// <param name="message"> Contains a message for why the error occurred.</param>
    public FormulaError( string message )
    {
        Reason = message;
    }

    /// <summary>
    ///  Gets the reason why this FormulaError was created.
    /// </summary>
    public string Reason { get; private set; }
}

/// <summary>
///   Any method meeting this type signature can be used for
///   looking up the value of a variable.  In general the expected behavior is that
///   the Lookup method will "know" about all variables in a formula
///   and return their appropriate value.
/// </summary>
/// <exception cref="ArgumentException">
///   If a variable name is provided that is not recognized by the implementing method,
///   then the method should throw an ArgumentException.
/// </exception>
/// <param name="variableName">
///   The name of the variable (e.g., "A1") to lookup.
/// </param>
/// <returns> The value of the given variable (if one exists). </returns>
public delegate double Lookup( string variableName );