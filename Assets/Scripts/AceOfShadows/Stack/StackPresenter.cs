using SoftgamesAssignment.AceOfShadows.Card;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace SoftgamesAssignment.AceOfShadows
{
    public class StackPresenter : MonoBehaviour
    {
        [SerializeField] private TextMeshPro _textCounter;
        [SerializeField] private bool _inverseCounter;
        
        [Inject] private CardModel _cardModel;
        
        private int _cardsCount;
        private int _currentShowCardCount;
        
        public void Start()
        {
            _cardsCount = _cardModel.Deck.Count;
            _currentShowCardCount = 0;
            UpdateCounter();
            
            _cardModel.OnGetTopCard
                .Subscribe(cardOrder =>
                {
                    _currentShowCardCount++;
                    UpdateCounter();
                }).AddTo(this);
        }

        private void UpdateCounter()
        {
            if (_inverseCounter)
            {
                _textCounter.text = $"{_cardsCount - _currentShowCardCount} / {_cardsCount}";
            }
            else
            {
                _textCounter.text = $"{_currentShowCardCount} / {_cardsCount}";
            }
        }
    }
}
