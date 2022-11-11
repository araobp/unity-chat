using System.Collections;
using System.Collections.Generic;

public static class LiveAgentHeader
{
    public const string X_LIVEAGENT_API_VERSION = "X-LIVEAGENT-API-VERSION";
    public const string X_LIVEAGENT_AFFINITY = "X-LIVEAGENT-AFFINITY";
    public const string X_LIVEAGENT_SESSION_KEY = "X-LIVEAGENT-SESSION-KEY";
    public const string X_LIVEAGENT_SEQUENCE = "X-LIVEAGENT-SEQUENCE";
}

public class Headers
{
    private Hashtable m_Headers;
    private int m_SeqNo;

    public Headers() {
        m_SeqNo = 1;
        m_Headers = new Hashtable()
        {
            { LiveAgentHeader.X_LIVEAGENT_API_VERSION, Config.API_VERSION },
            { LiveAgentHeader.X_LIVEAGENT_AFFINITY, "null" }
        };
    }

    public Hashtable headers
    {
        get {
            if (m_Headers.ContainsKey(LiveAgentHeader.X_LIVEAGENT_SESSION_KEY))
            {
                m_Headers[LiveAgentHeader.X_LIVEAGENT_SEQUENCE] = m_SeqNo++;
            }
            return m_Headers;
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
    public List<string> prechatDetails = new List<string>();
    public List<string> prechatEntities = new List<string>();
    public bool receiveQueueUpdates { get; }  = true;
    public bool isPost { get; }  = true;
}

public class Messages
{
    public List<Message> messages;
    public int offset;
    public int sequence;
}

public class Message
{
    public string type;
    public ChatMessageFromAgent message;
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
