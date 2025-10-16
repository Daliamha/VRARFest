using VRFest.Scripts.Game.Gameplay;

namespace VRFest.Scripts.Game.MainMenu
{
    public class MainMenuExitParams
    {
        public GameplayEnterParams TargetSceneEnterParams { get; }

        public MainMenuExitParams(GameplayEnterParams targetSceneEnterParams)
        {
            TargetSceneEnterParams = targetSceneEnterParams;
        }
    }
}