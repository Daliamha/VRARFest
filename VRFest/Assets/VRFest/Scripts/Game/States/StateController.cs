using Newtonsoft.Json;
using UnityEngine;

namespace VRFest.Scripts.Game.States
{
    public static class StateController
    {
        public static void Save(FamilyLinkState state)
        {
            PlayerPrefs.SetString("State", JsonConvert.SerializeObject(state));
        }

        public static FamilyLinkState Load()
        {
            var json = PlayerPrefs.GetString("State");
            if (json == null)
            {
                return null;
            }
            else
            {
                return JsonConvert.DeserializeObject<FamilyLinkState>(json);
            }
        }
    }
}