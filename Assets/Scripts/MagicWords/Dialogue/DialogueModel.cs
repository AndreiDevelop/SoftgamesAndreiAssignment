using System.Collections;
using System.Collections.Generic;
using MagicWords.Avatar;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Serialization;

namespace MagicWords.Dialogue
{
    public class DialogueModel : MonoBehaviour
    {
        [SerializeField] 
        private Sprite _defaultAvatarSprite;

        public ReactiveProperty<bool> OnDialogueFinished = new ReactiveProperty<bool>(false);
        

        private List<AvatarData> _avatarDatas = new List<AvatarData>();
        private Queue<DialogueResponse> _dialogueQueue = new Queue<DialogueResponse>();
        
        private string _url = "https://private-624120-softgamesassignment.apiary-mock.com/v3/magicwords";
        private string _avatarLeftPosition = "left";
        private string _avatarNobodyName = "Nobody";
        
        public void Awake()
        {
            StartCoroutine(LoadDialogueData());
        }

        public DialogueResponse GetNextDialogue()
        {
            if (_dialogueQueue.Count == 0)
            {
                OnDialogueFinished.Value = true;
                return default;
            }

            var dialogue = _dialogueQueue.Dequeue();
            return dialogue;
        }
        
        
        //TODO make uniTask
        private IEnumerator LoadDialogueData()
        {
            UnityWebRequest request = UnityWebRequest.Get(_url);
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("JSON Load Failed: " + request.error);
                yield break;
            }

            string json = request.downloadHandler.text;
            MagicWordsResponse data = JsonUtility.FromJson<MagicWordsResponse>(json);
            
            _dialogueQueue = new Queue<DialogueResponse>();

            foreach (var dialogue in data.dialogue)
            {
                _dialogueQueue.Enqueue(dialogue);
            }
            
            // Process avatars
            Dictionary<string, Sprite> avatarMap = new Dictionary<string, Sprite>();

            foreach (var avatar in data.avatars)
            {
                UnityWebRequest textureReq = UnityWebRequestTexture.GetTexture(avatar.url);
                yield return textureReq.SendWebRequest();

                Sprite sprite;

                if (textureReq.result == UnityWebRequest.Result.Success)
                {
                    Texture2D tex = DownloadHandlerTexture.GetContent(textureReq);
                    sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.one * 0.5f);
                    
                    TryToRemoveNotValidDuplicates(avatar.name);
                }
                else
                {
                    //handle avatar icon is missing
                    Debug.LogWarning($"Avatar load failed for {avatar.name}: {textureReq.error}");
                    sprite = _defaultAvatarSprite;
                }

                avatarMap[avatar.name] = sprite;

                _avatarDatas.Add(new AvatarData
                {
                    name = avatar.name,
                    sprite = sprite,
                    isLeft = avatar.position.Equals(_avatarLeftPosition) ? true : false
                });
            }
        }

        private void TryToRemoveNotValidDuplicates(string avatarName)
        {
            var avatarData = GetAvatar(avatarName);

            if (avatarData.sprite == _defaultAvatarSprite)
            {
                _avatarDatas.Remove(avatarData);
            }
        }
        
        public AvatarData GetAvatar(string avatarName)
        {
            AvatarData avatarData = default;

            if (_avatarDatas != null &&
                _avatarDatas.Count > 0 &&
                !string.IsNullOrEmpty(avatarName))
            {
                
                int avatarIndex = _avatarDatas.FindIndex(x => x.name.Equals(avatarName));

                if (avatarIndex == -1)
                {
                    Debug.LogWarning("Avatar not found: " + avatarName + " create new Avatar");
                    var nobodyAvatar = _avatarDatas.Find(x => x.name.Equals(_avatarNobodyName));

                    //add new avatar
                    _avatarDatas.Add(new AvatarData
                    {
                        name = avatarName,
                        sprite = _defaultAvatarSprite,
                        isLeft = nobodyAvatar.isLeft
                    });
                    
                    avatarData = _avatarDatas[_avatarDatas.Count - 1];
                }
                else
                {
                    avatarData = _avatarDatas[avatarIndex];
                }
                
            }

            return avatarData;
        }
    }
}

