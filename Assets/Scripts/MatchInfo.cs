using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchInfo : MonoBehaviour
{

    public List<GridItem> match = new List<GridItem>();
    public int matchStartingX;
    public int matchEndingX;
    public int matchStartingY;
    public int matchEndingY;

    public bool validMatch
    {
        get { return match != null; }
    }
}
