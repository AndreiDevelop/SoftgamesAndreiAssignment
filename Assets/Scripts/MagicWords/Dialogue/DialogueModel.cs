using System;
using System.Collections.Generic;
using SoftgamesAssignment.MagicWords.Avatar;
using UniRx;
using UnityEngine;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;

namespace SoftgamesAssignment.MagicWords.Dialogue
{
    public class DialogueModel : MonoBehaviour
    {
        [SerializeField]
        private DialogueParserSO _dialogueParserSO;
        
        [SerializeField] 
        private Sprite _defaultAvatarSprite;

        public ReactiveCommand OnDialogueFinished = new ReactiveCommand();

        private List<AvatarData> _avatarDatas = new List<AvatarData>();
        private Queue<DialogueResponse> _dialogueQueue = new Queue<DialogueResponse>();
        
        private string _url = "https://private-624120-softgamesassignment.apiary-mock.com/v3/magicwords";
        private string _avatarLeftPosition = "left";
        private string _avatarNobodyName = "Nobody";

        private bool _isDataLoaded = false;
        
        public void Initialize()
        {
            LoadDialogueData().Forget();
        }
        
        public async UniTask<DialogueResponse> GetNextDialogue()
        {
            //if data not load - load it
            if (!_isDataLoaded)
            {
                do
                {
                    await LoadDialogueData();
                } while (!_isDataLoaded);
            }
 
            if (_dialogueQueue.Count == 0)
            {
                OnDialogueFinished.Execute();
                return default;
            }

            var dialogue = _dialogueQueue.Dequeue();
            return dialogue;
        }

        private async UniTask LoadDialogueData()
        {
            UnityWebRequest request = UnityWebRequest.Get(_url);

            try
            {
                await request.SendWebRequest();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return;
            }

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("JSON Load Failed: " + request.error);
                return;
            }

            string json = request.downloadHandler.text;
            MagicWordsResponse data = JsonUtility.FromJson<MagicWordsResponse>(json);

            _dialogueQueue = new Queue<DialogueResponse>();

            for (int i = 0; i < data.dialogue.Count; i++)
            {
                var dialogue = data.dialogue[i];

                dialogue.text = _dialogueParserSO.ParseText(dialogue.text);
                _dialogueQueue.Enqueue(dialogue);
            }

            // Process avatars
            Dictionary<string, Sprite> avatarMap = new Dictionary<string, Sprite>();

            foreach (var avatar in data.avatars)
            {
                UnityWebRequest textureReq = UnityWebRequestTexture.GetTexture(avatar.url);

                try
                {
                    await textureReq.SendWebRequest();
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                    continue;
                }

                Sprite sprite;

                if (textureReq.result == UnityWebRequest.Result.Success)
                {
                    Texture2D tex = DownloadHandlerTexture.GetContent(textureReq);
                    sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.one * 0.5f);

                    TryToRemoveNotValidDuplicates(avatar.name);
                }
                else
                {
                    Debug.LogWarning($"Avatar load failed for {avatar.name}: {textureReq.error}");
                    sprite = _defaultAvatarSprite;
                }

                avatarMap[avatar.name] = sprite;

                _avatarDatas.Add(new AvatarData
                {
                    name = avatar.name,
                    sprite = sprite,
                    isLeft = avatar.position.Equals(_avatarLeftPosition)
                });
            }

            _isDataLoaded = true;
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

