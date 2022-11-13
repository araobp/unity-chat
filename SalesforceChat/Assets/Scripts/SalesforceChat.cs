using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SalesforceChat : RestClient
{
    [SerializeField]
    GameObject m_PanelChat;

    [SerializeField]
    Text m_ChatMessages;

    [SerializeField]
    InputField m_InputField;

    [SerializeField]
    GameObject m_PanelPrechat;

    [SerializeField]
    InputField m_InputFieldFirstName;

    [SerializeField]
    InputField m_InputFieldLastName;

    [SerializeField]
    InputField m_InputFieldPhone;

    EndPoint ep;

    bool m_Waiting = false;

    Headers m_Headers = new Headers();

    int m_Ack = -1;

    string m_FirstName;
    string m_LastName;
    string m_Phone;
    string m_Subject = "TEST";

    const string PROPERTY = "FZK Haus";

    // Start is called before the first frame update
    void Start()
    {
        m_PanelChat.SetActive(false);
        m_PanelPrechat.SetActive(true);
    }

    public void OnStartChatting()
    {
        m_FirstName = m_InputFieldFirstName.text;
        m_LastName = m_InputFieldLastName.text;
        m_Phone = m_InputFieldPhone.text;
        Debug.Log($"{m_FirstName} {m_LastName} {m_Phone}");

        m_Subject = $"{PROPERTY} ({m_FirstName} {m_LastName})";

        ep = new EndPoint();
        ep.baseUrl = Config.BASE_URL;

        Get(ep, "System/SessionId", m_Headers.headers, (err, text) =>
        {
            SessionId sessionId = JsonConvert.DeserializeObject<SessionId>(text);
            Debug.Log($"{sessionId.id}, {sessionId.key}, {sessionId.affinityToken}, {sessionId.clientPollTimeout}");
            m_Headers.affinity = sessionId.affinityToken;
            m_Headers.sessionKey = sessionId.key;

            m_Headers.state = State.INITIALIZED;

            // [Reference] https://help.salesforce.com/s/articleView?id=000388107&type=1

            //--- EntityFieldMap ---
            EntityFieldMaps entityFieldMapFirstName = new EntityFieldMaps
            {
                entityName = "Contact",
                fieldName = "FirstName"
            };

            EntityFieldMaps entityFieldMapLastName = new EntityFieldMaps
            {
                entityName = "Contact",
                fieldName = "LastName"
            };

            EntityFieldMaps entityFieldMapPhone = new EntityFieldMaps
            {
                entityName = "Contact",
                fieldName = "Phone"
            };

            EntityFieldMaps entityFieldMapSubject = new EntityFieldMaps
            {
                entityName = "Case",
                fieldName = "Subject"
            };

            //--- CustomDetail ---
            CustomDetail lastName = new CustomDetail
            {
                label = "LastName",
                value = m_LastName
            };
            lastName.transcriptFields.Add("LastName__c");
            lastName.entityFieldMaps.Add(entityFieldMapFirstName);

            CustomDetail firstName = new CustomDetail
            {
                label = "FirstName",
                value = m_FirstName
            };
            firstName.transcriptFields.Add("FirstName__c");
            firstName.entityFieldMaps.Add(entityFieldMapLastName);

            CustomDetail phone = new CustomDetail
            {
                label = "Phone",
                value = m_Phone
            };
            phone.entityFieldMaps.Add(entityFieldMapPhone);

            CustomDetail subject = new CustomDetail
            {
                label = "Subject",
                value = m_Subject
            };
            subject.entityFieldMaps.Add(entityFieldMapSubject);

            //--- Entity ---
            Entity contact = new Entity
            {
                entityName = "Contact",
                saveToTranscript = "ContactId",
                linkToEntityName = "Case",
                linkToEntityField = "ContactId"
            };
            contact.entityFieldsMaps.Add(
                new EntityFieldsMaps
                {
                    fieldName = "LastName",
                    label = "LastName"
                }
            );
            contact.entityFieldsMaps.Add(
                new EntityFieldsMaps
                {
                    fieldName = "FirstName",
                    label = "FirstName"
                }
            );
            contact.entityFieldsMaps.Add(
                new EntityFieldsMaps
                {
                    fieldName = "Phone",
                    label = "Phone"
                }
            );

            Entity case_ = new Entity
            {
                entityName = "Case",
                saveToTranscript = "Case"
            };
            case_.entityFieldsMaps.Add(
                new EntityFieldsMaps
                {
                    fieldName = "Subject",
                    label = "Subject",
                    doFind = false,
                    isExactMath = false
                }
            );

            //--- ChasitorInit ---
            ChasitorInit chasitorInit = new ChasitorInit
            {
                sessionId = sessionId.id,
            };
            chasitorInit.prechatDetails.Add(firstName);
            chasitorInit.prechatDetails.Add(lastName);
            chasitorInit.prechatDetails.Add(phone);
            chasitorInit.prechatDetails.Add(subject);
            chasitorInit.prechatEntities.Add(contact);
            chasitorInit.prechatEntities.Add(case_);

            Post(ep, "Chasitor/ChasitorInit", m_Headers.headers, JsonConvert.SerializeObject(chasitorInit), (err) =>
            {
                Debug.Log(JsonConvert.SerializeObject(chasitorInit));
                Debug.Log(err);
                m_Headers.state = State.MESSAGING;
            });

        });

        m_InputField.onEndEdit.AddListener(OnEndEdit);

        m_PanelChat.SetActive(true);
        m_PanelPrechat.SetActive(false);
    }

    public void OnEndEdit(string message)
    {
        if (m_Headers.state == State.MESSAGING)
        {
            string inputText = m_InputField.text;
            m_InputField.text = "";

            ChatMessageFromClient body = new ChatMessageFromClient
            {
                text = inputText
            };

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
