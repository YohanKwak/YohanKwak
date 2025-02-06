using Microsoft.Maui.Controls;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessBrowser
{
    internal class Queries
    {

        /// <summary>
        /// This function runs when the upload button is pressed.
        /// Given a filename, parses the PGN file, and uploads
        /// each chess game to the user's database.
        /// </summary>
        /// <param name="PGNfilename">The path to the PGN file</param>
        internal static async Task InsertGameData(string PGNfilename, MainPage mainPage)
        {
            // This will build a connection string to your user's database on atr,
            // assuimg you've typed a user and password in the GUI
            string connection = mainPage.GetConnectionString();

            PgnReader pngReader = new PgnReader();
            List<ChessGame> games = pngReader.readPGN(PGNfilename);

            mainPage.SetNumWorkItems(games.Count);

            using (MySqlConnection conn = new MySqlConnection(connection))
            {
                try
                {
                    // Open a connection
                    conn.Open();

                    foreach (ChessGame g in games)
                    {
                        MySqlCommand cmd = conn.CreateCommand();
                        //Insert WhitePlayer
                        cmd.CommandText = "Insert into Players (Name, Elo) values ( @WhitePlayerName , @WhiteElo ) on duplicate key update Elo = If(Elo > @WhiteElo , Elo, @WhiteElo );";
                        cmd.Parameters.AddWithValue("@WhitePlayerName", g.whiteName);
                        cmd.Parameters.AddWithValue("@WhiteElo", g.whiteElo);
                        cmd.ExecuteNonQuery();
                        //Insert BlackPlayer
                        cmd.CommandText = "Insert into Players (Name, Elo) values ( @BlackPlayerName , @BlackElo ) on duplicate key update Elo = If(Elo > @BlackElo , Elo, @BlackElo );";
                        cmd.Parameters.AddWithValue("@BlackPlayerName", g.blackName);
                        cmd.Parameters.AddWithValue("@BlackElo", g.blackElo);
                        cmd.ExecuteNonQuery();
                        //Insert current Event
                        cmd.CommandText = "Insert into Events (Name, Site, Date) values ( @Name , @Site, @Date ) on duplicate key update Name = Name;";
                        cmd.Parameters.AddWithValue("@Name", g.eventName);
                        cmd.Parameters.AddWithValue("@Site", g.site);
                        int year = g.eventDate.Year;
                        int month = g.eventDate.Month;
                        int day = g.eventDate.Day;
                        String eDate = year + "-" + month + "-" + day;
                        cmd.Parameters.AddWithValue("@Date", eDate);

                        cmd.ExecuteNonQuery();

                        //Get Event ID and Players' pIDs
                        cmd.CommandText = "select eID from Events where Name = @EName and Site = @ESite and Date = @EDate;";
                        cmd.Parameters.AddWithValue("@EName", g.eventName);
                        cmd.Parameters.AddWithValue("@ESite", g.site);
                        cmd.Parameters.AddWithValue("@EDate", eDate);

                        int eID = -1;

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                eID = int.Parse(reader["eID"].ToString());
                            }
                        }
                        int blackpID = -1;
                        int whitepID = -1;

                        cmd.CommandText = "select * from Players where Name = @BlackPName;";
                        cmd.Parameters.AddWithValue("@BlackPName", g.blackName);

                        System.Diagnostics.Debug.WriteLine(cmd.CommandText);


                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                blackpID = int.Parse(reader["pID"].ToString());
                            }
                        }

                        cmd.CommandText = "select * from Players where Name = @WhitePName;";
                        cmd.Parameters.AddWithValue("@WhitePName", g.whiteName);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                whitepID = int.Parse(reader["pID"].ToString());
                            }
                        }

                        // Insert Games
                        cmd.CommandText = "Insert into Games (Round, Result, Moves, BlackPlayer, WhitePlayer, eID) values (@GameRound, @GameResult, @GameMoves, @GameBlack , @GameWhite , @GameeID) on duplicate key update Result = Result;";

                        cmd.Parameters.AddWithValue("@GameRound", g.round);
                        cmd.Parameters.AddWithValue("@GameResult", g.result);
                        cmd.Parameters.AddWithValue("@GameMoves", g.moves);
                        cmd.Parameters.AddWithValue("@GameBlack", blackpID);
                        cmd.Parameters.AddWithValue("@GameWhite", whitepID);
                        cmd.Parameters.AddWithValue("@GameeID", eID);

                        cmd.ExecuteNonQuery();

                        await mainPage.NotifyWorkItemCompleted();
                    }
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }
            }
        }

        /// <summary>
        /// Queries the database for games that match all the given filters.
        /// The filters are taken from the various controls in the GUI.
        /// </summary>
        /// <param name="white">The white player, or null if none</param>
        /// <param name="black">The black player, or null if none</param>
        /// <param name="opening">The first move, e.g. "1.e4", or null if none</param>
        /// <param name="winner">The winner as "W", "B", "D", or null if none</param>
        /// <param name="useDate">True if the filter includes a date range, False otherwise</param>
        /// <param name="start">The start of the date range</param>
        /// <param name="end">The end of the date range</param>
        /// <param name="showMoves">True if the returned data should include the PGN moves</param>
        /// <returns>A string separated by newlines containing the filtered games</returns>
        internal static string PerformQuery(string white, string black, string opening,
          string winner, bool useDate, DateTime start, DateTime end, bool showMoves,
          MainPage mainPage)
        {
            // This will build a connection string to your user's database on atr,
            // assuimg you've typed a user and password in the GUI
            string connection = mainPage.GetConnectionString();

            // Build up this string containing the results from your query
            string parsedResult = "";

            // Use this to count the number of rows returned by your query
            // (see below return statement)
            int numRows = 0;

            using (MySqlConnection conn = new MySqlConnection(connection))
            {
                try
                {
                    // Open a connection
                    conn.Open();

                    MySqlCommand cmd = conn.CreateCommand();

                    int whitepID = -1;
                    int blackpID = -1;

                    //Get pID first
                    if (white is not null)
                    {
                        cmd.CommandText = "select pID from Players where Name = @WhitePlayerName;";
                        cmd.Parameters.AddWithValue("@WhitePlayerName", white);


                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                whitepID = int.Parse(reader["pID"].ToString());
                            }
                        }
                    }
                    if (black is not null)
                    {
                        cmd.CommandText = "select pID from Players where Name = @BlackPlayerName;";
                        cmd.Parameters.AddWithValue("@BlackPlayerName", black);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                blackpID = int.Parse(reader["pID"].ToString());
                            }
                        }
                    }
                    //Generate query accordingly
                    cmd.CommandText = "select Events.Name as Event_Name, Events.Site as Event_Site, Events.Date as Event_Date, WPlayer.Name as WName, BPlayer.Name as BName, Games.Result as GameResult, Games.Moves as GMoves, BPlayer.Elo as BElo, WPlayer.Elo as WElo from Games natural join Events join (select * from Players) as BPlayer join (select * from Players) as WPlayer where BPlayer.pID = Games.BlackPlayer and WPlayer.pID = Games.WhitePlayer";
                    if (whitepID != -1)
                    {
                        cmd.CommandText += " and WhitePlayer = " + whitepID;
                    }
                    if (blackpID != -1)
                    {
                        cmd.CommandText += " and BlackPlayer = " + blackpID;
                    }
                    if (opening is not null)
                    {
                        cmd.CommandText += " and Moves = \"^" + opening + "%\"";
                    }
                    if (winner is not null)
                    {
                        cmd.CommandText += " and Result = \"" + winner + "\"";
                    }
                    cmd.CommandText += ";";

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int year = int.Parse(reader["Event_Date"].ToString().Substring(6, 4));
                            int month = int.Parse(reader["Event_Date"].ToString().Substring(0, 2));
                            int day = int.Parse(reader["Event_Date"].ToString().Substring(3, 2));

                            DateTime GameDate = new DateTime(year, month, day);

                            if (useDate && (DateTime.Compare(GameDate, start) < 0 || DateTime.Compare(GameDate, end) > 0))
                            {
                            }
                            else
                            {
                                parsedResult += "Event: " + reader["Event_Name"] + "\n";
                                parsedResult += "Site: " + reader["Event_Site"] + "\n";
                                parsedResult += "Date: " + reader["Event_Date"] + "\n";
                                parsedResult += "White: " + reader["WName"] + " (" + reader["WElo"] + ")" + "\n";
                                parsedResult += "Black: " + reader["BName"] + " (" + reader["BElo"] + ")" + "\n";
                                parsedResult += "Result: " + reader["GameResult"] + "\n";

                                if (showMoves)
                                {
                                    parsedResult += reader["GMoves"] + "\n";
                                }
                                parsedResult += "\n";
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }
            }
            return numRows + " results\n" + parsedResult;
        }
    }
}
