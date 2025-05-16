using System.Collections.Generic;
using NUnit.Framework;
using SoftgamesAssignment.Card;
using UniRx;
using UnityEngine;
using Zenject;

namespace SoftgamesAssignment
{
    public class DeckPresenter : MonoBehaviour
    {
        [SerializeField] private CardPresenter _cardPrefab;

        [SerializeField] private Transform _stackLeft;
        [SerializeField] private Transform _stackRight;
        
        [SerializeField] private Transform[] _firstCardMovePath;
        [SerializeField] private Transform[] _secondCardMovePath;

        [Inject] private CardModel _cardModel;

        private Stack<CardPresenter> _cardPresenters = new Stack<CardPresenter>();
        private float _cardHeight = 0.025f;
        
        void Start()
        {
            _cardModel.OnDeckCleared.Subscribe(_ =>
            {
                ClearDeck();
            }).AddTo(this);
            
            _cardModel.OnDeckCreated.Subscribe(cards =>
            {
                FillDeck(cards);
            }).AddTo(this);
            
            _cardModel.OnGetTopCard
                .Subscribe(cardOrder =>
                {
                    if (_cardPresenters == null || _cardPresenters.Count == 0)
                        return;

                    var card = _cardPresenters.Pop();
                    float cardYOffset = cardOrder * _cardHeight;
                    
                    card.transform.SetParent(_stackRight);
                    card.ShowCard(_firstCardMovePath, _secondCardMovePath, cardOrder, cardYOffset);
                }).AddTo(this);
            
            if(_cardModel.Deck.Count > 0)
            {
                FillDeck(_cardModel.Deck);
            }
        }

        private void FillDeck(List<CardData> cards)
        {
            int order = 0;
            float yPosition = 0;
            foreach (var cardData in cards)
            {
                var cardPresenter = Instantiate(_cardPrefab, _stackLeft);
                cardPresenter.transform.localPosition = new Vector3(0, yPosition, 0);
                cardPresenter.Init(cardData, order);
                _cardPresenters.Push(cardPresenter);
                
                yPosition += _cardHeight;
                order++;
            }
            
            _cardModel.StartTakeCardProcess();
        }

        private void ClearDeck()
        {
            foreach (var cardPresenter in _cardPresenters)
            {
                Destroy(cardPresenter.gameObject);
            }
            
            _cardPresenters.Clear();
        }
    }
}
