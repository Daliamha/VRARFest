using System;
using System.Threading.Tasks;
using UnityEngine;

namespace VRFest.Scripts.Game.Gameplay
{
    public class CarController : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidBody;
        public int speed;
        private StereoscopicVisionTestService _service;

        public void Init(StereoscopicVisionTestService service)
        {
            _service = service;
        }
        
        
        private async void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("DestroyCollider"))
            {
                _service.AddScores(10);
                //await Task.Delay(2000);
                Destroy(gameObject);
            }
        }

        private void FixedUpdate()
        {
            _rigidBody.AddForce(Vector3.forward * speed);
        }
    }
}