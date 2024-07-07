using UnityEngine;
using System.Collections;

/*
 * Holder for event names
 * Created By: NeilDG
 */ 
public class EventNames {

    public class GAME_LOOP_EVENTS
    {

        public const string ON_PLAYER_SPAWN = "ON_PLAYER_SPAWNED"; //Player loads into scene, show the intro card
        public const string ON_GAME_STARTED = "ON_GAME_STARTED"; //When Intro card has been closed for the first time, start the game

        //Loop
        public const string ON_FURNITURE_INSPECTION = "ON_FURNITURE_INSPECTION"; 
        public const string ON_MUSIC_ROLL_FOUND = "ON_MUSIC_ROLL_FOUND";
        public const string ON_HOUR_PASSED = "ON_HOUR_PASSED";

        //Win Con
        public const string ON_ALL_ROLLS_FOUND = "ON_ALL_ROLLS_FOUND";

        //Lose Con
        public const string ON_TIMES_UP = "ON_TIMES_UP";
        public const string ON_PLAYER_CAPTURED = "ON_PLAYER_CAPTURED";

    }

    public class DIALOGUE_EVENTS
    {
        public const string ON_DIALOGUE_INSERTION = "ON_DIALOGUE_INSERTION";
        public const string ON_DIALOGUE_INVOCATION = "ON_DIALOGUE_INVOCATION";
        public const string ON_DIALOGUE_ENDED = "ON_DIALOGUE_ENDED";
    }

    public class PLAYER_ACTIONS
    {
        public const string ON_PLAYER_SPRINT_STARTED = "ON_PLAYER_SPRINT_STARTED";
        public const string ON_PLAYER_SPRINT = "ON_PLAYER_SPRINT";
        public const string ON_PLAYER_SPRINT_ENDED = "ON_PLAYER_SPRINT_ENDED";
    }
}







