using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNetwork : MonoBehaviourPunCallbacks
{
    [HideInInspector] public PhotonView view;
    private void Awake()
    {
        view = GetComponent<PhotonView>();
        //if (PhotonNetwork.IsMasterClient)
        //{
        //    InvokeRepeating("PlayerLoop", 0, 3f);
        //}
    }

    [PunRPC]
    public void Attack(float damage, int treeIndex, Player attacker,Vector3 attackerDirectionToTree, float remainingHealth = -1)
    {
        TreeScript tree = TreeScript.trees[treeIndex];
        if (PhotonNetwork.IsMasterClient)
        {
            if (tree.isDestroyed) return;

            float newHealth = tree.GetHealth() - damage;
            print(newHealth);
            if (newHealth <= 0 && attacker != null)
            {
                var hash = attacker.CustomProperties;
                hash["treesCut"] = (int)hash["treesCut"] + 1;
                attacker.SetCustomProperties(hash);
                view.RPC("DestroyTree", RpcTarget.All, treeIndex, attackerDirectionToTree);
                view.RPC("AddItem", attacker, tree.drops[0].id,(byte)Random.Range(tree.dropsCount.x, tree.dropsCount.y));
                return;
            }
            tree.TakeDamage(damage, attackerDirectionToTree);
            view.RPC("Attack", RpcTarget.Others, damage, treeIndex,null,attackerDirectionToTree, newHealth);
            return;
        }
        tree.SetHealth(remainingHealth,attackerDirectionToTree);
    }

    [PunRPC]
    public void RenewalTree(int index)
    {
        TreeScript.trees[index].Renewal();
    }
    [PunRPC]
    public void DestroyTree(int index, Vector3 attackerDirection)
    {
        StartCoroutine(TreeScript.trees[index].Destroy(attackerDirection));
    }

    [PunRPC]
    public void AddItem(short id, byte amount)
    {
        GetComponent<PlayerInventory>().inventory.AddItem(id, amount);
    }

    //void PlayerLoop()
    //{
    //    print(1);
    //    foreach(var player in GameObject.FindGameObjectsWithTag("Player"))
    //    {
    //        print(2);

    //        Vector3 playerPosition = player.transform.position;
    //        if (playerPosition.y <= 0)
    //        {
    //            print(3);

    //            player.transform.position = new Vector3(playerPosition.x, 130, playerPosition.z);
    //        }
    //    }
    //}

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            var hash = newPlayer.CustomProperties;
            hash.Add("treesCut", 0);
            newPlayer.SetCustomProperties(hash);
        }
        base.OnPlayerEnteredRoom(newPlayer);
    }
}
