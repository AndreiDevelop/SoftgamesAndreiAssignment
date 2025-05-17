using DG.Tweening;
using UnityEngine;

namespace SoftgamesAssignment.AceOfShadows.Card
{
    [CreateAssetMenu(fileName = "CardAnimationSettingsSO", menuName = "Scriptable Objects/new CardAnimationSettingsSO")]
    public class CardAnimationSettingsSO : ScriptableObject
    {
        [Header("Flip")]
        [SerializeField] private float _flipDurationInSeconds = 0.5f;
        [SerializeField] private Ease _flipEase = Ease.OutBack;
        
        [Header("Move")]
        [SerializeField] private float _moveDurationInSeconds = 1f;
        [SerializeField] private PathType _movePathType = PathType.CatmullRom;
        [SerializeField] private Ease _moveEase = Ease.OutBack;
        
        [Header("Delay")]
        [SerializeField] private int _delayTakeTopCardInMilliseconds = 2000;
        
        public float FlipDurationInSeconds => _flipDurationInSeconds;
        public Ease FlipEase => _flipEase;
        
        public float MoveDurationInSeconds => _moveDurationInSeconds;
        public PathType MovePathType => _movePathType;
        public Ease MoveEase => _moveEase;
        
        public int DelayTakeTopCardInMilliseconds => _delayTakeTopCardInMilliseconds;
    }
}
