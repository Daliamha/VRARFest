using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using VRFest.Scripts.Game.Gameplay.Contacts;
using VRFest.Scripts.Game.Gameplay.MiniGames;
using Random = UnityEngine.Random;

namespace VRFest.Scripts.Game.Gameplay
{
    public class StereoscopicVisionTestView : MonoBehaviour
    {
        [Header("General")]
        [SerializeField] private TextMeshProUGUI _time;
        [SerializeField] private TextMeshProUGUI _scores;
        [SerializeField] private TextMeshProUGUI _afterTime;
        [SerializeField] private TextMeshProUGUI _educationText;
        [SerializeField] private GameObject _educationPanel;
        [SerializeField] private List<GameObject> _exits = new();
        [SerializeField] private List<GameObject> _locations = new();
        [Space] [Header("Mini-Games")]
        [SerializeField] private List<GameObject> _carsPrefabs = new();
        [FormerlySerializedAs("_carsSpawnPosition")] 
        [SerializeField] private List<Transform> _carsSpawnPositions = new();
        [Space]
        [SerializeField] private GameObject _starTargetPrefab;
        [SerializeField] private List<Transform> _starsSpawnPositions = new();
        [Space]
        [SerializeField] private List<GameObject> _enemiesPrefabs = new();
        [FormerlySerializedAs("_spawnPositions")] [SerializeField] 
        private List<Transform> _enemiesSpawnPositions = new();
        [Space]
        [SerializeField] private List<GameObject> _magicItems = new();
        [SerializeField] private List<Transform> _magicItemsSpawnPositions = new();
        [Space] 
        
        [Space] 
        [SerializeField] private ButterfliesGroup _butterfliesPrefab;
        [SerializeField] private SphereCollider _netCollider;
        [SerializeField] private List<Transform> _butterfliesSpawnPoints = new();
      

        private void Start()
        {
            _time.gameObject.SetActive(false);
            foreach (var item in _exits)
            {
                item.gameObject.SetActive(false);
            }

            foreach (var item in _locations)
            {
                item.gameObject.SetActive(false);
            }
            _educationText.gameObject.SetActive(false);
            _educationPanel.SetActive(false);
        }

        public async void SetEducationTextForSeconds(string educationText, int seconds)
        {
            _educationText.gameObject.SetActive(true);
            _educationPanel.SetActive(true);
            _educationText.text = educationText;
            await Task.Delay(seconds * 1000);
            CloseEducation();
        }

        public void CloseEducation()
        {
            _educationPanel.SetActive(false);
            _educationText.gameObject.SetActive(false);
        }
        
        public void SpawnFireEnemy(StereoscopicVisionTestService service, int speed)
        {
            var index = Random.Range(0, _enemiesSpawnPositions.Count);
            var cont = Instantiate(_enemiesPrefabs[Random.Range(0, _enemiesPrefabs.Count)], 
                _enemiesSpawnPositions[index].position, _enemiesSpawnPositions[index].transform.rotation)
                .GetComponent<FireEnemyController>();
            cont.Init(service, speed);
        }

        private List<GameObject> _spawnedButterflies = new();
        public void SpawnButterfliesGroup(StereoscopicVisionTestService service)
        {
            var index = Random.Range(0, _butterfliesSpawnPoints.Count);
            var group = Instantiate(_butterfliesPrefab, _butterfliesSpawnPoints[index].position, 
                _butterfliesSpawnPoints[index].rotation).GetComponent<ButterfliesGroup>();
            group.Init(service, _netCollider);
            _spawnedButterflies.Add(group.gameObject);
        }
        public void DestroyAllButterflies()
        {
            foreach (var item in _spawnedButterflies)
            {
                if (item != null)
                {
                    Destroy(item);
                }
            }
            _spawnedTargets.Clear();
        }
        
        
        private List<GameObject> _spawnedTargets = new();
        public void SpawnTargetsToFire(int amount, bool onOneDistance, StereoscopicVisionTestService service)
        {
            var index = 0;
            if (onOneDistance)
            {
                index =  Random.Range(0, 5);
            }
            else
            {
                index =  Random.Range(0, _starsSpawnPositions.Count);
            }

            for (int i = 0; i < amount; i++)
            {
                var dest = Instantiate(_starTargetPrefab, _starsSpawnPositions[index].position + Vector3.up,
                    _starTargetPrefab.transform.rotation).GetComponent<DestructibleTarget>();
                dest.Init(service);
                _spawnedTargets.Add(dest.gameObject);
            }
        }

        public void DestroyAllTargets()
        {
            foreach (var item in _spawnedTargets)
            {
                if (item != null)
                {
                    Destroy(item);
                }
            }
            _spawnedTargets.Clear();
        }
        
        public void SpawnRandomCar(int speed, StereoscopicVisionTestService service, int amount)
        {
            var indexes = new List<int>();
            for (int i = 0; i < amount; i++)
            {
                var index = Random.Range(0, _magicItemsSpawnPositions.Count);
                while (indexes.Contains(index))
                {
                    index = Random.Range(0, _magicItemsSpawnPositions.Count);
                }
                indexes.Add(index);
            }

            for (int i = 0; i < amount; i++)
            {
                var position = _carsSpawnPositions[Random.Range(0, _carsSpawnPositions.Count)];
                var controller = Instantiate(_carsPrefabs[Random.Range(0, _carsPrefabs.Count)],
                    position.position, position.rotation).GetComponent<CarController>();
                controller.Init(service);
                controller.speed += speed;
                var cont = controller.gameObject.GetComponent<ContactWithPlayer>();
                cont._mainCollider = Camera.main.gameObject.GetComponent<SphereCollider>();
                cont.Init(service);
            }
        }

        public void SpawnMagicItem(int speed, StereoscopicVisionTestService service, int amount)
        {
            var indexes = new List<int>();
            for (int i = 0; i < amount; i++)
            {
                var index = Random.Range(0, _magicItemsSpawnPositions.Count);
                while (indexes.Contains(index))
                {
                    index = Random.Range(0, _magicItemsSpawnPositions.Count);
                }
                indexes.Add(index);
            }
            
            for (int i = 0; i < amount; i++)
            {
                var index = indexes[i];
                var position = _magicItemsSpawnPositions[index];
                var controller = Instantiate(_magicItems[Random.Range(0, _magicItems.Count)],
                    position.position, position.rotation).GetComponent<CarController>();
                controller.Init(service);
                controller.speed += speed;
                var cont = controller.gameObject.GetComponent<ContactWithPlayer>();
                cont._mainCollider = Camera.main.gameObject.GetComponent<SphereCollider>();
                cont.Init(service);
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