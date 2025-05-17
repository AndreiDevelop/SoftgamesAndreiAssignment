using System.Collections.Generic;
using SoftgamesAssignment.MagicWords.Avatar;
using SoftgamesAssignment.MagicWords.Dialogue;

namespace SoftgamesAssignment.MagicWords
{
    [System.Serializable]
    public struct MagicWordsResponse
    {
        public List<DialogueResponse> dialogue;
        public List<AvatarResponse> avatars;
    }
}
