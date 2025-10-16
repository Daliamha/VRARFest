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
        [SerializeField] private TextMeshProUGUI _time;

        private void Start()
        {
            foreach (var outline in _hintsObjects)
            {
                outline.enabled = false;
            }
        }

        public void PlayHintSound(int index)
        {
            _audioSource.PlayOneShot(_hintsSounds[index], 1);
        }

        public void EnableOutlineHint(int index)
        {
            _hintsObjects[index].enabled = true;
        }

        public IEnumerator StartTime()
        {
            var time = 0f;
            while (true)
            {
                _time.text = TimeSpan.FromDays(time++).ToString(@"hh\:mm\:ss");
                yield return new WaitForSeconds(1f);
            }
        }
    }
}