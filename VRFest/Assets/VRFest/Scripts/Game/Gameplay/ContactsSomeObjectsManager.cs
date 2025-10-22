using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace VRFest.Scripts.Game.Gameplay
{
    public class ContactsSomeObjectsManager : Manager
    {
        public SphereCollider _mainCollider;
        public Collider _otherCollider;
        [SerializeField] private int _time;
        [Space] 
        [SerializeField] private bool _destroyObject;
        [SerializeField] private bool _destroyOtherObject;
        
        private Coroutine _coroutine;
        private FirstAidService _service;

        public override void Init(FirstAidService service)
        {
            _service = service;
        }
        
        public void Update()
        {
            if (_mainCollider != null && _otherCollider != null)
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
                _service.NextMoveFirstAid();
            }
            

            if (_destroyOtherObject)
            {
                Destroy(_otherCollider.gameObject);
            }
            if (_destroyObject)
            {
                Destroy(_mainCollider.gameObject);
            }
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