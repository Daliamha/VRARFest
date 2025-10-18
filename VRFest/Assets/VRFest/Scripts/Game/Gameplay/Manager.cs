using UnityEngine;

namespace VRFest.Scripts.Game.Gameplay
{
    public abstract class Manager : MonoBehaviour
    {
        public abstract void Init(FirstAidService service);
    }
}