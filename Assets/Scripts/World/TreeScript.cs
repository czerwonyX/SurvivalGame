using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TreeScript : MonoBehaviour
{
    public static Dictionary<int, TreeScript> trees = new Dictionary<int, TreeScript>();
    public int index;
    [SerializeField] float defaultHealth = 100;
    [SerializeField] float treeFallStrength = 0.7f;
    public InventoryItem[] drops { get; private set; }
    public Vector2Int dropsCount => new Vector2Int(1, 5);
    float hp;
    public bool isDestroyed { get; private set; }
    float remainingRenewalTime;
    float renewalTimeMax = 30;
    float renewalTimeMin = 10;
    float renewalCheckPeriod = 3;
    private void Awake()
    {
        //index = trees.Count;
        //trees.Add(index, this);
        hp = defaultHealth;
    }
    private void Start()
    {
        drops = new InventoryItem[1];
        drops[0] = InventorySystem.GetItemFromID(2);
        //  GetComponent<TextMesh>().text +=" b:"+ index + "";
    }
    public void OnEnable()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(RenewalCheckCorutine());
        }
    }
    public void SetHealth(float health, Vector3 direction)
    {
        hp = health;
        if (hp <= 0)
        {
            StartCoroutine(Destroy(direction));
        }
    }
    public float GetHealth() => hp;

    public float TakeDamage(float damage, Vector3 direction)
    {
        if ((hp -= damage) <= 0)
        {
            StartCoroutine(Destroy(direction));
        }
        return hp;
    }
    public IEnumerator Destroy(Vector3 direction)
    {
        isDestroyed = true;
        if (PhotonNetwork.IsMasterClient)
        {
            remainingRenewalTime = Random.Range(renewalTimeMin, renewalTimeMax);
            StartCoroutine(RenewalCheckCorutine());
        }
        for (int i = 0; i < 100; i++)
        {
            transform.Rotate(direction, treeFallStrength);
            yield return new WaitForSeconds(0.017f);
        }

        transform.GetChild(0).gameObject.SetActive(false);
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
    public void Renewal()
    {
        isDestroyed = false;
        transform.GetChild(0).gameObject.SetActive(true);
        hp = defaultHealth;
    }
    IEnumerator RenewalCheckCorutine()
    {
        yield return new WaitForSeconds(renewalCheckPeriod);
        if ((remainingRenewalTime--) <= 0)
        {
            (PhotonNetwork.LocalPlayer.TagObject as GameObject).GetComponent<PlayerNetwork>().view.RPC("RenewalTree", RpcTarget.All, index);
        }else
            StartCoroutine(RenewalCheckCorutine());
        
    }
}
