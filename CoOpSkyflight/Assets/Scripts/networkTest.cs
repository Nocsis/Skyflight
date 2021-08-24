using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class networkTest : NetworkBehaviour
{

    void Start()
    {
        
    }
    
    [Command(requiresAuthority = false)]
    void CmdSendToServer(int stamp, Color32 color, Vector2 uv)
    {
        //DoStuff
        RpcReceive(stamp, color, uv);
    }

    [ClientRpc]
    void RpcReceive(int stamp, Color32 color, Vector2 uv)
    {
        //DoStuff

    }

    void Update()
    {
        CmdSendToServer(1, new Color32(1, 1, 1, 1), new Vector2(10,10));
    }
}
