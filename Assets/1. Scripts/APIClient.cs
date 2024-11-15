using System.Collections;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class Actor
{
    public int actor_id;
    public string first_name;
    public string last_name;
}

[System.Serializable]
public class ActorList
{
    public Actor[] actors;
}

public class APIClient : MonoBehaviour
{
    public TMP_InputField idStart, idEnd;
    public TextMeshProUGUI result;

    private string baseUrl = "http://localhost:8000";
    private string endPoint;
    //private int idStart, idEnd;

    private void Awake()
    {

    }

    void Start()
    {

    }

    public void ButtonGet()
    {
        endPoint = $"/read-by-id?id_start={idStart.text}&id_end={idEnd.text}";
        StartCoroutine(GetRequest(endPoint));
    }

    IEnumerator GetRequest(string endpoint)
    {
        UnityWebRequest request = UnityWebRequest.Get(baseUrl + endpoint);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(request.error);
        }
        else
        {
            string str = request.downloadHandler.text;
            ParseJson(str);
        }
    }

    //IEnumerator PostRequest(string endpoint, string jsonBody)
    //{
    //    UnityWebRequest request = new UnityWebRequest(baseUrl + endpoint, "POST");
    //    byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonBody);
    //    request.uploadHandler = new UploadHandlerRaw(bodyRaw);
    //    request.downloadHandler = new DownloadHandlerBuffer();
    //    request.SetRequestHeader("Content-Type", "application/json");

    //    yield return request.SendWebRequest();

    //    if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
    //    {
    //        Debug.LogError(request.error);
    //    }
    //    else
    //    {
    //        Debug.Log(request.downloadHandler.text);
    //    }
    //}

    void ParseJson(string json)
    {
        // 배열 형태의 JSON 문자열을 객체로 파싱하기 위해 래퍼 객체에 맞게 포맷 조정
        string wrappedJson = "{\"actors\":" + json + "}";
        ActorList actorList = JsonUtility.FromJson<ActorList>(wrappedJson);

        string res = "";
        foreach (Actor actor in actorList.actors)
        {
            //Debug.Log($"ID: {actor.actor_id}, First Name: {actor.first_name}, Last Name: {actor.last_name}");
            res += $"ID: {actor.actor_id.ToString().PadRight(5)}, First Name: {actor.first_name.PadRight(15)}, Last Name: {actor.last_name.PadRight(15)}\n";
        }

        result.text = res;

        print(res);
    }
}
