using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using UniRx;
using Random = UnityEngine.Random;

namespace SoftgamesAssignment.Card
{
    public class CardModel
    {
        public ReactiveCommand<int> OnGetTopCard { get; } = new ReactiveCommand<int>();
        public ReactiveProperty<bool> IsAllCardsShown { get; } = new ReactiveProperty<bool>(false);
        public ReactiveCommand<List<CardData>> OnDeckCreated { get; } = new ReactiveCommand<List<CardData>>();
        public ReactiveCommand OnDeckCleared { get; } = new ReactiveCommand();
        
        private List<CardData> _deck = new List<CardData>();
        public List<CardData> Deck => _deck;
        
        private Stack<CardData> _hidenCards = new Stack<CardData>();
        private CardsDataSO _cardsDataSO;
        private CardAnimationSettingsSO _cardAnimationSettingsSO;
        
        private CancellationDisposable _cancellationTakeCardDisposable = new CancellationDisposable();
        
        public CardModel(CardsDataSO cardsDataSO, CardAnimationSettingsSO cardAnimationSettingsSO)
        {
            _cardsDataSO = cardsDataSO;
            _cardAnimationSettingsSO = cardAnimationSettingsSO;
            CreateDeck();
        }

        public void StartTakeCardProcess()
        {
            if (_cancellationTakeCardDisposable.Token != null &&
                _cancellationTakeCardDisposable.Token.CanBeCanceled)
            {
                _cancellationTakeCardDisposable.Dispose();
                _cancellationTakeCardDisposable = new CancellationDisposable();
            }

            TakeCardsProcess(_cancellationTakeCardDisposable.Token)
                .Forget();
        }
        
        public void CreateDeck()
        {
            OnDeckCleared.Execute();
            _deck.Clear();
            
            foreach (var cardDataEntity in _cardsDataSO.CardsDataEntities)
            {
                for (int i = 0; i < cardDataEntity.Count; i++)
                {
                    _deck.Add(cardDataEntity.CardData);
                    _hidenCards.Push(cardDataEntity.CardData);
                }
            }

            ShuffleDeck();
            
            OnDeckCreated.Execute(_deck);
        }
        
        public void ShuffleDeck()
        {
            for (int i = 0; i < _deck.Count; i++)
            {
                int randomIndex = Random.Range(i, _deck.Count);
                CardData temp = _deck[i];
                _deck[i] = _deck[randomIndex];
                _deck[randomIndex] = temp;
            }
        }

        private void TakeCardFromTop(int cardOrder)
        {
            if(_hidenCards.Count == 0)
            {
                Debug.Log("There are no hidden cards.");
                IsAllCardsShown.Value = true;
                return;
            }
            
            _hidenCards.Pop();

            OnGetTopCard?.Execute(cardOrder);
        }

        private async UniTaskVoid TakeCardsProcess(CancellationToken cancellationToken)
        {
            if(_hidenCards.Count == 0)
            {
                Debug.Log("There are no hidden cards.");
                IsAllCardsShown.Value = true;
                return;
            }

            int cardOrder = 0;
            do
            {
                try
                {
                    await UniTask.Delay(_cardAnimationSettingsSO.DelayTakeTopCardInMilliseconds, 
                        cancellationToken: cancellationToken);
                    
                    TakeCardFromTop(cardOrder);
                    cardOrder++;
                }
                catch (OperationCanceledException)
                {
                    Debug.Log("TakeCardsProcess was canceled.");
                }
            } while (_hidenCards.Count > 0);
            
            IsAllCardsShown.Value = true;
        }
    }
}
