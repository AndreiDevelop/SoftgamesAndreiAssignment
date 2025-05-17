using UnityEngine;
using Zenject;

namespace SoftgamesAssignment.PhoenixFire
{
    public class ScenePhoenixFireInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<FireTorchModel>()
                .AsSingle();
        }
    }
}
