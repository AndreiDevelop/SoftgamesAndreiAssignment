using System;
using TMPro;
using UnityEngine;
using DG.Tweening;

namespace SoftgamesAssignment.Card
{
    public class CardPresenter : MonoBehaviour
    {
        [SerializeField] private Transform _holder;
        [SerializeField] private TextMeshPro _textName;
        [SerializeField] private SpriteRenderer _icon;
        [SerializeField] private SpriteRenderer _background;
        
        [Header("Animation properties")]
        [SerializeField] private float _flipDurationInSeconds = 0.5f;
        [SerializeField] private Ease _flipEase = Ease.OutBack;
        
        private Tween _flipTween;
        private bool _isShown = true;
        private Vector3 _shownRotation = new Vector3(0, 0, 0);
        private Vector3 _hiddenRotation = new Vector3(0, 180, 0);
        
        public void Init(CardData cardData)
        {
            _textName.text = cardData.Name;
            _icon.sprite = cardData.Icon;
            _background.sprite = cardData.Background;

            HideImmediately();
        }
        
        public void HideImmediately()
        {
            if(_flipTween!=null && _flipTween.IsActive())
            {
                _flipTween.Kill();
            }
            
            _holder.localRotation = Quaternion.Euler(_hiddenRotation);
        }
        
        public void FlipCard()
        {
            if(_flipTween!=null && _flipTween.IsActive())
            {
                _flipTween.Kill();
            }
            
            Vector3 cardRotation = _isShown ? _hiddenRotation : _shownRotation;
            
            _flipTween = _holder.DORotate(cardRotation, _flipDurationInSeconds).SetEase(_flipEase);
        }

        private void OnDestroy()
        {
            _flipTween.Kill();
        }
    }
}
