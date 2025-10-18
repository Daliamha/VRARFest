using System.Collections;
using UnityEngine;

namespace VRFest.Scripts.Game.Gameplay
{
    public class ContactsSomeObjectsManager : Manager
    {
        [SerializeField] private SphereCollider _mainCollider;
        [SerializeField] private Collider _otherCollider;
        [SerializeField] private int _time;
        private Coroutine _coroutine;
        private FirstAidService _service;

        public override void Init(FirstAidService service)
        {
            _service = service;
        }
        
        public void Update()
        {
            if (_coroutine == null && OverlapSphere(_mainCollider, _otherCollider))
            {
                _coroutine = StartCoroutine(TryDoResuscitation());
            }
        }

        private IEnumerator TryDoResuscitation()
        {
            for (int i = 0; i < _time; i++)
            {
                yield return new WaitForSeconds(1f);

                
                if (!OverlapSphere(_mainCollider, _otherCollider))
                {
                    _coroutine = null;
                    StopAllCoroutines();
                }
            }
            _service.NextMoveFirstAid();
            Debug.Log("All Nice");
            Destroy(this);
        }
        
        private bool OverlapSphere(SphereCollider main, Collider other)
        {
            Collider[] colliders = Physics.OverlapSphere(main.transform.position, main.radius);
            foreach (var collider in colliders)
            {
                if (collider.name.Contains(other.name))
                {
                    Debug.Log(collider.name);
                    return true;
                }
            }

            return false;
        }
    }
}