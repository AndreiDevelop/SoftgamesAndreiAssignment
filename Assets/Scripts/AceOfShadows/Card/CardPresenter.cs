using System;
using TMPro;
using UnityEngine;
using DG.Tweening;

namespace SoftgamesAssignment.AceOfShadows.Card
{
    public class CardPresenter : MonoBehaviour
    {
        [SerializeField] private Transform _holder;
        
        [Header("Front Side")]
        [SerializeField] private TextMeshPro _textName;
        [SerializeField] private SpriteRenderer _icon;
        [SerializeField] private SpriteRenderer _background;
        
        [Header("Back side")]
        [SerializeField] private SpriteRenderer _iconBack;
        [SerializeField] private SpriteRenderer _backgroundBack;
        
        [SerializeField] private CardAnimationSettingsSO _cardAnimationSettings;
        
        
        private Tween _flipTween;

        private bool _isShown = true;
        private Vector3 _shownRotation = new Vector3(0, 0, 0);
        private Vector3 _hiddenRotation = new Vector3(0, 180, 0);

        private int _orderLayer;
        
        public void Init(CardData cardData, int order)
        {
            _textName.text = cardData.Name;
            _icon.sprite = cardData.Icon;
            _background.sprite = cardData.Background;

            _orderLayer = order;
            IncreaseOrderInLayer(_orderLayer);
            HideImmediately();
        }

        private void ResetOrderLayer()
        {
            IncreaseOrderInLayer(_orderLayer * (-1));
        }
        
        private void IncreaseOrderInLayer(int increaseValue)
        {
            _icon.sortingOrder += increaseValue;
            _background.sortingOrder += increaseValue;
            
            _iconBack.sortingOrder += increaseValue;
            _backgroundBack.sortingOrder += increaseValue;
            
            _textName.sortingOrder += increaseValue;
        }
        
        public void HideImmediately()
        {
            if(_flipTween!=null && _flipTween.IsActive())
            {
                _flipTween.Kill();
            }
            
            _isShown = false;
            _holder.localRotation = Quaternion.Euler(_hiddenRotation);
        }

        public void ShowCard(Transform[] firstPath, Transform[] secondPath, int cardOrder, float cardYOffset)
        {
            MoveCard(firstPath, 0,() =>
            {
                ResetOrderLayer();
                FlipCard(() =>
                {
                    IncreaseOrderInLayer(cardOrder);
                    MoveCard(secondPath, cardYOffset);
                });
            });
        }
        
        public void MoveCard(Transform[] destinations, float cardYOffset, Action OnComplete = null)
        {
            if (destinations == null || destinations.Length == 0)
            {
                Debug.LogWarning("No destinations provided for MoveCard.");
                return;
            }

            // Extract positions from the destination transforms
            Vector3[] path = Array.ConvertAll(destinations, destination => destination.position);
            
            //offset card by Y
            int lastIndex = path.Length - 1;
            path[lastIndex] = new Vector3(
                path[lastIndex].x, 
                path[lastIndex].y + cardYOffset, 
                path[lastIndex].z);
            
            
            // Animate the card along the path
            transform.DOPath(path, _cardAnimationSettings.MoveDurationInSeconds, 
                    _cardAnimationSettings.MovePathType)
                .SetEase(_cardAnimationSettings.MoveEase).OnComplete(() =>
                {
                    OnComplete?.Invoke();
                });
        }
        
        private void FlipCard(Action OnComplete = null)
        {
            if(_flipTween!=null && _flipTween.IsActive())
            {
                _flipTween.Kill();
            }
            
            Vector3 cardRotation = _isShown ? _hiddenRotation : _shownRotation;
            
            _isShown = !_isShown;
            _flipTween = _holder.DOLocalRotate(cardRotation, 
                _cardAnimationSettings.FlipDurationInSeconds)
                .OnComplete(() =>
            {
                OnComplete?.Invoke();
            }).SetEase(_cardAnimationSettings.FlipEase);
        }
        
        private void OnDestroy()
        {
            _flipTween.Kill();
        }
    }
}
