# Salesforce Live Chat integration with Unity

(Work in progress, very experimental project)

## Config.cs

"Config.cs" is not included in this repo for security/privacy reasons. Create "Config.cs" with the following constants in the "Scripts" folder:

```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Config : MonoBehaviour
{
    public const string BASE_URL = "...";
    public const string ORGANIZATION_ID = "...";
            :
}
```

## References

- https://developer.salesforce.com/docs/atlas.en-us.live_agent_rest.meta/live_agent_rest/live_agent_rest_understanding_resources.htm
- https://help.salesforce.com/s/articleView?id=000331168&type=1

