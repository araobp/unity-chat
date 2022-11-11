# Salesforce Chat integration with Unity

It also means Salesforce CRM integration with Unity.

```
    VIRTUAL WORLD                                         REAL WORLD

                                           Objects: LiveChatTranscript, Case, Lead ...
                                                               ^
                                                               |
                                                            Pop up
                                                               |
       Unity ---------- Chat initiation ------------> Salesforce Service Console
     3D Games
     Digital Twin 
     Metaverse

```

## Potential use cases

- Virtual housing exhibition
- Virtual retail stores
- Virtual sightseeing

## Salesforce Chat REST API message sequence diagram

```

  Unity                                 Salesforce Platform

    Initializing a chat session
    |                                            |
    |------- GET System/SessionId -------------->|
    |<------ 200 OK -----------------------------|
    |                                            |
    |------- POST Chasitor/ChasitorInit -------->|
    |<------ 200 OK -----------------------------|
    |                                            |
    
    Sending a message to the agent
    |                                            |
    |------- POST Chasitor/ChatMessage --------->|
    |<------ 200 OK -----------------------------|
    |                                            |
    
    Polling for fetching a message from the agent
    |                                            |
    |------- GET System/Messages --------------->|
    |------- 200 OK -----------------------------|
    |                                            |

```

## Config.cs

"Config.cs" is a config file for this application. However, it is not included in this repo for security/privacy reasons. Create "Config.cs" with the following constants in the "Scripts" folder:

```
using UnityEngine;

public class Config : MonoBehaviour
{
    public const string BASE_URL = "<Salesforce Chat REST server host>/chat/rest/";
    public const string ORGANIZATION_ID = "<your company's organization ID";
    public const string DEPLOYMENT_ID = "<deployment ID>";
    public const string BUTTON_ID = "<button ID>";
    public const string AGENT_ID = "";  // Empty for normal chat
    public const string USER_AGENT = "UnityWebRequest";
    public const string LANGUAGE = "ja-JP";
    public const string SCREEN_RESOLUTION = "2560x1440";
    public const string VISITOR_NAME = "<your name>";

    public const string API_VERSION = "55";
}
```

Refer to [this document](https://developer.salesforce.com/docs/atlas.en-us.noversion.service_sdk_ios.meta/service_sdk_ios/live_agent_cloud_setup_get_settings.htm).
## References

- [Chat REST API Developer Guide](https://resources.docs.salesforce.com/240/latest/en-us/sfdc/pdf/chat_rest.pdf)
- [Get Chat Settings from Your Org](https://developer.salesforce.com/docs/atlas.en-us.noversion.service_sdk_ios.meta/service_sdk_ios/live_agent_cloud_setup_get_settings.htm)
