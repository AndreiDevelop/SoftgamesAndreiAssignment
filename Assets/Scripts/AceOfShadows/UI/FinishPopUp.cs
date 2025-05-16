using System;
using SoftgamesAssignment.Card;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SoftgamesAssignment
{
    public class FinishPopUp : MonoBehaviour
    {
        [SerializeField] private GameObject _holder;
        [SerializeField] private Button _buttonOk;
        
        [Inject] private CardModel _cardModel;
        
        void Start()
        {
            _cardModel.IsAllCardsShown
                .Subscribe(isAllCardsShown =>
                {
                    if (isAllCardsShown)
                    {
                        _holder.SetActive(true);
                    }
                }).AddTo(this);
        }

        private void OnEnable()
        {
            _buttonOk.onClick.AddListener(LoadMenuScene);
        }

        private void OnDisable()
        {
            _buttonOk.onClick.RemoveListener(LoadMenuScene);
        }

        private void LoadMenuScene()
        {
            SceneLoadingManager.Instance.LoadScene(Constants.SceneMenu);
        }
    }
}
