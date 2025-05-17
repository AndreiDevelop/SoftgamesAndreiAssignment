using UnityEngine;

namespace SoftgamesAssignment.AceOfShadows.Card
{
    [System.Serializable]
    public struct CardDataEntity
    {
        public CardData CardData;
        public int Count;
    }
    
    [CreateAssetMenu(fileName = "CardsDataSO", menuName = "Scriptable Objects/new CardsDataSO")]
    public class CardsDataSO : ScriptableObject
    {
        [SerializeField] private CardDataEntity[] _cardsDataEntities;
        public CardDataEntity[] CardsDataEntities => _cardsDataEntities;
    }
}

