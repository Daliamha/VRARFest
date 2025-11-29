using UnityEngine;
using VRFest.Scripts.Game.Gameplay.Contacts;

namespace VRFest.Scripts.Game.Gameplay.MiniGames
{
    public class ButterfliesGroup : MonoBehaviour
    {
        
        private StereoscopicVisionTestService _service;
        
        public void Init(StereoscopicVisionTestService service, SphereCollider collider)
        {
            _service = service;
            gameObject.GetComponent<ContactWithSetka>()._mainCollider = collider;
            gameObject.GetComponent<ContactWithSetka>().Init(service);
        }
        
    }
}