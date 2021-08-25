using Mirror;
using MultiUserKit;
using System.Collections.Generic;
using UnityEngine;

public class SkyflightNetworkManager : MultiUserKit.NetworkManager
{
    int skyflightPlayers = 0;
    Vector3 spawnPoint;
    Vector3 spawn1 = new Vector3(-0.5f, 3.3f, -5f);
    Vector3 spawn2 = new Vector3(-1f, -13f, -9.5f);

    public override void OnStartServer()
    {
        base.OnStartServer();
        NetworkServer.RegisterHandler<CreateUserMessage>(OnCreateCharacter);
    }

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

        //Oh my god this is so dirty. Dont do this. Bad dev. Bad.
        if (conn.connectionId == 0)
            spawnPoint = spawn1;
        else
            spawnPoint = spawn2;

        GameObject player = Instantiate(playerPrefab, spawnPoint, Quaternion.identity);
        NetworkServer.Spawn(player);

        player.GetComponent<NetworkUser>().PlayerName = message.userName;
        NetworkServer.AddPlayerForConnection(conn, player.gameObject);

        skyflightPlayers++;
        Debug.Log(skyflightPlayers + " Players.");

        ActivePlayers.Add(player.GetComponent<LocalUser>());
        Debug.Log("Active Players (1): " + ActivePlayers.Count);
    }

    public override void OnServerConnect(NetworkConnection conn)
    {
        base.OnServerConnect(conn);
        print("------CONN:--------");
        print(conn);
        print(conn.connectionId);
    }
}
