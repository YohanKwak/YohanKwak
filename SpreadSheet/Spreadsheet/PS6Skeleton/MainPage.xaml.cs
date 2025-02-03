using Microsoft.UI.Xaml.Controls.Primitives;
using Newtonsoft.Json.Linq;
using SpreadsheetUtilities;
using SS;
using Windows.ApplicationModel.Calls;
using Windows.Storage.Pickers;

namespace SpreadsheetGUI;

/// <summary>
/// Written by: Yohan Kwak and Simon Whidden
/// Date: 10/21/2022
/// The SpreadsheetGUI object.
/// </summary>
public partial class MainPage : ContentPage
{

    FileResult currentFile = null;
    String savedContent = "";

    /// <summary>
    /// Constructor for the MainPage.
    /// </summary>
	public MainPage()
    {
        InitializeComponent();
        spreadsheetGrid.SelectionChanged += displaySelection;
        spreadsheetGrid.SetSelection(0, 0);
    }

    /// <summary>
    /// This method displays the information about the selected cell.
    /// </summary>
    /// <param name="grid">The grid of cells.</param>
    private void displaySelection(SpreadsheetGrid grid)
    {
        //Gets selection coordinates, contents, and values of cell that was clicked.
        spreadsheetGrid.GetSelection(out int col, out int row);
        spreadsheetGrid.GetValue(col, row, out string value);
        spreadsheetGrid.GetContent(col, row, out string content);

        //Display the values retrieved above.
        cellName.Text = ((char)(65 + col)).ToString() + (row + 1).ToString();
        cellValue.Text = value.ToString();
        cellContents.Text = content;
        DependentCells.Text = spreadsheetGrid.GetDependentCells(col, row);
    }

    /// <summary>
    /// This method handles the code for when the user clicks File->New.
    /// </summary>
    /// <param name="sender">Event sender</param>
    /// <param name="e">EventArgument</param>
    private async void NewClicked(Object sender, EventArgs e)
    {
        bool answer = true;

        //If the spreadsheet has changed, ask the user if they want to save. If yes, save.
        if (spreadsheetGrid.IsChanged())
        {
            answer = await DisplayAlert("Unsaved Changes", "Would you like to save current spreadsheet", "Yes", "No");

            if (answer)
            {
                Save();
            }
        }

        //If the user decided not to save, resets the spreadsheet.
        if (!spreadsheetGrid.IsChanged() || !answer)
        {
            spreadsheetGrid.Clear();
            spreadsheetGrid.SetSelection(0, 0);
            cellName.Text = ((char)(65 + 0)).ToString() + (0 + 1).ToString();
            cellValue.Text = "";
            cellContents.Text = "";
            currentFile = null;
            DependentCells.Text = "";
        }
    }

    /// <summary>
    /// This method handles the code for when the user clicks File->Save
    /// </summary>
    /// <param name="sender">Event sender</param>
    /// <param name="e">EventArgument</param>
    private async void SaveClicked(Object sender, EventArgs e)
    {
        Save();
    }

    /// <summary>
    /// Handles saving the spreadsheet at a point chosen by the user. If the file already exists, overwrites it. 
    /// If not, asks the user for a filepath, and saves it there.
    /// </summary>
    private async void Save()
    {
        try
        {
            if (spreadsheetGrid.IsChanged())
            {
                if (currentFile != null)
                {
                    spreadsheetGrid.GetSheet().Save(currentFile.FullPath);
                }
                else
                {
                    string result = await DisplayPromptAsync("Save As", "Please provide the full file path to save the spreadsheet");
                    if (result == null)
                    {
                    }
                    spreadsheetGrid.GetSheet().Save(result);
                }
            }
            else
            {
                await DisplayAlert("Save Failed", "No changes have been made to current file", "OK");
            }
        }
        catch
        {
            await DisplayAlert("Save Failed", "File save failed", "OK");
        }
    }

