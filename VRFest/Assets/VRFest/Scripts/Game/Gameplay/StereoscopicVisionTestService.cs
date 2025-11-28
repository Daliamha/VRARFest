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

        public StereoscopicVisionTestService(StereoscopicVisionTestView view, GameplayEnterParams gameplayEnterParams, Coroutines coroutine)
        {
            _gameplayEnterParams = gameplayEnterParams;
            _coroutine = coroutine;
            _view = view;

            if (!PlayerPrefs.HasKey(PlayerPrefs.GetInt("LastDayPlayed2").ToString()))
            {
                PlayerPrefs.SetInt(PlayerPrefs.GetInt("LastDayPlayed2").ToString(), 0);
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
                
                if (!PlayerPrefs.HasKey("LastDayPlayed1"))
                {
                    PlayerPrefs.SetInt("LastDayPlayed1", DateTime.Now.Day);
                    PlayerPrefs.SetInt(PlayerPrefs.GetInt("LastDayPlayed1").ToString(), 0);
                    _currentBestScore = 0;
                }
                else if (!PlayerPrefs.HasKey("Record1"))
                {
                    _currentBestScore = PlayerPrefs.GetInt("Record1");
                }
                else
                {
                    PlayerPrefs.SetInt("Record1", 0);
                    _currentBestScore = 0;
                }

                _view.StartTime();
                yield return new WaitWhile(() => !_isNext);
                _view.StopTimer();

                if (_currentResult.Value > _currentBestScore)
                {
                    PlayerPrefs.SetInt("Record1", _currentResult.Value);
                }
                PlayerPrefs.SetInt(PlayerPrefs.GetInt("LastDayPlayed1").ToString(), _currentResult.Value);
                _view.EnableExits();
            }
            else if (_gameplayEnterParams.nameOfBad.Contains("Hypothermia"))
            {
                _view.DisplayLocation(1);
                
                if (!PlayerPrefs.HasKey("LastDayPlayed2"))
                {
                    PlayerPrefs.SetInt("LastDayPlayed2", DateTime.Now.Day);
                    PlayerPrefs.SetInt(PlayerPrefs.GetInt("LastDayPlayed2").ToString(), 0);
                    _currentBestScore = 0;
                }
                else if (!PlayerPrefs.HasKey("Record2"))
                {
                    _currentBestScore = PlayerPrefs.GetInt("Record2");
                }
                else
                {
                    PlayerPrefs.SetInt("Record2", 0);
                    _currentBestScore = 0;
                }
                
                yield return _view.StartTimer(60);

                if (_currentResult.Value > _currentBestScore)
                {
                    PlayerPrefs.SetInt("Record2", _currentResult.Value);
                }
                PlayerPrefs.SetInt(PlayerPrefs.GetInt("LastDayPlayed2").ToString(), _currentResult.Value);
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
                    dict[DateTime.Now] += _currentResult.Value;
                }
                else
                {
                    dict.Add(DateTime.Now, _currentResult.Value);
                }
                
                StateController.Save(new FamilyLinkState
                {
                    LastDayPlayed = DateTime.Now.Day,
                    Scores = dict,
                    PlayToday = json.PlayToday + 1,
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
            }
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
    }
}