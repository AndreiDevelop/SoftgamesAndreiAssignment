using UnityEngine;
using Zenject;

namespace SoftgamesAssignment
{
    public class SceneLoadingInstaller : MonoInstaller
    {
        [Header("Managers")]
        [SerializeField] private SceneLoadingManager _sceneLoadingManager;
        
        public override void InstallBindings()
        {
            Container.Bind<SceneLoadingManager>().
                FromComponentInHierarchy().
                AsSingle();
        }
    }
}