using System;
using System.Collections.Generic;

namespace VRFest.Scripts.Game.States
{
    [System.Serializable]
    public class FamilyLinkState
    {
        public int LastDayPlayed;
        public int PlayToday;
        public Dictionary<string, int> Scores = new();
    }
}