# Salesforce Einstein Bot integration with Unity

I lost my interest in this field after overwork in October 2022, so I terminate this project.

There are a lot of chat services in VR space:
- VRChat
- Photon

Once I used Photon for prototyping AR with younger people. Photon was nice.

---

(Work in progress, very experimental, still in very early stage)

## Architecture idea

```
                                                                Conncted App
  [Unity]---- Photon? ----[Gateway / Einstein Bot SDK / SpringBoot]-----[Einstein Bot w/ Knowledge base]
                             |                                    OAuth2
  [Other chat services]------+                                    Chat messaging

```

## Metaverse use cases

- Virtual showrooms
- Virtual exhibitions
- Virtual retail stores

## Experiment 1: REST API test (with Salesforce chat)

### Config.cs

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

## References

- [UnityWebRequest](https://docs.unity3d.com/2022.2/Documentation/ScriptReference/Networking.UnityWebRequest.html)
- [Understanding Chat REST Resources](https://developer.salesforce.com/docs/atlas.en-us.live_agent_rest.meta/live_agent_rest/live_agent_rest_understanding_resources.htm)
- https://help.salesforce.com/s/articleView?id=000331168&type=1
- [Get Started with Einstein Bots API](https://developer.salesforce.com/docs/service/einstein-bot-api/guide/prerequisites.html)
