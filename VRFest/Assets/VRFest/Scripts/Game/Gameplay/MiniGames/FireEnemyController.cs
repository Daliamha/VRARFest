using System;
using UnityEngine;

namespace VRFest.Scripts.Game.Gameplay.MiniGames
{
    public class FireEnemyController : MonoBehaviour
    {
        public int speed;
        public Rigidbody rigibody;
        private StereoscopicVisionTestService _service;
        
        public void Init(StereoscopicVisionTestService service)
        {
            _service = service;
        }

        private void FixedUpdate()
        {
            rigibody.AddForce(transform.forward * speed);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("DestroyCollider"))
            {
                _service.FinishGame();
                Destroy(gameObject);
            }
        }
    }
}