﻿// Written by Joe Zachary for CS 3500, September 2012
// Version 1.8
// Revision history:  
//   Version 1.1 9/20/12 12:59 p.m.  Fixed comment that describes circular dependencies
//   Version 1.2 9/20/12 1:38 p.m.   Changed return type of GetCellContents to object
//   Version 1.3 9/24/12 8:41 a.m.   Modified the specification of GetCellsToRecalculate by 
//                                   adding a requirement for the names parameter
// Branched from PS4Skeleton
//   Version 1.4                     Branched from PS4Skeleton
//           Edited class comment for AbstractSpreadsheet
//           Made the three SetCellContents methods protected
//           Added a new method SetContentsOfCell.  This method abstract.
//           Added a new method GetCellValue.  This method is abstract.
//           Added a new property Changed.  This property is abstract.
//           Added a new method Save.  This method is abstract.
//           Added a new method GetSavedVersion.  This method is abstract.
//           Added a new class SpreadsheetReadWriteException.
//           Added IsValid, Normalize, and Version properties
//           Added a constructor for AbstractSpreadsheet

// Revision history:
//    Version 1.5 9/28/12 2:22 p.m.   Fixed example in comment for Save
//    Version 1.6 9/29/12 11:07 a.m.  Put a constructor into SpreadsheetReadWriteException
//    Version 1.7 9/29/12 11:14 a.m.  Added missing </summary> tag to comment
//    
//    Version 1.8 (Daniel Kopta)      Unified error checking for setting cell contents
//                (Fall 2019)         SetCellContents signatures changed to return IList
//                            
//    Version 1.9 (Daniel Kopta)      Removed GetSavedVersion
//                (Fall 2022)


using SpreadsheetUtilities;
using System.Text.Json;
using Newtonsoft.Json;

namespace SS
{
    /// <summary>
    /// Thrown to indicate that a change to a cell will cause a circular dependency.
    /// </summary>
    public class CircularException : Exception
    {
    }


    /// <summary>
    /// Thrown to indicate that a name parameter was invalid.
    /// </summary>
    public class InvalidNameException : Exception
    {
    }


    // ADDED FOR PS5
    /// <summary>
    /// Thrown to indicate that a read or write attempt has failed.
    /// </summary>
    public class SpreadsheetReadWriteException : Exception
    {
        /// <summary>
        /// Creates the exception with a message
        /// </summary>
        public SpreadsheetReadWriteException(string msg)
            : base(msg)
        {
        }
    }


