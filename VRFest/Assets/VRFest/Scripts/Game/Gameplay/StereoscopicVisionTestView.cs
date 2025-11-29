using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using VRFest.Scripts.Game.Gameplay.Contacts;
using Random = UnityEngine.Random;

namespace VRFest.Scripts.Game.Gameplay
{
    public class StereoscopicVisionTestView : MonoBehaviour
    {
        [Header("General")]
        [SerializeField] private TextMeshProUGUI _time;
        [SerializeField] private TextMeshProUGUI _scores;
        [SerializeField] private TextMeshProUGUI _afterTime;
        [SerializeField] private List<GameObject> _exits = new();
        [SerializeField] private List<GameObject> _locations = new();
        [Space] [Header("Mini-Games")]
        [SerializeField] private List<GameObject> _carsPrefabs = new();
        [SerializeField] private List<Transform> _carsSpawnPosition = new();
      

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

        public void SpawnRandomCar(int speed, StereoscopicVisionTestService service)
        {
            var transform = _carsSpawnPosition[Random.Range(0, _carsSpawnPosition.Count)];
            var controller = Instantiate(_carsPrefabs[Random.Range(0, _carsPrefabs.Count)], 
                transform.position, transform.rotation).GetComponent<CarController>();
            controller.Init(service);
            controller.speed += speed;
            var cont = controller.gameObject.GetComponent<ContactWithPlayer>();
            cont._mainCollider = Camera.main.gameObject.GetComponent<SphereCollider>();
            cont.Init(service);
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