namespace VRFest.Scripts.Game.Gameplay.Contacts
{
    public class ContactWithSetka : ContactsSomeObjectsManager
    {
        public override void OnCollison()
        {
            _service.AddScores(10);
        }
    }
}