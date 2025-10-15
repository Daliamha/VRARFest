namespace mBuildngs
{
    public class GameplayEnterParams
    {
        public bool isEducation { get; }
        public string nameOfbad { get; } 
        
        public GameplayEnterParams(string name, bool education)
        {
            isEducation = education;
            nameOfbad = name;
        }
        
        
    }
}