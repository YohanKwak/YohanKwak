// Written by Joe Zachary and Travis Martin and Yohan Kwak and Simon Whidden for CS 3500, September 2011, 2021, October 21st, 2022.
using Font = Microsoft.Maui.Graphics.Font;
using SizeF = Microsoft.Maui.Graphics.SizeF;
using PointF = Microsoft.Maui.Graphics.PointF;
using System.Text.RegularExpressions;
using SpreadsheetUtilities;
using SpreadsheetGUI;
using System.Numerics;
using System.Linq.Expressions;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System.Text;

namespace SS;

/// <summary>
/// The type of delegate used to register for SelectionChanged events
/// </summary>
/// <param name="sender">Event sender</param>
public delegate void SelectionChangedHandler(SpreadsheetGrid sender);

/// <summary>
/// A grid that displays a spreadsheet with 26 columns (labeled A-Z) and 99 rows
/// (labeled 1-99).  Each cell on the grid can display a non-editable string.  One 
/// of the cells is always selected (and highlighted).  When the selection changes, a 
/// SelectionChanged event is fired.  Clients can register to be notified of
/// such events.
/// </summary>
public class SpreadsheetGrid : ScrollView, IDrawable
{
    /// <summary>
    /// The event used to send notifications of a selection change
    /// </summary>
    public event SelectionChangedHandler SelectionChanged;

    // These constants control the layout of the spreadsheet grid.
    // The height and width measurements are in pixels.
    private const int DATA_COL_WIDTH = 80;
    private const int DATA_ROW_HEIGHT = 20;
    private const int LABEL_COL_WIDTH = 30;
    private const int LABEL_ROW_HEIGHT = 30;
    private const int PADDING = 4;
    private const int COL_COUNT = 26;
    private const int ROW_COUNT = 99;
    private const int FONT_SIZE = 12;

    // Columns and rows are numbered beginning with 0.  This is the coordinate
    // of the selected cell.
    private int _selectedCol;
    private int _selectedRow;

    // Coordinate of cell in upper-left corner of display
    private int _firstColumn = 0;
    private int _firstRow = 0;

    // Scrollbar positions
    private double _scrollX = 0;
    private double _scrollY = 0;

    // The strings contained by this grid
    private Dictionary<Address, String> _values = new();

    // GraphicsView maintains the actual drawing of the grid and listens
    // for click events
    private GraphicsView graphicsView = new();

    // The SpreadSheet Object to keep track of the content and value of cells, and is also used to save / load.
    private Spreadsheet _sheet = new Spreadsheet(s => true, s => s.ToUpper(), "ps6");

    /// <summary>
    /// Constructor for the spreadsheet grid.
    /// </summary>
    public SpreadsheetGrid()
    {
        BackgroundColor = Colors.LightGray;
        graphicsView.Drawable = this;
        graphicsView.HeightRequest = LABEL_ROW_HEIGHT + (ROW_COUNT + 1) * DATA_ROW_HEIGHT;
        graphicsView.WidthRequest = LABEL_COL_WIDTH + (COL_COUNT + 1) * DATA_COL_WIDTH;
        graphicsView.BackgroundColor = Colors.LightGrey;
        graphicsView.EndInteraction += OnEndInteraction;
        this.Content = graphicsView;
        this.Scrolled += OnScrolled;
        this.Orientation = ScrollOrientation.Both;
    }

    /// <summary>
    /// Clears the display.
    /// </summary>
    public void Clear()
    {
        _values.Clear();
        _sheet = new Spreadsheet(s => true, s => s.ToUpper(), "ps6");
        Invalidate();
    }

