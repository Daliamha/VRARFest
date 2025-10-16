namespace VRFest.Scripts.Game.Gameplay
{
    public class GameplayEnterParams
    {
        public bool isEducation { get; }
        public string nameOfBad { get; } 
        
        public GameplayEnterParams(string name, bool education)
        {
            isEducation = education;
            nameOfBad = name;
        }
        
        
    }
}