    /// <summary>
    /// Opens any file as text and prints its contents.
    /// </summary>
    /// <param name="sender">Event sender</param>
    /// <param name="e">EventArgument</param>
    private async void OpenClicked(Object sender, EventArgs e)
    {
        bool answer = true;
        if (spreadsheetGrid.IsChanged())
        {
            answer = await DisplayAlert("Unsaved Changes", "Would you like to save current spreadsheet", "Yes", "No");

            if (answer)
            {
                Save();
            }
        }
        if (!spreadsheetGrid.IsChanged() || !answer)
        {
            try
            {
                FileResult fileResult = await FilePicker.Default.PickAsync();


                if (fileResult != null)
                {
                    Spreadsheet sheet = new Spreadsheet(fileResult.FullPath, S => true, s => s.ToUpper(), "ps6");
                    spreadsheetGrid.Clear();
                    spreadsheetGrid.ReplaceGrid(sheet);
                    await DisplayAlert("Success", "Successfully loaded the file", "OK");
                    currentFile = fileResult;

                    spreadsheetGrid.SetSelection(0, 0);
                    cellName.Text = ((char)(65 + 0)).ToString() + (0 + 1).ToString();
                    spreadsheetGrid.GetValue(0, 0, out string value);
                    spreadsheetGrid.GetContent(0, 0, out string content);
                    cellValue.Text = value;
                    cellContents.Text = content;

                    DependentCells.Text = spreadsheetGrid.GetDependentCells(0, 0);
                }
                else
                {
                    await DisplayAlert("Fail", "Failed to load the file", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Error opening file" + ex, "OK");
            }
        }
    }

    /// <summary>
    /// Handles values entered into the content field within the spreadsheet.
    /// </summary>
    /// <param name="sender">Event sender</param>
    /// <param name="e">EventArgument</param>
    private void EntryCompleted(Object sender, EventArgs E)
    {
        string text = ((Entry)sender).Text;

        spreadsheetGrid.GetSelection(out int col, out int row);

        try
        {
            spreadsheetGrid.SetValue(col, row, text);

            spreadsheetGrid.GetValue(col, row, out string value);

            cellValue.Text = value;


        }
        catch (FormulaFormatException)
        {
            DisplayAlert("Invalid Formula Format", "The Formula you have typed is in invalid format!", "OK");
            cellValue.Text = "!!ERROR!!";
        }

    }

    /// <summary>
    /// Handles when the user clicks the Change Selection help menu option.
    /// </summary>
    /// <param name="sender">Event sender</param>
    /// <param name="e">EventArgument</param>
    private async void ChangeSelectionHelpClicked(Object sender, EventArgs e)
    {
        await DisplayAlert("Changing Selection", "To change your selection of current cell, click the desired cell to select.", "OK");
    }

    /// <summary>
    /// Handles when the user clicks the Edit Cell help menu option.
    /// </summary>
    /// <param name="sender">Event sender</param>
    /// <param name="e">EventArgument</param>
    private async void EditCellContentHelpClicked(Object sender, EventArgs e)
    {
        await DisplayAlert("Editing Cell Content", "To edit the content of cell, click on Contents field right above the Spreadsheet, then press enter.", "OK");
    }

    /// <summary>
    /// Handles when the user clicks the Equation Format help menu option.
    /// </summary>
    /// <param name="sender">Event sender</param>
    /// <param name="e">EventArgument</param>
    private async void EquationFormatHelpClicked(Object sender, EventArgs e)
    {
        await DisplayAlert("Required Equation Format", "All Equation must begin with an equal sign, followed by an equation of numbers and" +
            " cell names. If any of the given/used cell name does not contain a number or successfully evaluated equation, it will not evaluate the expression and save the input as it was typed. Admitted operators are : + , - , * , / (Brackets\"()\" are also allowed.) \n Examples : \n = 5 + 2 \n =A1/B3+5 ", "OK");
    }

    /// <summary>
    /// Handles when the user clicks the Save/Load help menu option.
    /// </summary>
    /// <param name="sender">Event sender</param>
    /// <param name="e">EventArgument</param>
    private async void SaveLoadHelpClicked(Object sender, EventArgs e)
    {
        await DisplayAlert("Saving and Loading", "To save/load press the File on the menubar, and press desired operation \nTo save, enter the full desired filepath, if the spreadsheet is not already assigned to one, Save as always asks for a filepath to save \nTo load the file, use the file directory to navigate to a file you want to open then press 'open'.", "OK");
    }

    /// <summary>
    /// Handles when the user clicks the Dependent Cell help menu option.
    /// </summary>
    /// <param name="sender">Event sender</param>
    /// <param name="e">EventArgument</param>
    private async void DependentCellHelpClicked(Object sender, EventArgs e)
    {
        await DisplayAlert("Dependent Cells", "Displays as many dependent cells as possible in one line.(Includes itself)", "OK");
    }

    /// <summary>
    /// Handles when the user clicks the Copy/Move/Paste help menu option.
    /// </summary>
    /// <param name="sender">Event sender</param>
    /// <param name="e">EventArgument</param>
    private async void CopyPasteMoveHelpClicked(Object sender, EventArgs e)
    {
        await DisplayAlert("Copy/Move/Paste", "Copy\nWhen clicked, current cell's content is copied.\nMove\nWhen clicked, current cell's content is stored and deleted from the cell.\nPaste\nWhen Clicked, pastes the most recently saved content onto current cell, if copy or move have not been clicked, will paste an empty string.", "OK");
    }

    /// <summary>
    /// Saves current cell's content when the copy button is clicked.
    /// </summary>
    /// <param name="sender">Event sender</param>
    /// <param name="e">EventArgument</param>
    private void CopyClicked(Object sender, EventArgs e)
    {
        spreadsheetGrid.GetSelection(out int col, out int row);
        spreadsheetGrid.GetContent(col, row, out string content);
        savedContent = content;
    }

    /// <summary>
    /// Pastes the saved value into the current cell.
    /// </summary>
    /// <param name="sender">Event sender</param>
    /// <param name="e">EventArgument</param>
    private void PasteClicked(Object sender, EventArgs e)
    {
        spreadsheetGrid.GetSelection(out int col, out int row);
        spreadsheetGrid.SetValue(col, row, savedContent);
        spreadsheetGrid.GetValue(col, row, out string value);
        spreadsheetGrid.GetContent(col, row, out string content);

        cellValue.Text = value;
        cellContents.Text = content;
    }

    /// <summary>
    /// Saves the current cell's content and removes it from the spreadsheet.
    /// </summary>
    /// <param name="sender">Event sender</param>
    /// <param name="e">EventArgument</param>
    private void MoveClicked(Object sender, EventArgs e)
    {
        spreadsheetGrid.GetSelection(out int col, out int row);
        spreadsheetGrid.GetContent(col, row, out string content);
        savedContent = content;
        spreadsheetGrid.SetValue(col, row, "");
    }



}
