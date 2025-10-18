using BaCon;
using HurricaneVR.TechDemo.Scripts;
using UnityEngine;
using R3;
using VRFest.Scripts.Game.MainMenu;
using VRFest.Scripts.Game.Root;
using VRFest.Scripts.Utils;
using UnityEngine.UI;

namespace VRFest.Scripts.Game.Gameplay
{

    public class GameplayEntryPoint : MonoBehaviour
    {
        
        private DIContainer _gameplayContainer;
        private Subject<Unit> _exitSceneEvent;
        [SerializeField] private FirstAidView _firstAidView;
        [SerializeField] private CardiopulmonaryResuscitationManager _cardiopulmonaryResuscitation;
        [SerializeField] private DemoKeypad _mobile;
        
        public Observable<GameplayExitParams> Run(DIContainer gameplayContainer, GameplayEnterParams gameplayEnterParams)
        {
            _gameplayContainer = gameplayContainer;

            var service = new FirstAidService(_firstAidView, gameplayEnterParams,
                _gameplayContainer.Resolve<Coroutines>());
            _exitSceneEvent = new Subject<Unit>();
            _cardiopulmonaryResuscitation.Init(service);
            _mobile.Init(service);
            
            BindGoToMenuEvent(_exitSceneEvent);
            
            var mainMenuEnterParams = new MainMenuEnterParams();
            var exitParams = new GameplayExitParams(mainMenuEnterParams);
            var exitSceneRequest = gameplayContainer.Resolve<Subject<Unit>>(AppConstans.EXIT_SCENE_REQUEST_TAG);
            var exitToMainMenuSceneSignal = exitSceneRequest.Select(_ => exitParams);
            
            return exitToMainMenuSceneSignal;
        }
        
        public void BindGoToMenuButton(Button button)
        {
            button.onClick.AddListener(() => { _exitSceneEvent.OnNext(Unit.Default); });
        }

        public void BindGoToMenuEvent(Subject<Unit> e)
        {
            _gameplayContainer.RegisterInstance(AppConstans.EXIT_SCENE_REQUEST_TAG, e);
        } 
    }
}