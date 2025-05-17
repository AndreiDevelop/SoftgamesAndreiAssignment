using SoftgamesAssignment.MagicWords.Dialogue;
using UnityEngine;
using Zenject;

namespace MagicWords
{
    public class SceneMagicWordsInstaller: MonoInstaller
    {
        [Header("MonoBehaviours")]
        [SerializeField] private DialogueModel _dialogueModel;
        
        public override void InstallBindings()
        {
            Container.Bind<DialogueModel>()
                .FromInstance(_dialogueModel)
                .AsSingle();
        }
    }
}
