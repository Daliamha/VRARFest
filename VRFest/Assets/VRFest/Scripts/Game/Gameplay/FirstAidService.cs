using UnityEngine;
using System.Collections;
using VRFest.Scripts.Utils;

namespace VRFest.Scripts.Game.Gameplay
{
    public class FirstAidService
    {
        private readonly GameplayEnterParams _gameplayEnterParams;
        private readonly Coroutines _coroutines;
        private FirstAidView _firstAidView { get; }
        private int index = 0;
        private bool _isNext = false;

        public FirstAidService(FirstAidView firstAidView, GameplayEnterParams gameplayEnterParams, Coroutines coroutines)
        {
            _gameplayEnterParams = gameplayEnterParams;
            _coroutines = coroutines;
            _firstAidView = firstAidView;
            coroutines.StartCoroutine(StartFirstAid());
        }

        public void NextMoveFirstAid()
        {
            _isNext = true;
        }

        private IEnumerator StartFirstAid()
        {
            /*while (true)
            {*/
                if (!_gameplayEnterParams.isEducation)
                {
                    _coroutines.StartCoroutine(_firstAidView.StartTime());
                }
                else
                {
                    _firstAidView.PlayHintSound(index);
                    _firstAidView.EnableOutlineHint(index);
                }
                
                _firstAidView.EnableManager(index);
                yield return WaitUntilNextMove();
                _firstAidView.DisableAllHints();

                if (_gameplayEnterParams.nameOfBad.Contains("Burn"))
                {
                    index = 1;
                    _firstAidView.StartBurn();
                    for (int i = 0; i < 2; i++)
                    {
                        Debug.Log("index " + index);
                        _firstAidView.EnableManager(index);
                        if (_gameplayEnterParams.isEducation)
                        {
                            _firstAidView.EnableOutlineHint(index);
                            _firstAidView.PlayHintSound(index);
                        }

                        index += 1;
                        yield return WaitUntilNextMove();
                        _firstAidView.DisableAllHints();
                    }

                }
                else if (_gameplayEnterParams.nameOfBad.Contains("Hypothermia"))
                {
                    index = 3;
                    for (int i = 0; i < 2; i++)
                    {
                        Debug.Log("index " + index);
                        if (i == 1) { _firstAidView.ShowTryStandUpAnimation(); }
                        _firstAidView.EnableManager(index);
                        
                        if (_gameplayEnterParams.isEducation)
                        {
                            _firstAidView.EnableOutlineHint(index);
                            _firstAidView.PlayHintSound(index);
                        }

                        index += 1;
                        yield return WaitUntilNextMove();
                        _firstAidView.DisableAllHints();
                    }
                }
                else
                {
                    index = 5;
                    for (int i = 0; i < 2; i++)
                    {
                        Debug.Log("index " + index);
                        if (i == 0 && !_gameplayEnterParams.isEducation) { _firstAidView.EnableOutlineHint(index); _firstAidView.EnableOutlinee(index); }
                        _firstAidView.EnableManager(index);
                        if (_gameplayEnterParams.isEducation)
                        {
                            _firstAidView.EnableOutlineHint(index);
                            _firstAidView.PlayHintSound(index);
                        }
                        index += 1;
                        yield return WaitUntilNextMove();
                        if (i == 1) { _firstAidView.ShowTryStandUpAnimation(); }
                        _firstAidView.DisableAllHints();
                    }
                }
                
                if (index == 5)
                {
                    _firstAidView.StartVBurn();
                }
                
                if (_gameplayEnterParams.isEducation)
                {
                    _firstAidView.PlayHintSound(7);
                }
                else
                {
                    _firstAidView.StopTimer();
                }
            //}
        }

        private IEnumerator WaitUntilNextMove()
        {
            yield return new WaitUntil(() => _isNext);
            _isNext = false;
        }
    }
}