using System;
using System.Threading.Tasks;
using UnityEngine;

namespace VRFest.Scripts.Game.Gameplay.MiniGames
{
    public class FireEnemyController : MonoBehaviour
    {
        public int speed;
        public Rigidbody rigibody;
        public ParticleSystem particles;
        public SkinnedMeshRenderer visual;
        public Animator animator;
        private StereoscopicVisionTestService _service;
        
        public void Init(StereoscopicVisionTestService service)
        {
            _service = service;
        }

        public async void Die()
        {
            particles.Play();
            await Task.Delay(2000);
            visual.enabled = false;
            animator.enabled = false;
            _service.AddScores(10);
            await Task.Delay(5000);
            Destroy(gameObject);
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