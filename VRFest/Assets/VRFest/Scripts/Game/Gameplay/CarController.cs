using System;
using UnityEngine;

namespace VRFest.Scripts.Game.Gameplay
{
    public class CarController : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidBody;
        public int speed;
        
        private void FixedUpdate()
        {
            _rigidBody.AddForce(Vector3.forward * speed);
        }
    }
}