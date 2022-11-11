using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SalesforceChat : RestClient
{

    [SerializeField]
    Text m_ChatMessages;

    [SerializeField]
    InputField m_InputField;

    EndPoint ep;

    float m_Timer;
    const float PERIOD = 3F;

    Headers m_Headers = new Headers();

    // Start is called before the first frame update
    void Start()
    {
        ep = new EndPoint();
        ep.baseUrl = Config.BASE_URL;

        Get(ep, "System/SessionId", m_Headers.headers, (err, text) =>
        {
            SessionId sessionId = JsonConvert.DeserializeObject<SessionId>(text);
            Debug.Log($"{sessionId.id}, {sessionId.key}, {sessionId.affinityToken}, {sessionId.clientPollTimeout}");
            m_Headers.affinity = sessionId.affinityToken;
            m_Headers.sessionKey = sessionId.key;

            ChasitorInit chasitorInit = new ChasitorInit
            {
                sessionId = sessionId.id
            };

            Post(ep, "Chasitor/ChasitorInit", m_Headers.headers, JsonConvert.SerializeObject(chasitorInit), (err) =>
            {
                Debug.Log(JsonConvert.SerializeObject(chasitorInit));
                Debug.Log(err);
            });

        });

        m_InputField.onEndEdit.AddListener(OnEndEdit);
    }

    public void OnEndEdit(string message) {
        string inputText = m_InputField.text;
        m_InputField.text = "";

        ChatMessageFromClient body = new ChatMessageFromClient();
        body.text = inputText;

        Post(ep, "Chasitor/ChatMessage", m_Headers.headers, JsonConvert.SerializeObject(body), (err) =>
        {
            Debug.Log(JsonConvert.SerializeObject(body));
            Debug.Log(err);

            m_ChatMessages.text = m_ChatMessages.text + "\n" + $"{Config.VISITOR_NAME}: {inputText}";
        });
    }

    bool waiting = false;

    // Update is called once per frame
    void Update()
    {
        m_Timer += Time.deltaTime;
        if (m_Timer >= PERIOD && !waiting)
        {
            m_Timer = 0F;
            waiting = true;
            Get(ep, "System/Messages", m_Headers.headers, (err, text) =>
            {
                try
                {
                    Messages messages = JsonConvert.DeserializeObject<Messages>(text);
                    string agentName = messages.messages[0].message.name;
                    string messageFromAgent = messages.messages[0].message.text;
                    Debug.Log($"{agentName}: {messageFromAgent}");
                    if (messageFromAgent != null)
                    {
                        m_ChatMessages.text = m_ChatMessages.text + "\n"+ $"{ agentName}: {messageFromAgent}";
                    }
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
