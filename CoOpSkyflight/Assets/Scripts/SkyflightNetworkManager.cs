using Mirror;
using MultiUserKit;
using System.Collections.Generic;
using UnityEngine;

public class SkyflightNetworkManager : Mirror.NetworkManager
{
    public static SkyflightNetworkManager Instance;

    public Transform spawnPoint;

    int players = 0;

    private new void Awake()
    {
        DontDestroyOnLoad(this);
        Instance = this;

#if UNITY_STANDALONE_LINUX
        StartServer();
#endif
        }

    public override void OnStartServer()
    {
        base.OnStartServer();
        NetworkServer.RegisterHandler<CreateUserMessage>(OnCreateCharacter);
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);

        CreateUserMessage characterMessage = new CreateUserMessage
        {
            userName = UserNameInputFieldHandler.UserName == "" ? PlatformDetector.GetCurrentPlatform().label : UserNameInputFieldHandler.UserName
        };

        conn.Send(characterMessage);
    }

    public List<LocalUser> ActivePlayers = new List<LocalUser>();

    void OnCreateCharacter(NetworkConnection conn, CreateUserMessage message)
    {

        for (int x = 0; x < ActivePlayers.Count; x++)
        {
            if (ActivePlayers[x] == null)
            {
                Debug.Log("Removing player with ID: " + x);
                ActivePlayers.RemoveAt(x);
            }
        }

        spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint").transform;

        GameObject player = Instantiate(playerPrefab, spawnPoint == null ? new Vector3(0, 0, 0) : spawnPoint.position, Quaternion.identity);
        NetworkServer.Spawn(player);

        player.GetComponent<NetworkUser>().PlayerName = message.userName;
        NetworkServer.AddPlayerForConnection(conn, player.gameObject);

        players++;
        //Debug.Log(players + " Players.");

        ActivePlayers.Add(player.GetComponent<LocalUser>());
        //Debug.Log("Active Players (1): " + ActivePlayers.Count);
    }

    public static void CreateServer()
    {
        Instance.StartHost();
    }

    public static void JoinServer()
    {
        Instance.networkAddress = IPInputFieldHandler.IP;
        Instance.StartClient();
    }
}
