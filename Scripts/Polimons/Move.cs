using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move 
{
    public MoveBase Base { get; set; }
    public int Uses { get; set; }
    public int hits { get; set; }

    public Move(MoveBase MoveBase) {
        Base = MoveBase;
        Uses = MoveBase.Uses;
    }
}
