//Authors: Initial layout by Daniel Kopta and Travis Martin, full implementation by Yohan Kwak and Simon Whidden, 11/27/2022
//This method handles the view for the snake game client.
using Newtonsoft.Json;
using Windows.Gaming.Input;

namespace SnakeGame;

public partial class MainPage : ContentPage
{
    //Fields
    GameController gc;

    /// <summary>
    /// Creates the mainpage for the snake client.
    /// </summary>
    public MainPage()
    {
        InitializeComponent();
        gc = new GameController();
        worldPanel.SetWorldAndController(gc);

        gc.UpdateArrived += OnFrame;
        gc.Connected += DisableConnect;
        gc.ConnectBroke += NetworkErrorHandler;
    }

    /// <summary>
    /// If client connection is successful, disables the connect button.
    /// </summary>
    public void DisableConnect()
    {
        Dispatcher.Dispatch(() => connectButton.IsEnabled = false);
        gc.Connected -= DisableConnect;
    }

    /// <summary>
    /// Event handler for tap-gesture.
    /// When user keyboard input is made, adjusts focus to the "movement" field.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    public void OnTapped(object sender, EventArgs args)
    {
        keyboardHack.Focus();
    }

    /// <summary>
    /// Event handler for command entry "hack."
    /// Creates ControlCommands according to user inputs, JSON-Serializes them, and then sends them to the server.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    public void OnTextChanged(object sender, TextChangedEventArgs args)
    {
        Entry entry = (Entry)sender;
        String text = entry.Text.ToLower();
        
        ControlCommand movement = new ControlCommand(text);

        gc.SendMovement(JsonConvert.SerializeObject(movement));
        
        entry.Text = "";
    }

    /// <summary>
    /// Event handler for the connect button.
    /// When connect button is clicked, check that input is valid, then attempts to connect to the server.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void ConnectClick(object sender, EventArgs args)
    {

        gc.FailedToConnect += ConnectionFailed;

        if (serverText.Text == "")
        {
            DisplayAlert("Error", "Please enter a server address", "OK");
            return;
        }
        if (nameText.Text == "")
        {
            DisplayAlert("Error", "Please enter a name", "OK");
            return;
        }
        if (nameText.Text.Length > 16)
        {
            DisplayAlert("Error", "Name must be less than 16 characters", "OK");
            return;
        }

        gc.ConnectToServer(serverText.Text, nameText.Text);

        keyboardHack.Focus();

    }

    /// <summary>
    /// Event handler for the help button.
    /// When clicked, displays the movement controls for the game.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ControlsButton_Clicked(object sender, EventArgs e)
    {
        DisplayAlert("Controls",
                     "W:\t\t Move up\n" +
                     "A:\t\t Move left\n" +
                     "S:\t\t Move down\n" +
                     "D:\t\t Move right\n",
                     "OK");
    }

    /// <summary>
    /// Event handler for the about button.
    /// When clicked, displays the creators, artists, and game designers.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void AboutButton_Clicked(object sender, EventArgs e)
    {
        DisplayAlert("About",
      "SnakeGame solution\nArtwork by Jolie Uk and Alex Smith\nGame design by Daniel Kopta and Travis Martin\n" +
      "Implementation by Yohan Kwak and Simon Whidden.\n" +
        "CS 3500 Fall 2022, University of Utah", "OK");
    }

    /// <summary>
    /// When connection fails, display alert.
    /// </summary>
    private void ConnectionFailed()
    {
        Dispatcher.Dispatch(() => DisplayAlert("Connection Failed", "Connection couldn't be established, please try again.", "OK"));
        gc.FailedToConnect -= ConnectionFailed;
    }

    /// <summary>
    /// Tells the view to redraw the world every frame.
    /// </summary>
    private void OnFrame()
    {
        Dispatcher.Dispatch(() => graphicsView.Invalidate());
    }

    /// <summary>
    /// If server disconnects, inform the user.
    /// </summary>
    private void NetworkErrorHandler()
    {
        Dispatcher.Dispatch(() => DisplayAlert("Error", "Disconnected from server", "OK"));
        
    }
    /// <summary>
    /// Focuses the cursor on the movement "hack" entry.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ContentPage_Focused(object sender, FocusEventArgs e)
    {
        if (!connectButton.IsEnabled)
            keyboardHack.Focus();
    }
}