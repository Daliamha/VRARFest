using System.Collections;
using UnityEngine;

namespace VRFest.Scripts.Game.Gameplay
{
    public class CardiopulmonaryResuscitationManager : Manager
    {
        [SerializeField] private SphereCollider _collider;
        private Coroutine _coroutine;
        private FirstAidService _service;

        public override void Init(FirstAidService service)
        {
            _service = service;
        }
        
        public void Update()
        {
            if (_coroutine == null)
            {
                Collider[] colliders = Physics.OverlapSphere(_collider.transform.position, _collider.radius);

                foreach (var collider in colliders)
                {
                    if (collider.name.Contains("Camera"))
                    {
                        Debug.Log(collider.name);
                        _coroutine = StartCoroutine(TryDoResuscitation());
                        break;
                    }
                }
            }
        }

        private IEnumerator TryDoResuscitation()
        {
            var allNice = false;
            for (int i = 0; i < 7; i++)
            {
                yield return new WaitForSeconds(1f);
                Collider[] colliders = Physics.OverlapSphere(_collider.transform.position, _collider.radius);
                foreach (var collider in colliders)
                {
                    allNice = false;
                    if (collider.name.Contains("Camera"))
                    {
                        Debug.Log(collider.name);
                        allNice = true;
                        break;
                    }
                }

                if (colliders.Length == 0)
                {
                    allNice = false;
                }

                if (allNice == false)
                {
                    _coroutine = null;
                    break;
                }
            }

            if (allNice)
            {
                _service.NextMoveFirstAid();
                Debug.Log("All Nice");
                Destroy(this);
            }
        }
    }
}
