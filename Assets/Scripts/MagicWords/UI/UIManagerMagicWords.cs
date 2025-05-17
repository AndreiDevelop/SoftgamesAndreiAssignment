

using SoftgamesAssignment.MagicWords.Dialogue;
using UniRx;
using UnityEngine;
using Zenject;

namespace SoftgamesAssignment
{
    public class UIManagerMagicWords : MonoBehaviour
    {
        [SerializeField] private FinishPopUp _finishPopUp;
        
        [Inject] private DialogueModel _dialogueModel;

        private string _textFinishPopUpDescription = "Dialog is finished! \n" +
                                                     "Please go to main menu and discover other modes!";


        void Start()
        {
            if (_dialogueModel != null)
            {
                _dialogueModel.OnDialogueFinished
                    .Subscribe(_ =>
                    {
                        _finishPopUp.Activate(_textFinishPopUpDescription);
                    }).AddTo(this);
            }
        }
    }
}
