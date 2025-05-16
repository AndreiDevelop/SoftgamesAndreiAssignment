using System.Collections;
using System.Collections.Generic;
using MagicWords;
using MagicWords.Avatar;
using MagicWords.Dialogue;
using UniRx;
using UnityEngine;
using UnityEngine.Networking;

public class MagicWordsLoader : MonoBehaviour
{
    public ReactiveCommand<List<AvatarData>> OnAvatarDataLoaded = new ReactiveCommand<List<AvatarData>>();
    public ReactiveCommand<List<DialogueResponse>> OnDialogueDataLoaded = new ReactiveCommand<List<DialogueResponse>>();
    
    public string url = "https://private-624120-softgamesassignment.apiary-mock.com/v3/magicwords";

    void Start() 
    {
        StartCoroutine(LoadDialogueData());
    }

    IEnumerator LoadDialogueData()
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success) {
            Debug.LogError("JSON Load Failed: " + request.error);
            yield break;
        }

        string json = request.downloadHandler.text;
        MagicWordsResponse data = JsonUtility.FromJson<MagicWordsResponse>(json);

        // Process avatars
        Dictionary<string, Sprite> avatarMap = new Dictionary<string, Sprite>();
        List<AvatarData> avatarDataList = new List<AvatarData>();

        foreach (var avatar in data.avatars) {
            UnityWebRequest textureReq = UnityWebRequestTexture.GetTexture(avatar.url);
            yield return textureReq.SendWebRequest();

            if (textureReq.result == UnityWebRequest.Result.Success)
            {
                Texture2D tex = DownloadHandlerTexture.GetContent(textureReq);
                Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.one * 0.5f);
                avatarMap[avatar.name] = sprite;

                avatarDataList.Add(new AvatarData
                {
                    name = avatar.name, 
                    sprite = sprite, 
                    isLeft = avatar.position.Equals("left") ? true : false
                });
            }
            else
            {
                Debug.LogWarning($"Avatar load failed for {avatar.name}: {textureReq.error}");
            }
        }

        // Invoke OnAvatarDataLoaded
        OnAvatarDataLoaded.Execute(avatarDataList);

        // Invoke OnDialogueDataLoaded
        OnDialogueDataLoaded.Execute(data.dialogue);
    }
}
