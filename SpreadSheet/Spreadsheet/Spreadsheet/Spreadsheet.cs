//SpreadSheet class, which keeps track of each cell and its dependency, and Cell class which stores its value and content.
//Written by Yohan Kwak,Originally created on 09/23/2022, then modified on 9/30/2022
// Version default

using Newtonsoft.Json;
using SpreadsheetUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace SS
{

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
    public class Spreadsheet : AbstractSpreadsheet
    {
        [JsonProperty(PropertyName = "cells")]
        private Dictionary<String, Cell> cells;

        private DependencyGraph graph;

        private bool changed;


        public override bool Changed { get => this.changed; protected set => this.changed = value;}


        /// <summary>
        /// A zero-argument constructor for Spreadsheet class, which constructs an empty Spreadsheet.
        /// With default normalizer that takes variable name as itself and default validator that accepts all variable name.
        /// </summary>
        public Spreadsheet() : base((s => true), (s => s), "default")
        {
            cells = new Dictionary<string, Cell>();
            graph = new DependencyGraph();
            this.Normalize = (s => s);
            this.IsValid = (s => true);
            this.Version = "default";
            this.changed = false;

        }

        /// <summary>
        /// A constructor for Spreadsheet, which takes validator function, normalize function, and version the the spreadsheet.
        /// </summary>
        /// <param name="isValid"></param>  - A validator function this Spreadsheet to use to validate variable name.
        /// <param name="normalize"></param> - A normalize function this Spreadsheet to use to normalize variable name.
        /// <param name="version"></param> - The version of the Spreadsheet
        public Spreadsheet(Func<string, bool> isValid, Func<string, string> normalize, string version) : base(isValid, normalize, version)
        {
            cells = new Dictionary<string, Cell>();
            graph = new DependencyGraph();
            this.changed = false;
        }

        /// <summary>
        /// A constructor for Spreadsheet, which takes filePath to already existing Spreadsheet, validator function, normalize function, and version the the spreadsheet.
        /// This constructor utilizes given filepath's Spreadsheet to created a new Spreadsheet with given validator, normalize function and version.
        /// </summary>
        /// <param name="filePath"></param> - A filepath to a spreadsheet that this constructor uses to create new Spreadsheet.
        /// <param name="isValid"></param> - A validator function new Spreadsheet to use to validate variable name.
        /// <param name="normalize"></param> - A normalize function new Spreadsheet to use to normalize variable name.
        /// <param name="version"></param> - The version of new Spreadsheet
        public Spreadsheet(string filePath, Func<string, bool> isValid, Func<string, string> normalize, string version) : base(isValid, normalize, version)
        {
            cells = new Dictionary<string, Cell>();
            graph = new DependencyGraph();
            

         


            try
            {

                Spreadsheet? TempSpreadsheet = JsonConvert.DeserializeObject<Spreadsheet>(File.ReadAllText(filePath));

                if (TempSpreadsheet is not null)
                {
                    if (!this.Version.Equals(TempSpreadsheet.Version))
                    {
                        throw new SpreadsheetReadWriteException("");
                    }
                    foreach (string key in TempSpreadsheet.cells.Keys)
                    {
                        this.SetContentsOfCell(key, TempSpreadsheet.cells[key].stringForm);
                    }
                }

            }
            catch
            {
                throw new SpreadsheetReadWriteException("Failed to read file");
            }

            this.changed = false;


        }

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
        /// If there are any problems opening, writing, or closing theory. file, the method should throw a
        /// SpreadsheetReadWriteException with an explanatory message.
        /// </summary>
        public override void Save(string filename)
        {
           
            try
            {
                string ssAsJson = JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(filename, ssAsJson);
                this.changed = false;
            }
            catch
            {
                throw new SpreadsheetReadWriteException("Failed to save file");
            }

        }

        /// <summary>
        /// If name is invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the value (as opposed to the contents) of the named cell.  The return
        /// value should be either a string, a double, or a SpreadsheetUtilities.FormulaError.
        /// </summary>
        public override object GetCellValue(string name)
        {
            name = Normalize(name);

            if (isValidName(name) && IsValid(name))
            {
                if (cells.ContainsKey(name))
                {
                    return cells[name].GetValue();
                }
                else
                {
                    return "";
                }
            }
            else
            {
                throw new InvalidNameException();
            }
        }

        /// <summary>
        /// Enumerates the names of all the non-empty cells in the spreadsheet.
        /// </summary>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            return cells.Keys;
        }
        
        /// <summary>
        /// Gets the dependents of the given cell.
        /// </summary>
        /// <param name="cellName">Cell whose dependents are to be retrieved.</param>
        /// <returns>A list of the dependenet cells.</returns>
        public List<string> GetAllDependents(string cellName)
        {
            return this.GetCellsToRecalculate(cellName).ToList<string>();
        }

        /// <summary>
        /// A helper method to force other classes to save cell contents as string.
        /// </summary>
        /// <param name="name">Cell whose content should be retrieved.</param>
        /// <returns>Returns the content of the given cell. If the cell name is invalid, returns an empty string.</returns>
        /// <exception cref="InvalidNameException">Thrown if the given cellname is invalid.</exception>
        public override object GetCellContents(string name)
        {
            name = Normalize(name);

            if (isValidName(name) && IsValid(name))
            {
                if (cells.ContainsKey(name))
                {
                    return cells[name].GetContent();
                }
                else
                {
                    return "";
                }
            }
            else
            {
                throw new InvalidNameException();
            }
        }

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
        public override IList<string> SetContentsOfCell(string name, string content)
        {
            name = Normalize(name);

            if (isValidName(name) && IsValid(name))
            {
                if (double.TryParse(content, out double dValue))
                {
                    SetCellContents(name, dValue);
                }
                else if (content.Length > 0 && content[0] == '=')
                {
                    Formula f = new Formula(content.Substring(1), this.Normalize, this.IsValid);
                    SetCellContents(name, f);
                }
                else
                {
                    SetCellContents(name, content);
                }

                List<String> affectedCells = GetCellsToRecalculate(name).ToList<String>();

                if (affectedCells.Count != 1)
                {
                    Recalculate(affectedCells);
                }

                this.changed = true;

                return affectedCells;

            }
            else
            {
                throw new InvalidNameException();
            }
        }

        public IList<string> SetStringContent(string name, string text)
        {
            if (text != "")
            {
                graph.ReplaceDependees(name, new List<String>());
                if (cells.ContainsKey(name))
                {
                    cells[name].modifyCell(text);
                }
                else
                {
                    cells.Add(name, new Cell(text));
                }
                return GetCellsToRecalculate(name).ToList<string>();
            }
            else
            {
                graph.ReplaceDependees(name, new List<String>());
                cells.Remove(name);
                return GetCellsToRecalculate(name).ToList<string>();
            }
        }


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
        /// <param name="name"></param> the name of the cell which values to be set on
        /// <param name="number"></param> the double value to be set in the cell.
        /// <returns></returns>
        /// <exception cref="InvalidNameException"></exception> is thrown when the given name is not a valid form of name.
        protected override IList<string> SetCellContents(string name, double number)
        {
            graph.ReplaceDependees(name, new List<String>());
            if (cells.ContainsKey(name))
            {
                cells[name].modifyCell(number);
            }
            else
            {
                cells.Add(name, new Cell(number));
            }
            return GetCellsToRecalculate(name).ToList<string>();
        }

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
        /// <param name="name"></param> the name of the cell which values to be set on
        /// <param name="text"></param> the String value to be set in the cell.
        /// <returns></returns>
        /// <exception cref="InvalidNameException"></exception> is thrown when the given name is not a valid form of name.
        protected override IList<string> SetCellContents(string name, string text)
        {
            if (text != "")
            {
                graph.ReplaceDependees(name, new List<String>());
                if (cells.ContainsKey(name))
                {
                    cells[name].modifyCell(text);
                }
                else
                {
                    cells.Add(name, new Cell(text));
                }
                return GetCellsToRecalculate(name).ToList<string>();
            }
            else
            {
                graph.ReplaceDependees(name, new List<String>());
                cells.Remove(name);
                return GetCellsToRecalculate(name).ToList<string>();
            }
        }

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
        /// <param name="name"></param> the name of the cell which values to be set on
        /// <param name="formula"></param> the Formula value to be set in the cell.
        /// <returns></returns>
        /// <exception cref="InvalidNameException"></exception> is thrown when the given name is not a valid form of name.
        protected override IList<string> SetCellContents(string name, Formula formula)
        {
            List<string> prevDependees = graph.GetDependees(name).ToList();
            List<string> dependees = formula.GetVariables().ToList();
            graph.ReplaceDependees(name, dependees);
            IList<string> affectedCells;

            try
            {
                affectedCells = GetCellsToRecalculate(name).ToList<string>();
            }
            catch (CircularException)
            {
                graph.ReplaceDependees(name, prevDependees);
                throw new CircularException();
            }

            if (cells.ContainsKey(name))
            {
                cells[name].modifyCell(formula, formula.Evaluate(GetVariableValue));
            }
            else
            {
                cells.Add(name, new Cell(formula, formula.Evaluate(GetVariableValue)));
            }

            return affectedCells;

        }


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
        /// <param name="name"></param> the cell to return direct dependents of.
        /// <returns></returns> the IEnumerable of string, of direct dependents of given name's cell.
        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            name = Normalize(name);

            return graph.GetDependents(name);
        }

        /// <summary>
        /// Checks if the given name is valid input for this Spreadsheet class.
        /// Name needs to be any number of letters followed by anynumber of digits, in that strict order.
        /// </summary>
        /// <param name="name"></param> name to be evaluated.
        /// <returns></returns> if the name is in valid form.
        private bool isValidName(string name)
        {
            bool isVar = Regex.IsMatch(name, @"^[a-zA-Z]*[0-9]*$");
            return isVar;
        }

        /// <summary>
        /// A private helper method for getting the cell's value(when it was being found as variable).
        /// the value is expected to be double.
        /// an error is thrown when it is not a double value being returned.
        /// </summary>
        /// <param name="name"></param> the variable name(cell's name) being found
        /// <returns></returns> returns a double value of given name cell.
        /// <exception cref="ArgumentException"></exception> if the value was unassigned or the value is not double.
        private double GetVariableValue(string name)
        {
            if (cells.ContainsKey(name))
            {
                if (Double.TryParse(cells[name].GetValue().ToString(), out double outcome))
                {
                    return outcome;
                }
                else
                {
                    throw new ArgumentException();
                }
            }
            else
            {
                throw new ArgumentException();
            }

        }

        private void Recalculate(List<string> names)
        {
            bool isFirst = true;
            foreach(string name in names)
            {
                if (isFirst)
                {
                    isFirst = false;
                }
                else
                {
                    SetCellContents(name, (Formula) cells[name].GetContent());
                }
            }
        }
        [JsonObject(MemberSerialization.OptIn)]
        /// <summary>
        /// A Cell class being used by Spreadsheet class, and cell 
        /// class keeps track of each cell's value and contents.
        /// </summary>
        private class Cell
        {

            private object value;
            [JsonProperty]
            public string stringForm { get; private set; }
            private object content;
            private bool isFuncCell;

            public Cell()
            {
                value = "";
                stringForm = "";
                content = "";
                isFuncCell = false;
            }

            /// <summary>
            /// Constructor with String Content
            /// </summary>
            /// <param name="StringCont"></param> String content that will be the value of this cell.
            public Cell(string StringCont)
            {
                this.content = StringCont;
                this.value = StringCont;
                isFuncCell = false;
                stringForm = StringCont;

            }
            /// <summary>
            /// Constructor with double content
            /// </summary>
            /// <param name="doubleCont"></param> Double content that will be the value of this cell.
            public Cell(double doubleCont)
            {
                this.content = doubleCont.ToString();
                this.value = doubleCont.ToString();
                isFuncCell = false;
                stringForm = doubleCont.ToString();
            }
            /// <summary>
            /// Constructor with formula, and value which is evaluated.
            /// </summary>
            /// <param name="formula"></param> A formula to be stored in Content field
            /// <param name="val"></param> A value to be stored in cell's value field
            public Cell(Formula formula, Object val)
            {
                this.content = formula;
                this.value = val;
                isFuncCell = true;
                stringForm = "=" + formula.ToString();
            }
            /// <summary>
            /// returns the value of the cell
            /// </summary>
            /// <returns></returns> the value stored in cell.
            public object GetValue()
            {
                if (!isFuncCell)
                {
                    if (double.TryParse(this.content.ToString(), out double val))
                        return val;
                    else
                        return this.content;
                }
                else
                {
                    if (value is FormulaError)
                    {
                        return (FormulaError)value;
                    }
                    else
                    {
                        return (double)value;
                    }
                }
            }

            public object GetContent()
            {
                if (!isFuncCell)
                {
                    if (double.TryParse(this.content.ToString(), out double val))
                        return val;
                    else
                        return this.content;
                }
                else
                {
                    return (Formula) this.content;
                }
            }


            /// <summary>
            /// modify the content and value of the cell.
            /// </summary>
            /// <param name="val"></param> Double value to be saved in the value and content field.
            public void modifyCell(double val)
            {
                this.content = val.ToString();
                this.value = val.ToString();
                isFuncCell = false;
                stringForm = val.ToString();

            }
            /// <summary>
            /// modify the content and value of the cell.
            /// </summary>
            /// <param name="text"></param> String value to be saved in the value and content field.
            public void modifyCell(string text)
            {
                this.content = text;
                this.value = text;
                isFuncCell = false;
                stringForm = text;
            }
            /// <summary>
            /// modify the content and value of the cell.
            /// </summary>
            /// <param name="formula"></param> Formula to be saved in content field
            /// <param name="val"></param> Value to be saved in value field
            public void modifyCell(Formula formula, object val)
            {
                this.content = formula;
                this.value = val;
                isFuncCell = true;
                stringForm = "=" + formula.ToString();
            }
            public bool isFormula()
            {
                return isFuncCell;
            }

        }
    }
}
