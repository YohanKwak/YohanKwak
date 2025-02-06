using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessBrowser
{
    /*
    * Author: Yohan Kwak, u0992389 for CS 5530
    * 3/17/2023
    * Represents one instance of a Chess games including relevant Player and EventInfo, such as the players, round, result, moves, event name, date, event date, event site, and player Elo.
    */
    internal class ChessGame
    {
        public string eventName;
        public string site;
        public string round;
        public string whiteName;
        public string blackName;
        public int whiteElo;
        public int blackElo;
        public char result;
        public DateTime eventDate;
        public string moves;

        /**
         * A constructor of ChessGame instance with required field.
         */
        public ChessGame(string _eventName, string _site, string _round, string _whiteName, string _blackName, int _whiteElo, int _blackElo, char _result, DateTime _eventDate, string _moves)
        {
            eventName = _eventName;
            site = _site;
            round = _round;
            whiteName = _whiteName;
            blackName = _blackName;
            whiteElo = _whiteElo;
            blackElo = _blackElo;
            result = _result;
            eventDate = _eventDate;
            moves = _moves;
        }
    }
}