    /// <summary>
    /// If the zero-based column and row are in range, sets the value of that
    /// cell and returns true.  Otherwise, returns false.
    /// </summary>
    /// <param name="col">Column</param>
    /// <param name="row">=Row</param>
    /// <param name="c">Cell content</param>
    /// <returns></returns>
    public bool SetValue(int col, int row, string c)
    {
        //checks if the given address(col, val) is valid
        if (InvalidAddress(col, row))
        {
            return false;
        }

        Address a = new Address(col, row);
        //Converts col, row data into a cellName, i.e. A1, Z99, etc.
        string cellName = ((char)(65 + col)).ToString() + (row + 1).ToString();

        try
        {
            //If cell contents are empty or null, reset the spreadsheet and cell dictionary and updates the cell's dependents.
            if (c == null || c == "")
            {
                _values.Remove(a);

                List<string> affected = _sheet.SetContentsOfCell(cellName, "").ToList<string>();
                affected.RemoveAt(0);
                foreach (string name in affected)
                {
                    if (_sheet.GetCellValue(name) is FormulaError)
                    {
                        _values[new Address(((int)(name[0]) - 65), (int.Parse(name.Substring(1))) - 1)] = _sheet.GetCellContents(name).ToString();
                    }
                    else
                    {
                        _values[new Address(((int)(name[0]) - 65), (int.Parse(name.Substring(1))) - 1)] = _sheet.GetCellValue(name).ToString();
                    }
                }
            }
            //If the cell has valid contents, sets its value, updates its dependents.
            else
            {
                List<string> affected = _sheet.SetContentsOfCell(cellName, c).ToList<string>();
                foreach (string name in affected)
                {
                    if (_sheet.GetCellValue(name) is FormulaError)
                    {
                        _values[new Address(((int)(name[0]) - 65), (int.Parse(name.Substring(1))) - 1)] = _sheet.GetCellContents(name).ToString();
                    }
                    else
                    {
                        _values[new Address(((int)(name[0]) - 65), (int.Parse(name.Substring(1))) - 1)] = _sheet.GetCellValue(name).ToString();
                    }
                }

            }
        }
        //If cell contents are a formula that is invalid, saves as a string.
        catch (FormulaFormatException)
        {
            _values[a] = c;
            List<string> affected = _sheet.SetStringContent(cellName, c).ToList<string>();
            foreach (string name in affected)
            {
                if (!name.Equals(cellName))
                {
                    _values[new Address(((int)(name[0]) - 65), (int.Parse(name.Substring(1))) - 1)] = _sheet.GetCellContents(name).ToString();
                }
            }
        }

        Invalidate();
        return true;
    }

    /// <summary>
    /// If the zero-based column and row are in range, assigns the value
    /// of that cell to the out parameter and returns true.  Otherwise,
    /// returns false.
    /// </summary>\
    /// <param name="col">Column</param>
    /// <param name="row">Row</param>
    /// <param name="c">Cell Contents</param>
    /// <returns>A boolean indicating whether or not the value was retrieved.</returns>
    public bool GetValue(int col, int row, out string c)
    {
        if (InvalidAddress(col, row))
        {
            c = null;
            return false;
        }
        string cellName = ((char)(65 + col)).ToString() + (row + 1).ToString();
        Object data = _sheet.GetCellValue(cellName);

        if (data is FormulaError)
        {
            c = _values[new Address(col, row)];
        }
        else
        {
            c = data.ToString();
        }

        return true;
    }

    /// <summary>
    /// If the zero-based column and row are in range, assigns the content
    /// of that cell to the out parameter and returns true.  Otherwise,
    /// returns false.
    /// </summary>
    /// <param name="col">Column</param>
    /// <param name="row">Row</param>
    /// <param name="c">Cell Contents</param>
    /// <returns>A boolean indicating whether or not the content was retrieved.</returns>
    public bool GetContent(int col, int row, out String c)
    {
        if (InvalidAddress(col, row))
        {
            c = null;
            return false;
        }
        string cellName = ((char)(65 + col)).ToString() + (row + 1).ToString();

        if (_sheet.GetCellContents(cellName) is Formula)
        {
            c = "=" + _sheet.GetCellContents(cellName).ToString();
        }
        else
        {
            c = _sheet.GetCellContents(cellName).ToString();
        }

        return true;
    }

    /// <summary>
    /// If the zero-based column and row are in range, uses them to set
    /// the current selection and returns true.  Otherwise, returns false.
    /// </summary>
    /// <param name="col">Column</param>
    /// <param name="row">Row</param>
    /// <returns>A boolean indicating whether or not the value was successfully set.</returns>
    public bool SetSelection(int col, int row)
    {
        if (InvalidAddress(col, row))
        {
            return false;
        }
        _selectedCol = col;
        _selectedRow = row;
        Invalidate();
        return true;
    }

    /// <summary>
    /// Assigns the column and row of the current selection to the
    /// out parameters.
    /// </summary>
    /// <param name="col">Column</param>
    /// <param name="row">Row</param>
    public void GetSelection(out int col, out int row)
    {
        col = _selectedCol;
        row = _selectedRow;
    }

    /// <summary>
    /// Checks to see if the given column and row values are valid.
    /// </summary>
    /// <param name="col">Column</param>
    /// <param name="row">Row</param>
    /// <returns></returns>
    private bool InvalidAddress(int col, int row)
    {
        return col < 0 || row < 0 || col >= COL_COUNT || row >= ROW_COUNT;
    }

    /// <summary>
    /// Listener for click events on the grid.
    /// </summary>
    private void OnEndInteraction(object sender, TouchEventArgs args)
    {
        PointF touch = args.Touches[0];
        OnMouseClick(touch.X, touch.Y);
    }

    /// <summary>
    /// Listener for scroll events. Redraws the panel, maintaining the
    /// row and column headers.
    /// </summary>
    private void OnScrolled(object sender, ScrolledEventArgs e)
    {
        _scrollX = e.ScrollX;
        _firstColumn = (int)e.ScrollX / DATA_COL_WIDTH;
        _scrollY = e.ScrollY;
        _firstRow = (int)e.ScrollY / DATA_ROW_HEIGHT;
        Invalidate();
    }

