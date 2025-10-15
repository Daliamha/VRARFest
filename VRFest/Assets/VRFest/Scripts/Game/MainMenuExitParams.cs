namespace mBuildngs
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