using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;


namespace ChessBrowser
{
    /*
    * Author: Yohan Kwak, u0992389 for CS 5530
    * 3/17/2023
    * provides method to return (for example) a List<ChessGame> given a path to a PGN file.
    */
    internal class PgnReader
    {

        /**
         * method to return (for example) a List<ChessGame> given a path to a PGN file.
         */
        public List<ChessGame> readPGN(string filePath)
        {
            List<ChessGame> games = new List<ChessGame>();

            string[] lines = System.IO.File.ReadAllLines(filePath);

            string _eventName = null;
            string _site = null;
            string _round = null;
            string _whiteName = null;
            string _blackName = null;
            int _whiteElo = 0;
            int _blackElo = 0;
            char _result = ' ';
            DateTime _eventDate = default(DateTime);
            string _moves = "";

            bool isMoves = false;

            foreach (string line in lines)
            {
                if (isMoves)
                {
                    if (line.Length == 0)
                    {
                        games.Add(new ChessGame(_eventName, _site, _round, _whiteName, _blackName, _whiteElo, _blackElo, _result, _eventDate, _moves));
                        isMoves = false;

                        //reset the fields
                        _eventName = null;
                        _site = null;
                        _round = null;
                        _whiteName = null;
                        _blackName = null;
                        _whiteElo = 0;
                        _blackElo = 0;
                        _result = ' ';
                        _eventDate = _eventDate = new DateTime(0000, 00, 00);
                        _moves = "";
                    }
                    else
                    {
                        _moves += line;
                    }
                }
                else
                {
                    if (line.Length == 0)
                    {
                        isMoves = true;
                    }
                    else
                    {
                        if (line.StartsWith("[Event") && !line.StartsWith("[EventDate"))
                        {
                            _eventName = line.Remove(line.Length - 2).Substring(8);
                        }
                        else if (line.StartsWith("[Site"))
                        {
                            _site = line.Remove(line.Length - 2).Substring(7);
                        }
                        else if (line.StartsWith("[Round"))
                        {
                            _round = line.Remove(line.Length - 2).Substring(8);
                        }
                        else if (line.StartsWith("[White") && !line.StartsWith("[WhiteElo"))
                        {
                            _whiteName = line.Remove(line.Length - 2).Substring(8);
                        }
                        else if (line.StartsWith("[Black") && !line.StartsWith("[BlackElo"))
                        {
                            _blackName = line.Remove(line.Length - 2).Substring(8);
                        }
                        else if (line.StartsWith("[WhiteElo"))
                        {
                            _whiteElo = int.Parse(line.Remove(line.Length - 2).Substring(11));
                        }
                        else if (line.StartsWith("[BlackElo"))
                        {
                            _blackElo = int.Parse(line.Remove(line.Length - 2).Substring(11));
                        }
                        else if (line.StartsWith("[Result"))
                        {
                            if (line.Substring(9, 2).Equals("0-"))
                            {
                                _result = 'B';
                            }
                            else if (line.Substring(9, 2).Equals("1-"))
                            {
                                _result = 'W';
                            }
                            else
                            {
                                _result = 'D';
                            }
                        }
                        else if (line.StartsWith("[EventDate"))
                        {
                            try
                            {
                                string date = line.Remove(line.Length - 2).Substring(12);
                                int year = int.Parse(date.Substring(0, 4));
                                int month = int.Parse(date.Substring(5, 2));
                                int day = int.Parse(date.Substring(8, 2));

                                if (year != 0 && month != 0 && day != 0)
                                {
                                    _eventDate = new DateTime(0000, 00, 00);
                                }
                            }
                            catch (ArgumentOutOfRangeException)
                            {
                                //nothing, alreadt set to default
                            }
                        }
                    }
                }
                Console.WriteLine("\t" + line);
            }
            return games;
        }
    }
}
