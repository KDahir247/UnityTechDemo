using System;
using System.Collections.Generic;
using UnityEngine;

namespace Experimental.Network
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        public static Dictionary<int, PlayerManager> players = new Dictionary<int, PlayerManager>();

        public GameObject localPlayerPrefab;
        public GameObject playerPrefab;
        
        
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(this);
            }
        }


        public void SpawnPlayers(int id, string userName, Vector3 position, Quaternion rotation)
        {
            GameObject player;

            if (id == Client.instance.myId)
            {
                player = Instantiate(localPlayerPrefab, position, rotation);
            }
            else
            {
                player = Instantiate(playerPrefab, position, rotation);
            }

            player.GetComponent<PlayerManager>().id = id;
            player.GetComponent<PlayerManager>().username = userName;
            players.Add(id, player.GetComponent<PlayerManager>());
        }
        
    }
}