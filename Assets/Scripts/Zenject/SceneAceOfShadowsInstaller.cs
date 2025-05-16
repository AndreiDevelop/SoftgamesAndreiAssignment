using SoftgamesAssignment.Card;
using UnityEngine;
using Zenject;

namespace SoftgamesAssignment
{
    public class SceneAceOfShadowsInstaller : MonoInstaller
    {
        [Header("SO")]
        [SerializeField] private CardsDataSO _cardsDataSo;

        public override void InstallBindings()
        {
            var cardMode = new CardModel(_cardsDataSo);
            Container.Bind<CardModel>()
                .FromInstance(cardMode)
                .AsSingle();
        }
    }
}