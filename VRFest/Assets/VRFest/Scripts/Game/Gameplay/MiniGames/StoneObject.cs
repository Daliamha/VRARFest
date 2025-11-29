using System;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace VRFest.Scripts.Game.Gameplay.MiniGames
{
    public class StoneObject : MonoBehaviour
    {

        public Collider collider;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.TryGetComponent(out DestructibleTarget target))
            {
                target.Destroy();
                Destroy(gameObject);
            }
        }

        private async void OnEnable()
        {
            await Task.Delay(2000);
            collider.isTrigger = false;
        }
    }
}