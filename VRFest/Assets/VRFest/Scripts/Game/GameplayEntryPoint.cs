using BaCon;
using UnityEngine;
using R3;
using VRFest.Scripts.Game;

namespace mBuildngs
{

    public class GameplayEntryPoint : MonoBehaviour
    {
        
        private DIContainer _gameplayContainer;
        
        public Observable<GameplayExitParams> Run(DIContainer gameplayContainer)
        {
            _gameplayContainer = gameplayContainer;
            
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