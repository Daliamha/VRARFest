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
                if (_gameplayEnterParams.isEducation)
                {
                    _coroutines.StartCoroutine(_firstAidView.StartTime());
                }

                _firstAidView.PlayHintSound(index);
                _firstAidView.EnableOutlineHint(index);
                yield return WaitUntilNextMove();

                if (_gameplayEnterParams.nameOfBad == "Burn")
                {
                    index = 1;
                    yield return WaitUntilNextMove();
                }
                else if (_gameplayEnterParams.nameOfBad == "Hypothermia")
                {
                    index = 4;
                    yield return WaitUntilNextMove();
                }
                else
                {
                    index = 7;
                    yield return WaitUntilNextMove();
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