    /// <summary>
    /// Determines which cell, if any, was clicked.  Generates a SelectionChanged
    /// event.  All of the indexes are zero based.
    /// </summary>
    /// <param name="eventX">EventX coordinate</param>
    /// <param name="eventY">EventY coordinate</param>
    private void OnMouseClick(float eventX, float eventY)
    {
        int x = (int)(eventX - _scrollX - LABEL_COL_WIDTH) / DATA_COL_WIDTH + _firstColumn;
        int y = (int)(eventY - _scrollY - LABEL_ROW_HEIGHT) / DATA_ROW_HEIGHT + _firstRow;
        if (eventX > LABEL_COL_WIDTH && eventY > LABEL_ROW_HEIGHT && (x < COL_COUNT) && (y < ROW_COUNT))
        {
            _selectedCol = x;
            _selectedRow = y;
            if (SelectionChanged != null)
            {
                SelectionChanged(this);
            }
        }
        Invalidate();
    }

    /// <summary>
    /// Redraws the grid.
    /// </summary>
    private void Invalidate()
    {
        graphicsView.Invalidate();
    }

    /// <summary>
    /// Used internally to keep track of cell addresses
    /// </summary>
    private class Address
    {
        public int Col { get; set; }
        public int Row { get; set; }

        public Address(int c, int r)
        {
            Col = c;
            Row = r;
        }

        /// <summary>
        /// Gets the hashcode for the columns and the rows.
        /// </summary>
        /// <returns>Returns the hashcode of the cell's coordinates.</returns>
        public override int GetHashCode()
        {
            return Col.GetHashCode() ^ Row.GetHashCode();
        }

        /// <summary>
        /// Compares two objects, returns true if equal, false if not.
        /// </summary>
        /// <param name="obj">Object being compared.</param>
        /// <returns>true or false</returns>
        public override bool Equals(object obj)
        {
            if ((obj == null) || !(obj is Address))
            {
                return false;
            }
            Address a = (Address)obj;
            return Col == a.Col && Row == a.Row;
        }
    }

    /// <summary>
    /// Draws the grid.
    /// </summary>
    /// <param name="canvas">MAUI graphics canvas</param>
    /// <param name="dirtyRect">MAUI graphics rectangle</param>
    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        // Move the canvas to the place that needs to be drawn.
        canvas.SaveState();
        canvas.Translate((float)_scrollX, (float)_scrollY);

        // Color the background of the data area white
        canvas.FillColor = Colors.White;
        canvas.FillRectangle(
            LABEL_COL_WIDTH,
            LABEL_ROW_HEIGHT,
            (COL_COUNT - _firstColumn) * DATA_COL_WIDTH,
            (ROW_COUNT - _firstRow) * DATA_ROW_HEIGHT);

        // Draw the column lines
        int bottom = LABEL_ROW_HEIGHT + (ROW_COUNT - _firstRow) * DATA_ROW_HEIGHT;
        canvas.DrawLine(0, 0, 0, bottom);
        for (int x = 0; x <= (COL_COUNT - _firstColumn); x++)
        {
            canvas.DrawLine(
                LABEL_COL_WIDTH + x * DATA_COL_WIDTH, 0,
                LABEL_COL_WIDTH + x * DATA_COL_WIDTH, bottom);
        }

        // Draw the column labels
        for (int x = 0; x < COL_COUNT - _firstColumn; x++)
        {
            DrawColumnLabel(canvas, x,
                (_selectedCol - _firstColumn == x) ? Font.Default : Font.DefaultBold);
        }

        // Draw the row lines
        int right = LABEL_COL_WIDTH + (COL_COUNT - _firstColumn) * DATA_COL_WIDTH;
        canvas.DrawLine(0, 0, right, 0);
        for (int y = 0; y <= ROW_COUNT - _firstRow; y++)
        {
            canvas.DrawLine(
                0, LABEL_ROW_HEIGHT + y * DATA_ROW_HEIGHT,
                right, LABEL_ROW_HEIGHT + y * DATA_ROW_HEIGHT);
        }

        // Draw the row labels
        for (int y = 0; y < (ROW_COUNT - _firstRow); y++)
        {
            DrawRowLabel(canvas, y,
                (_selectedRow - _firstRow == y) ? Font.Default : Font.DefaultBold);
        }

        // Highlight the selection, if it is visible
        if ((_selectedCol - _firstColumn >= 0) && (_selectedRow - _firstRow >= 0))
        {
            canvas.DrawRectangle(
                LABEL_COL_WIDTH + (_selectedCol - _firstColumn) * DATA_COL_WIDTH + 1,
                              LABEL_ROW_HEIGHT + (_selectedRow - _firstRow) * DATA_ROW_HEIGHT + 1,
                              DATA_COL_WIDTH - 2,
                              DATA_ROW_HEIGHT - 2);
        }

