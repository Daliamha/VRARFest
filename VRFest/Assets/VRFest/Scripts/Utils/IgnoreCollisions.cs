using System.Collections.Generic;
using UnityEngine;

namespace VRFest.Scripts.Utils
{
    public class IgnoreCollisions : MonoBehaviour
    {
        [SerializeField] private Collider _currentCollider;
        [Space] [SerializeField] private List<Collider> _objectsToIgnore;

        private void Start()
        {
            foreach (var collider in _objectsToIgnore)
            {
                Physics.IgnoreCollision(_currentCollider, collider);
            }
        }
    }
}
