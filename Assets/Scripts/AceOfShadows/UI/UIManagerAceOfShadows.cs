using SoftgamesAssignment.AceOfShadows.Card;
using UniRx;
using UnityEngine;
using Zenject;

namespace SoftgamesAssignment.AceOfShadows
{
    public class UIManagerAceOfShadows : MonoBehaviour
    {
        [SerializeField] private FinishPopUp _finishPopUp;
        
        [Inject] private CardModel _cardModel;

        private string _textFinishPopUpDescription = "All cards are shown! \n" +
                                          "Please go to main menu and discover other modes!";
        
        void Start()
        {
            _cardModel.IsAllCardsShown
                .Subscribe(isAllCardsShown =>
                {
                    if (isAllCardsShown)
                    {
                        _finishPopUp.Activate(_textFinishPopUpDescription);
                    }
                }).AddTo(this);
        }
    }
}
