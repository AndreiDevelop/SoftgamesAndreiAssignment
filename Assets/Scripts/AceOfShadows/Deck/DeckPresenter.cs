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
        
        [Inject] private CardModel _cardModel;

        private List<CardPresenter> _cardPresenters = new List<CardPresenter>();

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
            
            if(_cardModel.Deck.Count > 0)
            {
                FillDeck(_cardModel.Deck);
            }
        }
        
        //TODO flip cards
        //create custom shader for TMP
        private void FillDeck(List<CardData> cards)
        {
            foreach (var cardData in cards)
            {
                var cardPresenter = Instantiate(_cardPrefab, _stackLeft);
                cardPresenter.Init(cardData);
                _cardPresenters.Add(cardPresenter);
            }
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
