using SoftgamesAssignment.Card;
using UnityEngine;
using Zenject;

namespace SoftgamesAssignment.AceOfShadows
{
    public class SceneAceOfShadowsInstaller : MonoInstaller
    {
        [Header("SO")]
        [SerializeField] private CardsDataSO _cardsDataSo;
        [SerializeField] private CardAnimationSettingsSO _cardAnimationSettingsSo;
        
        public override void InstallBindings()
        {
            var cardMode = new CardModel(_cardsDataSo, _cardAnimationSettingsSo);
            Container.Bind<CardModel>()
                .FromInstance(cardMode)
                .AsSingle();
        }
    }
}