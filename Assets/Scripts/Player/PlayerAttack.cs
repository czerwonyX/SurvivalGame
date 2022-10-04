using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] Button attackButton;
    [SerializeField] AudioSource attackToTreeSound;
    [SerializeField] TMP_Text treesCutText;
    public float damage = 30;
    public float attackSpeed = 1;
    Transform player;
    private void Awake()
    {
        player = (PhotonNetwork.LocalPlayer.TagObject as GameObject).transform;
        attackButton.onClick.AddListener(() => Attack());
        InvokeRepeating("UpdateText", 0, 1f);
    }
    private void Attack()
    {
        Ray ray = new Ray(player.GetChild(1).position, player.GetChild(1).forward);
        RaycastHit hit;
        if (Physics.Raycast(ray,out hit, 5f))
        {
            TreeScript tree;
            Vector3 treePos;
            try
            {
                Transform treeTransform = hit.transform.parent.parent.parent.parent;
                treePos = treeTransform.position;
                treeTransform.TryGetComponent(out tree);
            }catch (NullReferenceException) { return; }
            //tree.TakeDamage(damage);
            if (tree.isDestroyed) return;

            treePos.y = 0;
            Vector3 playerPos = player.position;
            playerPos.y = 0;
            Vector3 dir = player.right;
            player.GetComponent<PlayerNetwork>().view.RPC("Attack", RpcTarget.MasterClient, damage, tree.index, PhotonNetwork.LocalPlayer,dir, -1f);
            attackToTreeSound.Play();
            treesCutText.text = "Trees cut: "+ PhotonNetwork.LocalPlayer.CustomProperties["treesCut"] + "";
        }
    }
    void UpdateText()
    {
        treesCutText.text = "Trees cut: " + PhotonNetwork.LocalPlayer.CustomProperties["treesCut"] + "";
    }
}
