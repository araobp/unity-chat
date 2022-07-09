using Newtonsoft.Json;
using UnityEngine;

class SessionId
{
    public string id;
    public string key;
    public string affinityToken;
    public int clientPollTimeout;
}


public class TestClient : RestClient
{

    EndPoint ep;

    SessionId sessionId;
    
    // Start is called before the first frame update
    void Start()
    {
        ep = new EndPoint();
        ep.baseUrl = Config.BASE_URL;

        Get(ep, "/System/SessionId", (err, text) =>
        {
            sessionId = JsonConvert.DeserializeObject<SessionId>(text);
            Debug.Log($"{sessionId.id}, {sessionId.key}, {sessionId.affinityToken}, {sessionId.clientPollTimeout}");

        });
    }

    // Update is called once per frame
        void Update()
    {
        
    }
}
