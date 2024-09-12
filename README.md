```
Author:	Madeline Abio
Partner: None
Start Date: 21/08/2024
Course: CS 3500 Software Practice, University of Utah, School of Computing
GitHub ID: madiabio
Repo: https://github.com/uofu-cs3500-20-fall2024/spreadsheet-madiabio
Date: 07/09/2024 1:12pm (when submission was completed)
Project: Spreadsheet
Copyright: CS 3500 and Madeline Abio - This work may not be copied for use in Academic Coursework
```

# Comments to Evaluators:
Assignment stands on its own

# Assignment Specific Topics:

Hours
Estimated/Worked,   Assignment,        Note

    3/4,            Assignment 2,      N/A
    3,              Assignment 3,      N/A


# Consulted Peers:
	1. Sadie Bowen

# References:
	1. C# Documentation - https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/
	2. ChatGPT - https://chatgpt.com/
	3. Title of page - URL

# ChatGPT:

__Query 1:__
~~~
can you fix this regex pattern for me:
/// <summary>
/// Only alphanumeric characters or (, ), +, -, *, / are valid in a token.
/// </summary>
private const string ValidTokensPattern = @"^[a-zA-Z0-9()+-*/]+$";
~~~
_Respose:_
~~~
private const string ValidTokensPattern = @"^[a-zA-Z0-9\(\)\+\-\*/]+$";
~~~

__Query 2.__
~~~
how do you remove unnecessary .00s etc from a string that represents a number? (ie, "5.000" to "5") with regex
~~~

_Response:_
~~~
To remove unnecessary trailing .00 (or other trailing zeros) from a string representing a number, you can use a regular expression to match these patterns and replace them.

Here's a step-by-step approach:

Pattern: The regex pattern should identify:

Trailing zeroes after a decimal point.
Remove the decimal point itself if all digits after the decimal are zeros.
Regex Pattern:

Pattern: @"\.?0+$"
\.: Matches the decimal point (if it exists).
0+: Matches one or more zeros.
?: Makes the decimal point optional (in case there are only trailing zeros).
$: Ensures it matches only at the end of the string.
C# Example:
csharp
Copy code
using System;
using System.Text.RegularExpressions;

public class Program
{
    public static void Main()
    {
        string[] numbers = { "5.000", "123.4500", "10.00", "3.14159", "0.00" };

        foreach (string number in numbers)
        {
            string cleanedNumber = RemoveTrailingZeros(number);
            Console.WriteLine(cleanedNumber);  // Output: 5, 123.45, 10, 3.14159, 0
        }
    }

    public static string RemoveTrailingZeros(string number)
    {
        return Regex.Replace(number, @"\.?0+$", "");
    }
}
~~~
__Query 3.__
~~~
how to match if a string is (, +, -, *, /
~~~
_Response_
~~~
To match a string that consists of one of the characters (, +, -, *, / in C#, you can use the following regular expression:

Regex Pattern:
^[()+\-*/]$
~~~