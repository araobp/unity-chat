using Newtonsoft.Json;
using UnityEngine;
using System.Collections;

public class TestClient : RestClient
{

    EndPoint ep;
    SessionId sessionId;

    int seq = 0;

    // Start is called before the first frame update
    void Start()
    {
        ep = new EndPoint();
        ep.baseUrl = Config.BASE_URL;

        Hashtable headers = new Hashtable();
        headers.Add(LiveAgentHeader.X_LIVEAGENT_API_VERSION, Config.API_VERSION);
        headers.Add(LiveAgentHeader.X_LIVEAGENT_AFFINITY, "null");

        Get(ep, "System/SessionId", headers, (err, text) =>
        {
            sessionId = JsonConvert.DeserializeObject<SessionId>(text);
            Debug.Log($"{sessionId.id}, {sessionId.key}, {sessionId.affinityToken}, {sessionId.clientPollTimeout}");

            headers.Clear();
            headers.Add(LiveAgentHeader.X_LIVEAGENT_API_VERSION, Config.API_VERSION);
            headers.Add(LiveAgentHeader.X_LIVEAGENT_AFFINITY, sessionId.affinityToken);
            headers.Add(LiveAgentHeader.X_LIVEAGENT_SESSION_KEY, sessionId.key);
            headers.Add(LiveAgentHeader.X_LIVEAGENT_SEQUENCE, ++seq);

            ChasitorInit init = new ChasitorInit();
            init.sessionId = sessionId.id;

            Post(ep, "Chasitor/ChasitorInit", headers, JsonConvert.SerializeObject(init), (err) =>
            {
                Debug.Log(JsonConvert.SerializeObject(init));
                Debug.Log(err);
            });

        });
    }

    // Update is called once per frame
        void Update()
    {
        
    }
}
