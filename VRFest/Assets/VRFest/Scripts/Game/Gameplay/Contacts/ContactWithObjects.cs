using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRFest.Scripts.Game.Gameplay.Contacts
{
    public class ContactWithObjects : Manager
    {
        public SphereCollider _mainCollider;
        public List<Collider> _otherCollider = new();
        [SerializeField] private int _time;
        [Space] 
        [SerializeField] private bool _destroyObject;
        [SerializeField] private bool _destroyOtherObject;
        
        private Coroutine _coroutine;
        protected StereoscopicVisionTestService _service;

        public override void Init(StereoscopicVisionTestService service)
        {
            _service = service;
        }
        
        public void Update()
        {
            if (_mainCollider != null && _otherCollider.Count != 0)
            {
                
                if (_coroutine == null && OverlapSphere(_mainCollider, _otherCollider))
                {
                    _coroutine = StartCoroutine(TryDoResuscitation());
                }
            }
            else
            {
                Debug.Log("Null Reference Exception");
            }
        }

        private IEnumerator TryDoResuscitation()
        {
            for (int i = 0; i < _time; i++)
            {
                yield return new WaitForSeconds(1f);

                Debug.Log(i + " seconds of " + _time);
                if (!OverlapSphere(_mainCollider, _otherCollider))
                {
                    _coroutine = null;
                    StopAllCoroutines();
                }
            }
            if (_service == null) { Debug.Log("Service is null"); }
            else
            {
                _service.AddScores(10);
            }
            

            if (_destroyOtherObject)
            {
                foreach (var col in _otherCollider)
                {
                    Destroy(col.gameObject);
                }
            }
            if (_destroyObject)
            {
                Destroy(_mainCollider.gameObject);
            }
        }
        
        private bool OverlapSphere(SphereCollider main, List<Collider> other)
        {
            var abc = 0;
            Collider[] colliders = Physics.OverlapSphere(main.transform.position, main.radius);
            print(colliders.Length);
            foreach (var currentCollider in colliders)
            {
                if (currentCollider.gameObject != main.gameObject)
                {
                    foreach (var col in other)
                    {
                        print(currentCollider.gameObject.name);
                        print(col.gameObject.name);
                        if (currentCollider.gameObject == col.gameObject)
                        {
                            abc++;
                        }
                    }
                }
            }

            print(abc);
            if (abc >= 3)
            {
                return true;
            }
            
            return false;
        }
    }
}