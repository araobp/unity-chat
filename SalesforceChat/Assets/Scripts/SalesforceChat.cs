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

    bool m_Waiting = false;

    Headers m_Headers = new Headers();

    int m_Ack = -1;

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

            m_Headers.state = State.INITIALIZED;

            ChasitorInit chasitorInit = new ChasitorInit
            {
                sessionId = sessionId.id
            };

            Post(ep, "Chasitor/ChasitorInit", m_Headers.headers, JsonConvert.SerializeObject(chasitorInit), (err) =>
            {
                Debug.Log(JsonConvert.SerializeObject(chasitorInit));
                Debug.Log(err);
                m_Headers.state = State.MESSAGING;
            });

        });

        m_InputField.onEndEdit.AddListener(OnEndEdit);
    }

    public void OnEndEdit(string message)
    {
        if (m_Headers.state == State.MESSAGING)
        {
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
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Headers.state == State.MESSAGING && !m_Waiting)
        {
            // Long polling
            m_Waiting = true;
            Get(ep, $"System/Messages?ack={m_Ack}", m_Headers.headersForMessages, (err, text) =>
            {
                try
                {
                    Messages messages = JsonConvert.DeserializeObject<Messages>(text);
                    m_Ack = messages.sequence;
                    foreach (Message message in messages.messages)
                    {
                        string agentName = message.message.name;
                        string messageFromAgent = message.message.text;
                        Debug.Log($"{agentName}: {messageFromAgent}");
                        if (messageFromAgent != null)
                        {
                            m_ChatMessages.text = m_ChatMessages.text + "\n" + $"{agentName}: {messageFromAgent}";
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.Log(e.StackTrace);
                }
                m_Waiting = false;
            });
        }
    }
}
