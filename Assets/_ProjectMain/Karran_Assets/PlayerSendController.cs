using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using Photon.Pun;

public class PlayerSendController : MonoBehaviourPunCallbacks, IPunObservable
{
    public TextMeshProUGUI lagText;
    public static GameObject LocalPlayerInstance;

    [SerializeField] List<SkinnedMeshRenderer> meshRenderers;
    [SerializeField] List<Material> materials;
    [SerializeField] int materialIndex;
    public static PlayerSendController instance;

    public void Awake()
    {
        // #Important
        // used in GameManager.cs: we keep track of the localPlayer instance to prevent instanciation when levels are synchronized
        if (photonView.IsMine)
        {
            LocalPlayerInstance = gameObject;
        }

    }

    public bool IsPhotonMine()
    {
        return photonView.IsMine;
    }

    float lag = 0;
    // getting / sending data
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.timestamp));

        if (stream.IsWriting)
        {
            // We own this player: send the others our data

            if (lag != 0)
            {
                lagText.text = "Lag: " + lag + " ms";
            }
            else
            {

            }

        }
        else
        {
            // Network player, receive data
        }
    }


    public void ChangeAvatar(int index)
    {
        Debug.LogError("ChangeMaterial");
        photonView.RPC("SetIndex", RpcTarget.All, index);
    }

    [PunRPC]
    void SetIndex(int x)
    {
        Debug.LogError("Teesting " + x);
    }



}
