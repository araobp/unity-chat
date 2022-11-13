using System.Collections;
using System.Collections.Generic;

public static class LiveAgentHeader
{
    public const string X_LIVEAGENT_API_VERSION = "X-LIVEAGENT-API-VERSION";
    public const string X_LIVEAGENT_AFFINITY = "X-LIVEAGENT-AFFINITY";
    public const string X_LIVEAGENT_SESSION_KEY = "X-LIVEAGENT-SESSION-KEY";
    public const string X_LIVEAGENT_SEQUENCE = "X-LIVEAGENT-SEQUENCE";
}

public enum State {
    INITIALIZING,
    INITIALIZED,
    MESSAGING
}

public class Headers
{
    private Hashtable m_Headers;
    private Hashtable m_HeadersForMessages;
    private int m_SeqNo;
    private State m_State;

    public Headers() {
        m_State = State.INITIALIZING;
        m_SeqNo = 1;
        m_Headers = new Hashtable()
        {
            { LiveAgentHeader.X_LIVEAGENT_API_VERSION, Config.API_VERSION },
            { LiveAgentHeader.X_LIVEAGENT_AFFINITY, "null" }
        };
        m_HeadersForMessages = (Hashtable)m_Headers.Clone();
    }

    public State state
    {
        set
        {
            m_State = value;
            if (m_State == State.INITIALIZED)
            {
                m_HeadersForMessages[LiveAgentHeader.X_LIVEAGENT_AFFINITY] = m_Headers[LiveAgentHeader.X_LIVEAGENT_AFFINITY];
                m_HeadersForMessages[LiveAgentHeader.X_LIVEAGENT_SESSION_KEY] = m_Headers[LiveAgentHeader.X_LIVEAGENT_SESSION_KEY];
            }
        }
        get => m_State;
    }

    public Hashtable headers
    {
        get {
            if (m_State != State.INITIALIZING)
            {
                m_Headers[LiveAgentHeader.X_LIVEAGENT_SEQUENCE] = m_SeqNo++;
            }
            return m_Headers;
        }
    }

    public Hashtable headersForMessages
    {
        get
        {
            if (m_State == State.MESSAGING)
            {
                return m_HeadersForMessages;
            } else
            {
                throw new System.Exception("Not in MESSAGING state");
            }
        }
    }

    public string affinity
    {
        set => m_Headers[LiveAgentHeader.X_LIVEAGENT_AFFINITY] = value;
        get => (string)m_Headers[LiveAgentHeader.X_LIVEAGENT_AFFINITY];
    }

    public string sessionKey
    {
        set => m_Headers[LiveAgentHeader.X_LIVEAGENT_SESSION_KEY] = value;
        get => (string)m_Headers[LiveAgentHeader.X_LIVEAGENT_SESSION_KEY];
    }
}

public class SessionId
{
    public string id;
    public string key;
    public string affinityToken;
    public int clientPollTimeout;
}

public class ChasitorInit
{
    public string organizationId { get; } = Config.ORGANIZATION_ID;
    public string deploymentId { get; } = Config.DEPLOYMENT_ID;
    public string buttonId { get; } = Config.BUTTON_ID;
    //public string agentId = "";
    public bool doFallback = true;
    public string sessionId;
    public string userAgent { get; } = Config.USER_AGENT;
    public string language { get; }  = Config.LANGUAGE;
    public string screenResolution { get; } = Config.SCREEN_RESOLUTION;
    public string visitorName { get; } = Config.VISITOR_NAME;
    public List<CustomDetail> prechatDetails = new List<CustomDetail>();
    public List<Entity> prechatEntities = new List<Entity>();
    public bool receiveQueueUpdates { get; }  = true;
    public bool isPost { get; }  = true;
}

public class EntityFieldMaps
{
    public string entityName;
    public string fieldName;
    public bool isFastFillable = false;
    public bool isAutoQueryable = true;
    public bool isExactMatchable = true;
}

public class CustomDetail
{
    public string label;
    public string value;
    public List<EntityFieldMaps> entityFieldMaps = new List<EntityFieldMaps>();
    public List<string> transcriptFields = new List<string>();
    public bool displayToAgent = true;
}

public class EntityFieldsMaps
{
    public string fieldName;
    public string label;
    public bool doFind = true;
    public bool isExactMath = true;
    public bool doCreate = true;
}

public class Entity
{
    public string entityName;
    public bool showOnCreate = true;
    public string linkToEntityName;
    public string linkToEntityField;
    public string saveToTranscript;
    public List<EntityFieldsMaps> entityFieldsMaps = new List<EntityFieldsMaps>();
}

public class Message
{
    public string type;
    public ChatMessageFromAgent message;
}

public class Messages
{
    public List<Message> messages;
    public int offset;
    public int sequence;
}

public class ChatMessageFromAgent
{
    public string name;
    public string text;
}

public class ChatMessageFromClient
{
    public string text;
}
