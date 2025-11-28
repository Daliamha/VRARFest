namespace VRFest.Scripts.Game.Gameplay.Contacts
{
    public class ContactWithPlayer : ContactsSomeObjectsManager
    {
        public override void OnCollison()
        {
            _service.OnCollisionWithPlayer();
        }
    }
}