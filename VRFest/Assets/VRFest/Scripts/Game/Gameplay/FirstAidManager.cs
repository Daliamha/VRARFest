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
        [SerializeField] private Animator _humanAnimator;
        [SerializeField] private GameObject _burnGameObject;

        private void Start()
        {
            _time.gameObject.SetActive(false);
            foreach (var outline in _hintsObjects)
            {
                outline.enabled = false;
            }
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
        
        public void PlayHintSound(int index)
        {
            _audioSource.PlayOneShot(_hintsSounds[index], 1);
            Debug.Log("Worked sound");
        }

        public void EnableManager(int index)
        {
            Debug.Log(index);
            _managers[index].enabled = true;
        }
        
        public void EnableOutlineHint(int index)
        {
            _hintsObjects[index].enabled = true;
            Debug.Log("Worked hint");
        }

        public IEnumerator StartTime()
        {
            var time = 0f;
            _time.gameObject.SetActive(true);
            while (true)
            {
                _time.text = TimeSpan.FromSeconds(time++).ToString(@"hh\:mm\:ss");
                yield return new WaitForSeconds(1f);
            }
        }
    }
}