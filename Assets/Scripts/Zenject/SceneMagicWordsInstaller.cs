using SoftgamesAssignment.MagicWords.Dialogue;
using UnityEngine;
using Zenject;

namespace SoftgamesAssignment.MagicWords
{
    public class SceneMagicWordsInstaller: MonoInstaller
    {
        [Header("MonoBehaviours")]
        [SerializeField] private DialogueModel _dialogueModel;
        
        public override void InstallBindings()
        {
            _dialogueModel.Initialize();
            
            Container.Bind<DialogueModel>()
                .FromInstance(_dialogueModel)
                .AsSingle();
        }
    }
}
