using System.Collections;
using BaCon;
using R3;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRFest.Scripts.Game.Gameplay;
using VRFest.Scripts.Game.MainMenu;
using VRFest.Scripts.Utils;


namespace VRFest.Scripts.Game.Root
{
    public class GameEntryPoint
    {
        private static GameEntryPoint _instance;
        private readonly Coroutines _coroutines;
        private readonly DIContainer _rootContainer = new();
        private DIContainer _cachedSceneContainer;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void AutostartGame()
        {
            _instance = new GameEntryPoint();
            _instance.StartGame();
        }

        private GameEntryPoint()
        {
            _coroutines = new GameObject("[COROUTINES]").AddComponent<Coroutines>();
            Object.DontDestroyOnLoad(_coroutines.gameObject);
            _rootContainer.RegisterInstance(_coroutines);
        }
        
        private void StartGame()
        {
            #if UNITY_EDITOR
                        var sceneName = SceneManager.GetActiveScene().name;
            
                        if (sceneName == Scenes.GAMEPLAY)
                        {
                            var gameplayEnterParams = new GameplayEnterParams("EducationButtonLoss", true);
                            _coroutines.StartCoroutine(LoadAndStartGame(gameplayEnterParams));
                            return;
                        }
            #endif
            
            _coroutines.StartCoroutine(LoadAndStartMainMenu());
        }

        private IEnumerator LoadAndStartGame(GameplayEnterParams enterParams)
        {
            _cachedSceneContainer?.Dispose();
            
            yield return LoadScene(Scenes.BOOT);
            yield return LoadScene(Scenes.GAMEPLAY);
            
            yield return null;
            
            var sceneEntryPoint = Object.FindFirstObjectByType<GameplayEntryPoint>();
            var gameplayContainer = _cachedSceneContainer = new DIContainer(_rootContainer);
            
            sceneEntryPoint.Run(gameplayContainer, enterParams).Subscribe(gameplayExitParams =>
            {
                _coroutines.StartCoroutine(LoadAndStartMainMenu());
            });
        }
        
        private IEnumerator LoadAndStartMainMenu(MainMenuEnterParams enterParams = null)
        {
            _cachedSceneContainer?.Dispose();
            
            yield return LoadScene(Scenes.BOOT);
            yield return LoadScene(Scenes.MAIN_MENU);
            
            yield return null;

            var sceneEntryPoint = Object.FindFirstObjectByType<MainMenuEntryPoint>();
            var mainMenuContainer = _cachedSceneContainer = new DIContainer(_rootContainer);
            sceneEntryPoint.Run(mainMenuContainer, enterParams).Subscribe(mainMenuExitParams =>
            {
                _coroutines.StartCoroutine(LoadAndStartGame(mainMenuExitParams));
            });
        }

        private IEnumerator LoadScene(string sceneName)
        {
            yield return SceneManager.LoadSceneAsync(sceneName);
        }
    }
}