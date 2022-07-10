# Salesforce Live Chat integration with Unity

(Work in progress, very experimental project)

## Use cases

- Virtual showrooms
- Virtual retail stores

## Architecture

```
Virtual 3D world
[UnityWebRequest]------ REST ------[Live Chat]
```

## Config.cs

"Config.cs" is a config file for this application. However, it is not included in this repo for security/privacy reasons. Create "Config.cs" with the following constants in the "Scripts" folder:

```
using UnityEngine;

public class Config : MonoBehaviour
{
    public const string BASE_URL = "<Live chat REST server host>/chat/rest/";
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

- https://developer.salesforce.com/docs/atlas.en-us.live_agent_rest.meta/live_agent_rest/live_agent_rest_understanding_resources.htm
- https://help.salesforce.com/s/articleView?id=000331168&type=1

