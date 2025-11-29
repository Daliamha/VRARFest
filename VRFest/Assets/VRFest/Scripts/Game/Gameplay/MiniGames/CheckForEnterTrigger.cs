using System.Collections.Generic;
using UnityEngine;

namespace VRFest.Scripts.Game.Gameplay.MiniGames
{
    public class CheckForEnterTrigger : MonoBehaviour
    {
        private List<GameObject> _objectsInside = new List<GameObject>();

        private void OnTriggerEnter(Collider other)
        {
            _objectsInside.Add(other.gameObject);
        }

        private void OnTriggerExit(Collider other)
        {
            if (_objectsInside.Contains(other.gameObject))
            {
                _objectsInside.Remove(other.gameObject);
            }
        }

        public void CheckForEntries()
        {
            foreach (var enemy in _objectsInside)
            {
                if (enemy.TryGetComponent(out FireEnemyController controller))
                {
                    controller.Die(true);
                    _objectsInside.Remove(enemy.gameObject);
                }
            }
        }
    }
}