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

        private bool _isDead;
        public async void Die(bool nice)
        {
            particles.Play();
            _isDead = true;
            animator.enabled = false;
            await Task.Delay(2000);
            visual.enabled = false;
            if (nice) _service.AddScores(10);
            await Task.Delay(5000);
            Destroy(gameObject);
        }

        private void FixedUpdate()
        {
            if (!_isDead)
            {
                rigibody.AddForce(transform.right * speed);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("DestroyCollider"))
            {
                _service.FinishGame();
                var enemies = FindObjectsByType<FireEnemyController>(FindObjectsSortMode.None);
                foreach (var e in enemies)
                {
                    e.Die(false);
                }
                Die(false);
            }
        }
    }
}