        // Draw the text
        foreach (KeyValuePair<Address, String> address in _values)
        {
            String text = address.Value;
            int col = address.Key.Col - _firstColumn;
            int row = address.Key.Row - _firstRow;
            SizeF size = canvas.GetStringSize(text, Font.Default, FONT_SIZE + FONT_SIZE * 1.75f);
            canvas.Font = Font.Default;
            if (col >= 0 && row >= 0)
            {
                canvas.DrawString(text,
                    LABEL_COL_WIDTH + col * DATA_COL_WIDTH + PADDING,
                    LABEL_ROW_HEIGHT + row * DATA_ROW_HEIGHT + (DATA_ROW_HEIGHT - size.Height) / 2,
                    size.Width, size.Height, HorizontalAlignment.Left, VerticalAlignment.Center);
            }
        }
        canvas.RestoreState();
    }

    /// <summary>
    /// Draws a column label.  The columns are indexed beginning with zero.
    /// </summary>
    /// <param name="canvas"></param>
    /// <param name="x"></param>
    /// <param name="f"></param>
    private void DrawColumnLabel(ICanvas canvas, int x, Font f)
    {
        String label = ((char)('A' + x + _firstColumn)).ToString();
        SizeF size = canvas.GetStringSize(label, f, FONT_SIZE + FONT_SIZE * 1.75f);
        canvas.Font = f;
        canvas.FontSize = FONT_SIZE;
        canvas.DrawString(label,
              LABEL_COL_WIDTH + x * DATA_COL_WIDTH + (DATA_COL_WIDTH - size.Width) / 2,
              (LABEL_ROW_HEIGHT - size.Height) / 2, size.Width, size.Height,
              HorizontalAlignment.Center, VerticalAlignment.Center);
    }

    /// <summary>
    /// Draws a row label.  The rows are indexed beginning with zero.
    /// </summary>
    /// <param name="canvas">MAUI graphics canvas.</param>
    /// <param name="y"></param>
    /// <param name="f">Font</param>
    private void DrawRowLabel(ICanvas canvas, int y, Font f)
    {
        String label = (y + 1 + _firstRow).ToString();
        SizeF size = canvas.GetStringSize(label, f, FONT_SIZE + FONT_SIZE * 1.75f);
        canvas.Font = f;
        canvas.FontSize = FONT_SIZE;
        canvas.DrawString(label,
            LABEL_COL_WIDTH - size.Width - PADDING,
            LABEL_ROW_HEIGHT + y * DATA_ROW_HEIGHT + (DATA_ROW_HEIGHT - size.Height) / 2,
            size.Width, size.Height,
              HorizontalAlignment.Right, VerticalAlignment.Center);

    }

    /// <summary>
    /// Gets the backing spreadsheet.
    /// </summary>
    /// <returns>The backing spreadsheet.</returns>
    public Spreadsheet GetSheet()
    {
        return this._sheet;
    }

    /// <summary>
    /// Checks if the spreadsheet has changed after being saved or opened.
    /// </summary>
    /// <returns>True if changed, false if not.</returns>
    public bool IsChanged()
    {
        return _sheet.Changed;
    }

    /// <summary>
    /// Gets the dependent cells of the cell at the given address.
    /// </summary>
    /// <param name="col">Column</param>
    /// <param name="row">Row</param>
    /// <returns>A string of the dependent cells, contains itself.</returns>
    public string GetDependentCells(int col, int row)
    {
        string cellName = ((char)(65 + col)).ToString() + (row + 1).ToString();
        List<string> list = _sheet.GetAllDependents(cellName);
        int counter = 0;
        StringBuilder sb = new StringBuilder();

        foreach (string s in list)
        {
            if (counter == 30)
            {
                return sb.ToString();
            }
            sb.Append(s + " ");
            counter++;
        }

        return sb.ToString();
        }

    /// <summary>
    /// Replaces grid with a given backing spreadsheet. Used primarily when File->Open is clicked.
    /// </summary>
    /// <param name="sheet">The new backing spreadsheet.</param>
    public void ReplaceGrid(Spreadsheet sheet)
    {
        _sheet = sheet;

        foreach (string name in sheet.GetNamesOfAllNonemptyCells())
        {
            if (_sheet.GetCellValue(name) is FormulaError)
            {
                _values[new Address(((int)(name[0]) - 65), (int.Parse(name.Substring(1))) - 1)] = _sheet.GetCellContents(name).ToString();
            }
            else
            {
                _values[new Address(((int)(name[0]) - 65), (int.Parse(name.Substring(1))) - 1)] = _sheet.GetCellValue(name).ToString();
            }
        }
        Invalidate();
    }


}





