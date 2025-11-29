using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using R3;
using VRFest.Scripts.Game.States;
using VRFest.Scripts.Utils;

namespace VRFest.Scripts.Game.Gameplay
{
    public class StereoscopicVisionTestService
    {
        public ReadOnlyReactiveProperty<int> CurrentResult => _currentResult;
        private ReactiveProperty<int> _currentResult = new(0);
        private int _currentBestScore;
        
        private readonly GameplayEnterParams _gameplayEnterParams;
        private readonly Coroutines _coroutine;
        private bool _isNext;
        private StereoscopicVisionTestView _view { get; }
        private FlaskUploader _uploader;
        
        
        public StereoscopicVisionTestService(StereoscopicVisionTestView view, GameplayEnterParams gameplayEnterParams, 
            Coroutines coroutine, FlaskUploader uploader)
        {
            _uploader = uploader;
            _gameplayEnterParams = gameplayEnterParams;
            _coroutine = coroutine;
            _view = view;

            if (StateController.Load() == null)
            {
                PlayerPrefs.SetInt("LastDayPlayed1", 0);
                PlayerPrefs.SetInt("LastDayPlayed2", 0);
                PlayerPrefs.SetInt("LastDayPlayed3", 0);
                PlayerPrefs.SetInt("TodayBestResult1", 0);
                PlayerPrefs.SetInt("TodayBestResult2", 0);
                PlayerPrefs.SetInt("TodayBestResult3", 0);
            }
            else
            {
                var state = StateController.Load();
                if (state.LastDayPlayed != DateTime.Now.Day)
                {
                    PlayerPrefs.SetInt("TodayBestResult1", 0);
                    PlayerPrefs.SetInt("TodayBestResult2", 0);
                    PlayerPrefs.SetInt("TodayBestResult3", 0);
                }
            }
            
            coroutine.StartCoroutine(StartFirstAid());
        }

        private IEnumerator StartFirstAid()
        {
            var day = DateTime.Now.Day;
            _currentResult.Subscribe(x =>
            {
                _view.DisplayScores(x);
            });
            
            if (_gameplayEnterParams.nameOfBad.Contains("Burn"))
            {
                _view.DisplayLocation(0);
                
                
                _view.StartTime();
                yield return new WaitForSeconds(2f);
                while (!_isNext)
                {
                    _view.SpawnRandomCar(5, this);
                    yield return new WaitForSeconds(4);
                }
                _view.StopTimer();

                _currentBestScore = PlayerPrefs.GetInt("TodayBestResult1");
                if (_currentResult.Value > _currentBestScore)
                {
                    PlayerPrefs.SetInt("TodayBestResult1", _currentResult.Value);
                }
                PlayerPrefs.SetInt("LastDayPlayed1", DateTime.Now.Day);
                _view.EnableExits();
            }
            else if (_gameplayEnterParams.nameOfBad.Contains("Hypothermia"))
            {
                _view.DisplayLocation(1);
                
                yield return _view.StartTimer(60);

                _currentBestScore = PlayerPrefs.GetInt("TodayBestResult2");
                if (_currentResult.Value > _currentBestScore)
                {
                    PlayerPrefs.SetInt("TodayBestResult2", _currentResult.Value);
                }
                PlayerPrefs.SetInt("LastDayPlayed2", DateTime.Now.Day);
                _view.EnableExits();
            }
            else
            {
                
            }

            var json = StateController.Load();
            if (json != null)
            {
                var dict = json.Scores;
                if (json.Scores.ContainsKey(DateTime.Now))
                {
                    dict[DateTime.Now] = PlayerPrefs.GetInt("TodayBestResult1") +
                                         PlayerPrefs.GetInt("TodayBestResult2") +
                                         PlayerPrefs.GetInt("TodayBestResult3");
                }
                else
                {
                    dict.Add(DateTime.Now, _currentResult.Value);
                }

                int playToday = 0;
                if (PlayerPrefs.GetInt("TodayBestResult1") > 0) playToday++;
                if (PlayerPrefs.GetInt("TodayBestResult2") > 0) playToday++;
                if (PlayerPrefs.GetInt("TodayBestResult3") > 0) playToday++;
                
                StateController.Save(new FamilyLinkState
                {
                    LastDayPlayed = DateTime.Now.Day,
                    Scores = dict,
                    PlayToday = playToday,
                });
            }
            else
            {
                var dict = new Dictionary<DateTime, int>();
                dict.Add(DateTime.Now, _currentResult.Value);
                StateController.Save(new FamilyLinkState
                {
                    LastDayPlayed = DateTime.Now.Day,
                    Scores = dict,
                    PlayToday = 1,
                });
                Debug.Log(dict.Keys);
            }

            yield return new WaitForSeconds(2f);
            SaveOnServer();
        }

        public void FinishGame()
        {
            _isNext = true;
        }
        
        public void AddScores(int score)
        {
            _currentResult.Value += score;
        }

        private IEnumerator WaitUntilNextMove()
        {
            yield return new WaitUntil(() => _isNext);
            _isNext = false;
        }
        
        private void SaveOnServer()
        {
            var score = new List<ScoreEntry>();
            var state = StateController.Load();
            Debug.Log(state.Scores.Keys);
            Debug.Log(state.Scores.Keys.Count);
            foreach (var key in  state.Scores.Keys)
            {
                Debug.Log(key);
                score.Add(new ScoreEntry(key.Date.ToString("yyyy-MM-dd"), state.Scores[key]));
            }
            Debug.Log(score[0].date);
            Debug.Log(score[0].score);
            var players = new List<Player>
            {
                new Player(
                    name: PlayerPrefs.GetString("Name"),
                    gender: PlayerPrefs.GetInt("Gender") != 0 ? "Женский" : "Мужской",
                    age: int.Parse(PlayerPrefs.GetString("Age")),
                    scores: score
                ),
            };

            _uploader.SendPlayersToServer(players);
        }
    }
}