    // Modified for PS5.
    /// <summary>
    /// An AbstractSpreadsheet object represents the state of a simple spreadsheet.  A 
    /// spreadsheet consists of an infinite number of named cells.
    /// 
    /// A string is a cell name if and only if it consists of one or more letters,
    /// followed by one or more digits AND it satisfies the predicate IsValid.
    /// For example, "A15", "a15", "XY032", and "BC7" are cell names so long as they
    /// satisfy IsValid.  On the other hand, "Z", "X_", and "hello" are not cell names,
    /// regardless of IsValid.
    /// 
    /// Any valid incoming cell name, whether passed as a parameter or embedded in a formula,
    /// must be normalized with the Normalize method before it is used by or saved in 
    /// this spreadsheet.  For example, if Normalize is s => s.ToUpper(), then
    /// the Formula "x3+a5" should be converted to "X3+A5" before use.
    /// 
    /// A spreadsheet contains a cell corresponding to every possible cell name.  
    /// In addition to a name, each cell has a contents and a value.  The distinction is
    /// important.
    /// 
    /// The contents of a cell can be (1) a string, (2) a double, or (3) a Formula.  If the
    /// contents is an empty string, we say that the cell is empty.  (By analogy, the contents
    /// of a cell in Excel is what is displayed on the editing line when the cell is selected.)
    /// 
    /// In a new spreadsheet, the contents of every cell is the empty string.
    ///  
    /// The value of a cell can be (1) a string, (2) a double, or (3) a FormulaError.  
    /// (By analogy, the value of an Excel cell is what is displayed in that cell's position
    /// in the grid.)
    /// 
    /// If a cell's contents is a string, its value is that string.
    /// 
    /// If a cell's contents is a double, its value is that double.
    /// 
    /// If a cell's contents is a Formula, its value is either a double or a FormulaError,
    /// as reported by the Evaluate method of the Formula class.  The value of a Formula,
    /// of course, can depend on the values of variables.  The value of a variable is the 
    /// value of the spreadsheet cell it names (if that cell's value is a double) or 
    /// is undefined (otherwise).
    /// 
    /// Spreadsheets are never allowed to contain a combination of Formulas that establish
    /// a circular dependency.  A circular dependency exists when a cell depends on itself.
    /// For example, suppose that A1 contains B1*2, B1 contains C1*2, and C1 contains A1*2.
    /// A1 depends on B1, which depends on C1, which depends on A1.  That's a circular
    /// dependency.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class AbstractSpreadsheet
    {

        // ADDED FOR PS5
        /// <summary>
        /// True if this spreadsheet has been modified since it was created or saved                  
        /// (whichever happened most recently); false otherwise.
        /// </summary>
        public abstract bool Changed { get; protected set; }

        // ADDED FOR PS5
        /// <summary>
        /// Method used to determine whether a string that consists of one or more letters
        /// followed by one or more digits is a valid variable name.
        /// </summary>
        public Func<string, bool> IsValid { get; protected set; }

        // ADDED FOR PS5
        /// <summary>
        /// Method used to convert a cell name to its standard form.  For example,
        /// Normalize might convert names to upper case.
        /// </summary>
        public Func<string, string> Normalize { get; protected set; }

        // ADDED FOR PS5
        /// <summary>
        /// Version information
        /// </summary>
        [JsonProperty]
        public string Version { get; protected set; }

        // ADDED FOR PS5
        /// <summary>
        /// Constructs an abstract spreadsheet by recording its variable validity test,
        /// its normalization method, and its version information.  The variable validity
        /// test is used throughout to determine whether a string that consists of one or
        /// more letters followed by one or more digits is a valid cell name.  The variable
        /// equality test should be used thoughout to determine whether two variables are
        /// equal.
        /// </summary>
        public AbstractSpreadsheet(Func<string, bool> isValid, Func<string, string> normalize, string version)
        {
            this.IsValid = isValid;
            this.Normalize = normalize;
            this.Version = version;
        }


        // ADDED FOR PS5
        /// <summary>
        /// Writes the contents of this spreadsheet to the named file using a JSON format.
        /// The JSON object should have the following fields:
        /// "Version" - the version of the spreadsheet software (a string)
        /// "cells" - an object containing 0 or more cell objects
        ///           Each cell object has a field named after the cell itself 
        ///           The value of that field is another object representing the cell's contents
        ///               The contents object has a single field called "stringForm",
        ///               representing the string form of the cell's contents
        ///               - If the contents is a string, the value of stringForm is that string
        ///               - If the contents is a double d, the value of stringForm is d.ToString()
        ///               - If the contents is a Formula f, the value of stringForm is "=" + f.ToString()
        /// 
        /// For example, if this spreadsheet has a version of "default" 
        /// and contains a cell "A1" with contents being the double 5.0 
        /// and a cell "B3" with contents being the Formula("A1+2"), 
        /// a JSON string produced by this method would be:
        /// 
        /// {
        ///   "cells": {
        ///     "A1": {
        ///       "stringForm": "5"
        ///     },
        ///     "B3": {
        ///       "stringForm": "=A1+2"
        ///     }
        ///   },
        ///   "Version": "default"
        /// }
        /// 
        /// If there are any problems opening, writing, or closing the file, the method should throw a
        /// SpreadsheetReadWriteException with an explanatory message.
        /// </summary>
        public abstract void Save(string filename);

        // ADDED FOR PS5
        /// <summary>
        /// If name is invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the value (as opposed to the contents) of the named cell.  The return
        /// value should be either a string, a double, or a SpreadsheetUtilities.FormulaError.
        /// </summary>
        public abstract object GetCellValue(string name);

        /// <summary>
        /// Enumerates the names of all the non-empty cells in the spreadsheet.
        /// </summary>
        public abstract IEnumerable<string> GetNamesOfAllNonemptyCells();

        /// <summary>
        /// If name is invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the contents (as opposed to the value) of the named cell.  The return
        /// value should be either a string, a double, or a Formula.
        /// </summary>
        public abstract object GetCellContents(string name);

        // ADDED FOR PS5
        /// <summary>
        /// If name is invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, if content parses as a double, the contents of the named
        /// cell becomes that double.
        /// 
        /// Otherwise, if content begins with the character '=', an attempt is made
        /// to parse the remainder of content into a Formula f using the Formula
        /// constructor.  There are then three possibilities:
        /// 
        ///   (1) If the remainder of content cannot be parsed into a Formula, a 
        ///       SpreadsheetUtilities.FormulaFormatException is thrown.
        ///       
        ///   (2) Otherwise, if changing the contents of the named cell to be f
        ///       would cause a circular dependency, a CircularException is thrown,
        ///       and no change is made to the spreadsheet.
        ///       
        ///   (3) Otherwise, the contents of the named cell becomes f.
        /// 
        /// Otherwise, the contents of the named cell becomes content.
        /// 
        /// If an exception is not thrown, the method returns a list consisting of
        /// name plus the names of all other cells whose value depends, directly
        /// or indirectly, on the named cell. The order of the list should be any
        /// order such that if cells are re-evaluated in that order, their dependencies 
        /// are satisfied by the time they are evaluated.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// list {A1, B1, C1} is returned.
        /// </summary>
        public abstract IList<string> SetContentsOfCell(string name, string content);

        // MODIFIED PROTECTION FOR PS5
        /// <summary>
        /// The contents of the named cell becomes number.  The method returns a
        /// list consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell. The order of the list should be any
        /// order such that if cells are re-evaluated in that order, their dependencies 
        /// are satisfied by the time they are evaluated.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// list {A1, B1, C1} is returned.
        /// </summary>
        protected abstract IList<string> SetCellContents(string name, double number);

        // MODIFIED PROTECTION FOR PS5
        /// <summary>
        /// The contents of the named cell becomes text.  The method returns a
        /// list consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell. The order of the list should be any
        /// order such that if cells are re-evaluated in that order, their dependencies 
        /// are satisfied by the time they are evaluated.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// list {A1, B1, C1} is returned.
        /// </summary>
        protected abstract IList<string> SetCellContents(string name, string text);

        // MODIFIED PROTECTION FOR PS5
        /// <summary>
        /// If changing the contents of the named cell to be the formula would cause a 
        /// circular dependency, throws a CircularException, and no change is made to the spreadsheet.
        /// 
        /// Otherwise, the contents of the named cell becomes formula. The method returns a
        /// list consisting of name plus the names of all other cells whose value depends,
        /// directly or indirectly, on the named cell. The order of the list should be any
        /// order such that if cells are re-evaluated in that order, their dependencies 
        /// are satisfied by the time they are evaluated.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// list {A1, B1, C1} is returned.
        /// </summary>
        protected abstract IList<string> SetCellContents(string name, Formula formula);


        /// <summary>
        /// Returns an enumeration, without duplicates, of the names of all cells whose
        /// values depend directly on the value of the named cell.  In other words, returns
        /// an enumeration, without duplicates, of the names of all cells that contain
        /// formulas containing name.
        /// 
        /// For example, suppose that
        /// A1 contains 3
        /// B1 contains the formula A1 * A1
        /// C1 contains the formula B1 + A1
        /// D1 contains the formula B1 - C1
        /// The direct dependents of A1 are B1 and C1
        /// </summary>
        protected abstract IEnumerable<string> GetDirectDependents(string name);


        /// <summary>
        /// This method is implemented for you, but makes use of your GetDirectDependents.
        /// 
        /// Requires that name be a valid cell name.
        /// 
        /// If the cell referred to by name is involved in a circular dependency,
        /// throws a CircularException.
        /// 
        /// Otherwise, returns an enumeration of the names of all cells whose values must
        /// be recalculated, assuming that the contents of the cell referred to by name has changed.
        /// The cell names are enumerated in an order in which the calculations should be done.  
        /// 
        /// For example, suppose that 
        /// A1 contains 5
        /// B1 contains the formula A1 + 2
        /// C1 contains the formula A1 + B1
        /// D1 contains the formula A1 * 7
        /// E1 contains 15
        /// 
        /// If A1 has changed, then A1, B1, C1, and D1 must be recalculated,
        /// and they must be recalculated in an order which has A1 first, and B1 before C1
        /// (there are multiple such valid orders).
        /// The method will produce one of those enumerations.
        /// 
        /// PLEASE NOTE THAT THIS METHOD DEPENDS ON THE ABSTRACT METHOD GetDirectDependents.
        /// IT WON'T WORK UNTIL GetDirectDependents IS IMPLEMENTED CORRECTLY.
        /// </summary>
        protected IEnumerable<string> GetCellsToRecalculate(string name)
        {
            LinkedList<string> changed = new LinkedList<string>();
            HashSet<string> visited = new HashSet<string>();
            Visit(name, name, visited, changed);
            return changed;
        }


        /// <summary>
        /// A helper for the GetCellsToRecalculate method.
        /// 
        ///   -- You should fully comment what is going on below --
        /// </summary>
        private void Visit(string start, string name, ISet<string> visited, LinkedList<string> changed)
        {
            visited.Add(name);
            foreach (string n in GetDirectDependents(name))
            {
                if (n.Equals(start))
                {
                    throw new CircularException();
                }
                else if (!visited.Contains(n))
                {
                    Visit(start, n, visited, changed);
                }
            }
            changed.AddFirst(name);
        }

    }
}