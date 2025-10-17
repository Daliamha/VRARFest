using BaCon;
using UnityEngine;
using R3;
using VRFest.Scripts.Game.Gameplay;

namespace VRFest.Scripts.Game.MainMenu
{

    public class MainMenuEntryPoint : MonoBehaviour
    {
        private Subject<GameplayEnterParams> _exitSceneSignalSubj;
        
        public Observable<GameplayEnterParams> Run(DIContainer mainMenuContainer, MainMenuEnterParams enterParams)
        {
            var exitSignalSubj = new Subject<GameplayEnterParams>();

            Debug.Log(12313123123123);
            Bind(exitSignalSubj);
            return exitSignalSubj;
        }
        
        public void HanleGoToGameplayButtonClicked(GameObject button)
        {
            bool isEducation = button.name.Contains("Education");
            var enterParams = new GameplayEnterParams(button.name, isEducation);
            _exitSceneSignalSubj.OnNext(enterParams);
        }

        public void BindExitButton()
        {
            Debug.Log("Bind ExitButton");
            Application.Quit();
        }
        
        private void Bind(Subject<GameplayEnterParams> exitSceneSignalSubj)
        {
            _exitSceneSignalSubj = exitSceneSignalSubj;
        }
    }
}