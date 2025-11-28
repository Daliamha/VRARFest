using System;
using UnityEngine;
using System.Collections;
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
            }
            else if (_gameplayEnterParams.nameOfBad.Contains("Hypothermia"))
            {
                _view.DisplayLocation(1);
                
                if (!PlayerPrefs.HasKey("LastDayPlayed2"))
                {
                    PlayerPrefs.SetInt("LastDayPlayed2", DateTime.Now.Day);
                }

                if (PlayerPrefs.GetInt("LastDayPlayed2") == DateTime.Now.Day)
                {
                    _currentResult.Value = PlayerPrefs.GetInt("LastDayPlayed2");
                }
                else
                {
                    _currentResult.Value = 0;
                }
                yield return WaitTimer();

                if (_currentResult.Value > _currentBestScore)
                {
                    PlayerPrefs.SetInt(PlayerPrefs.GetInt("LastDayPlayed2").ToString(), _currentBestScore);
                }
            }
            else
            {
                
            }
            
            
            var best2 = PlayerPrefs.GetInt(PlayerPrefs.GetInt("LastDayPlayed2").ToString());
            StateController.Save(new FamilyLinkState
            {
                LastDayPlayed = DateTime.Now.Day,
                BestRecord1 = 0,
                BestRecord2 = best2,
                BestRecord3 = 0,
            });
        }

        public void AddScores(int score)
        {
            _currentResult.Value += score;
        }

        public void OnCollisionWithPlayer()
        {
            
        }

        public IEnumerator WaitTimer()
        {
            yield return _view.StartTimer(60);
        }

        private IEnumerator WaitUntilNextMove()
        {
            yield return new WaitUntil(() => _isNext);
            _isNext = false;
        }
    }
}