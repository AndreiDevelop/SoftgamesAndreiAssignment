using UnityEngine;

namespace SoftgamesAssignment.MagicWords.Dialogue
{
    [System.Serializable]
    public struct DialogParserEntity
    {
        public string originalString;
        public string parsedString;
    }
    
    [CreateAssetMenu(fileName = "DialogueParserSO", menuName = "Scriptable Objects/new DialogueParserSO")]
    public class DialogueParserSO : ScriptableObject
    {
        [SerializeField] private DialogParserEntity[] _dialogParserEntities;
        
        public string ParseText(string input)
        {
            foreach (var entity in _dialogParserEntities)
            {
                if (input.Contains(entity.originalString))
                {
                    input = input.Replace(entity.originalString, entity.parsedString);
                }
            }

            return input;
        }
    }
}
