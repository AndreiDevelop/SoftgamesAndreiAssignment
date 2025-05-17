using System;
using Cysharp.Threading.Tasks;
using SoftgamesAssignment.MagicWords.Avatar;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SoftgamesAssignment.MagicWords.Dialogue
{
    public class DialoguePresenter : MonoBehaviour
    {
        [Header("Left")] 
        [SerializeField] private Image _leftAvatar;
        [SerializeField] private TextMeshProUGUI _leftText;
        
        [Header("Right")]
        [SerializeField] private Image _rightAvatar;
        [SerializeField] private TextMeshProUGUI _rightText;
        
        [Header("Empty Values")]
        [SerializeField] private Sprite _emptyAvatar;
        [SerializeField] private string _emptyText;
        
        [Header("Buttons")]
        [SerializeField] private Button _buttonNext;
        
        [Inject] private DialogueModel _dialogueModel;

        private void Awake()
        {
            SetEmptyValues();
        }

        private void OnEnable()
        {
            _buttonNext.onClick.AddListener(LoadDialog);
        }

        private void OnDisable()
        {
            _buttonNext.onClick.RemoveListener(LoadDialog);
        }

        private void SetEmptyValues()
        {
            _leftAvatar.sprite = _emptyAvatar;
            _leftText.text = _emptyText;

            _rightAvatar.sprite = _emptyAvatar;
            _rightText.text = _emptyText;
        }

        private void LoadDialog()
        {
            LoadNextDialogue().Forget();
        }
        
        private async UniTaskVoid LoadNextDialogue()
        {
            var dialog = await _dialogueModel.GetNextDialogue();

            if (dialog.Equals(default(DialogueResponse)))
            {
                Debug.LogWarning("There are no dialogs...");
                SetEmptyValues();
                return;
            }
            
            var dialogAvatar = _dialogueModel.GetAvatar(dialog.name);
            
            if(dialogAvatar.Equals(default(AvatarData)))
            {
                Debug.LogWarning("There are no avatars...");
                SetEmptyValues();
                return;
            }
            
            //handle Dialog text is missing
            string dialogText = String.IsNullOrEmpty(dialog.text) ? "..." : dialog.text;

            if (dialogAvatar.isLeft)
            {
                Debug.Log("Left dialog");
                _leftAvatar.sprite = dialogAvatar.sprite;
                _leftText.text = dialogText;
            }
            else
            {
                Debug.Log("Right dialog");
                _rightAvatar.sprite = dialogAvatar.sprite;
                _rightText.text = dialogText;
            }
        }
    }
}
