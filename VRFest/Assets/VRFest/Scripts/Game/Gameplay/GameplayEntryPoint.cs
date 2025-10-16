using BaCon;
using UnityEngine;
using R3;
using VRFest.Scripts.Game.MainMenu;
using VRFest.Scripts.Game.Root;
using VRFest.Scripts.Utils;

namespace VRFest.Scripts.Game.Gameplay
{

    public class GameplayEntryPoint : MonoBehaviour
    {
        
        private DIContainer _gameplayContainer;
        [SerializeField] private FirstAidView _firstAidView;
        
        public Observable<GameplayExitParams> Run(DIContainer gameplayContainer, GameplayEnterParams gameplayEnterParams)
        {
            _gameplayContainer = gameplayContainer;

            var service = new FirstAidService(_firstAidView, gameplayEnterParams,
                _gameplayContainer.Resolve<Coroutines>());
            
            var mainMenuEnterParams = new MainMenuEnterParams();
            var exitParams = new GameplayExitParams(mainMenuEnterParams);
            var exitSceneRequest = gameplayContainer.Resolve<Subject<Unit>>(AppConstans.EXIT_SCENE_REQUEST_TAG);
            var exitToMainMenuSceneSignal = exitSceneRequest.Select(_ => exitParams);
            
            return exitToMainMenuSceneSignal;
        }
        
        public void BindGoToMenu(Subject<Unit> e)
        {
            _gameplayContainer.RegisterInstance(AppConstans.EXIT_SCENE_REQUEST_TAG, e);
        } 
    }
}