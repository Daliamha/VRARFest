using HurricaneVR.Framework.Components;

namespace VRFest.Scripts.Game.Gameplay
{
    public class DestructibleTarget : HVRDestructible
    {

        private StereoscopicVisionTestService _service;
        public void Init(StereoscopicVisionTestService service)
        {
            _service = service;
        }

        public override void AfterDestroy()
        {
            _service.AddScores(10);
        }
    }
}