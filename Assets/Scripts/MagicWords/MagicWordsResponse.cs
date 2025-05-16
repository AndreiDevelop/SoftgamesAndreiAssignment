using System.Collections.Generic;
using MagicWords.Avatar;
using MagicWords.Dialogue;
using UnityEngine;

namespace MagicWords
{
    [System.Serializable]
    public struct MagicWordsResponse
    {
        public List<DialogueResponse> dialogue;
        public List<AvatarResponse> avatars;
    }
}
