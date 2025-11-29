namespace VRFest.Scripts.Game.MainMenu
{
    public class MainMenuEnterParams
    {
        public bool HaveBreak { get; }
        
        public MainMenuEnterParams(bool haveBreak)
        {
            this.HaveBreak = haveBreak;
        }
        
        public MainMenuEnterParams()
        {
            HaveBreak = false;
        }
    }
}