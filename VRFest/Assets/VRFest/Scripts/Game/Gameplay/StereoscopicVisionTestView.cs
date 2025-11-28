using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace VRFest.Scripts.Game.Gameplay
{
    public class StereoscopicVisionTestView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _time;
        [SerializeField] private TextMeshProUGUI _scores;
        [SerializeField] private TextMeshProUGUI _afterTime;
        [SerializeField] private List<GameObject> _exits = new();
        [SerializeField] private List<GameObject> _locations = new();
      

        private void Start()
        {
            foreach (var item in _exits)
            {
                item.gameObject.SetActive(false);
            }

            foreach (var item in _locations)
            {
                item.gameObject.SetActive(false);
            }
        }

        public void EnableExits()
        {
            foreach (var item in _exits)
            {
                item.gameObject.SetActive(true);
            }
        }
        
        public void DisplayLocation(int index)
        {
            _locations[index].gameObject.SetActive(true);
        }

        public void DisplayScores(int scores)
        {
            _scores.text = scores.ToString() + " баллов";
        }
            
        private bool _isTime = false;

        
        public IEnumerator StartTimer(int time)
        {
            _time.gameObject.SetActive(true);
            _time.text = TimeSpan.FromSeconds(time).ToString(@"hh\:mm\:ss");
            print(time + "sss");
            while (_time.text != "00:00:00")
            {
                _time.text = TimeSpan.FromSeconds(time--).ToString(@"hh\:mm\:ss");
                print(time + "sss");
                yield return new WaitForSeconds(1f);
            }
            _afterTime.gameObject.SetActive(true);
        }

        public void StartTime()
        {
            StartCoroutine(StartWaitTime());
        }
        
        public IEnumerator StartWaitTime()
        {
            var time = 0f;
            _time.gameObject.SetActive(true);
            _isTime = true;
            while (_isTime)
            {
                _time.text = TimeSpan.FromSeconds(time++).ToString(@"hh\:mm\:ss");
                yield return new WaitForSeconds(1f);
            }
            _afterTime.gameObject.SetActive(true);
        }

        public void StopTimer()
        {
            _isTime = false;
        }
    }
}