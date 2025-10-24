using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace VRFest.Scripts.Game.Gameplay
{
    public class FirstAidView : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private List<AudioClip> _hintsSounds;
        [SerializeField] private List<Outline> _hintsObjects;
        [SerializeField] private List<Manager> _managers;
        [SerializeField] private TextMeshProUGUI _time;
        [SerializeField] private TextMeshProUGUI _nice;
        [SerializeField] private Animator _humanAnimator;
        [SerializeField] private GameObject _burnGameObject;
        [SerializeField] private GameObject _bwurnGameObject;
        [SerializeField] private CapsuleCollider _humanCollider;

        private void Start()
        {
            _time.gameObject.SetActive(false);
            DisableAllHints();
            foreach (var manager in _managers)
            {
                manager.enabled = false;
            }
        }

        public void ShowTryStandUpAnimation()
        {
            _humanAnimator.SetTrigger("TryUp");
        }

        public void StartBurn()
        {
            _burnGameObject.SetActive(true);
        }

        public void EnableOutlinee(int index)
        {
            _hintsObjects[index].OutlineWidth = 0;
            Debug.Log("Worked hint");
        }

        public void StartVBurn()
        {
            _bwurnGameObject.SetActive(true);
        }
        
        public void PlayHintSound(int index)
        {
            _audioSource.Stop();
            _audioSource.PlayOneShot(_hintsSounds[index], 1);
            Debug.Log("Worked sound");
        }

        public void EnableManager(int index)
        {
            Debug.Log(index);
            _managers[index].enabled = true;
            if (_managers[index].name == "LiftManager")
            {
                _humanCollider.direction = 0;
            }
        }
        
        public void EnableOutlineHint(int index)
        {
            _hintsObjects[index].enabled = true;
            Debug.Log("Worked hint");
        }

        public void DisableAllHints()
        {
            foreach (var outline in _hintsObjects)
            {
                if (outline != null)
                {
                    outline.enabled = false;
                }
            }
        }

        private bool _isTime = false;
        public IEnumerator StartTime()
        {
            var time = 0f;
            _time.gameObject.SetActive(true);
            _isTime = true;
            while (_isTime)
            {
                _time.text = TimeSpan.FromSeconds(time++).ToString(@"hh\:mm\:ss");
                yield return new WaitForSeconds(1f);
            }
            _nice.gameObject.SetActive(true);
        }

        public void StopTimer()
        {
            _isTime = false;
        }
    }
}