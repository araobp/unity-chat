using Newtonsoft.Json;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class TestClient : RestClient
{

    [SerializeField]
    Text chatMessages;

    [SerializeField]
    InputField inputField;

    EndPoint ep;
    SessionId sessionId;

    int seq = 0;

    float timer;
    const float PERIOD = 3F;

    // Start is called before the first frame update
    void Start()
    {
        ep = new EndPoint();
        ep.baseUrl = Config.BASE_URL;

        Hashtable headers = new Hashtable
        {
            { LiveAgentHeader.X_LIVEAGENT_API_VERSION, Config.API_VERSION },
            { LiveAgentHeader.X_LIVEAGENT_AFFINITY, "null" }
        };

        Get(ep, "System/SessionId", headers, (err, text) =>
        {
            sessionId = JsonConvert.DeserializeObject<SessionId>(text);
            Debug.Log($"{sessionId.id}, {sessionId.key}, {sessionId.affinityToken}, {sessionId.clientPollTimeout}");

            headers.Clear();
            headers.Add(LiveAgentHeader.X_LIVEAGENT_API_VERSION, Config.API_VERSION);
            headers.Add(LiveAgentHeader.X_LIVEAGENT_AFFINITY, sessionId.affinityToken);
            headers.Add(LiveAgentHeader.X_LIVEAGENT_SESSION_KEY, sessionId.key);
            headers.Add(LiveAgentHeader.X_LIVEAGENT_SEQUENCE, ++seq);

            ChasitorInit init = new ChasitorInit
            {
                sessionId = sessionId.id
            };

            Post(ep, "Chasitor/ChasitorInit", headers, JsonConvert.SerializeObject(init), (err) =>
            {
                Debug.Log(JsonConvert.SerializeObject(init));
                Debug.Log(err);
            });

        });
    }

    public void OnEndEdit(string message) {
        inputField.text = "";

    }

    Hashtable headers = new Hashtable();
    bool waiting = false;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= PERIOD && !waiting)
        {
            timer = 0F;
            Debug.Log("Zero clear");
            headers.Clear();
            headers.Add(LiveAgentHeader.X_LIVEAGENT_API_VERSION, Config.API_VERSION);
            headers.Add(LiveAgentHeader.X_LIVEAGENT_AFFINITY, sessionId.affinityToken);
            headers.Add(LiveAgentHeader.X_LIVEAGENT_SESSION_KEY, sessionId.key);
            waiting = true;
            Get(ep, "System/Messages", headers, (err, text) =>
            {
                try
                {
                    Messages messages = JsonConvert.DeserializeObject<Messages>(text);
                    string agentName = messages.messages[0].message.name;
                    string messageFromAgent = messages.messages[0].message.text;
                    Debug.Log($"{agentName}: {messageFromAgent}");
                    chatMessages.text = $"{agentName}: {messageFromAgent}";
                }
                catch(Exception e)
                {
                    Debug.Log(e.StackTrace);
                }
                waiting = false;
            });
        }
    }
}
