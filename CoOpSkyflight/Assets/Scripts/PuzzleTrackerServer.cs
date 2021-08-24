using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleTrackerServer : MonoBehaviour
{
    //General
    private bool puzzleChanged;

    //Puzzles
    //P0 = iniial power switch in machine room
    private bool p0_finished = false;
    private bool p0_activated = false;

    //P1 = glow button console at front of bridge
    private bool p1_finished = false;
    private bool[] p1_Solution = new bool[20] 
    {false, false, false, true, true,
     true, false, true, true, false,
     false, false, true, true, false,
     true, true, false, false, true};

    private bool[] p1_Current = new bool[20]
    {false, false, false, false, false,
     false, false, false, false, false,
     false, false, false, false, false,
     false, false, false, false, false};


    private void Update()
    {
        if (puzzleChanged)
            CheckPuzzles();
    }

    private void CheckPuzzles()
    {
        //P0
        if (p0_finished != p0_activated)
        {
            p0_finished = p0_activated;
            //Send update to all clients
        }

        //P1
        if (!p1_finished && p1_Current == p1_Solution)
        {
            p1_finished = true;
            //Send update to all clients
        }

        puzzleChanged = false;
    }

    public void SetP0Activated(bool state)
    {
        p0_activated = state;
        puzzleChanged = true;
    }

    public void SetP1State(bool[] states)
    {
        if (states.Length != 20)
        {
            Debug.LogError("[PuzzleTrackerServer] The given length of P1 button states was not 20");
            return;
        }
        p1_Current = states;
        puzzleChanged = true;
    }
}
