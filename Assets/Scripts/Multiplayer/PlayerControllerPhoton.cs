using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using Photon.Pun;

public class PlayerControllerPhoton : MonoBehaviourPunCallbacks, IPunObservable
{
    public TextMeshProUGUI lagText;
    public static GameObject LocalPlayerInstance;

    [SerializeField] List<SkinnedMeshRenderer> meshRenderers;
    [SerializeField] List<Material> materials;
    [SerializeField] int materialIndex;


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
        float lag = Mathf.Abs((float) (PhotonNetwork.Time - info.timestamp));
        
        if (stream.IsWriting)
        {
            // We own this player: send the others our data

            if ( lag != 0 )
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


    public void ChangeMaterial()
    {
        materialIndex++;
        if( materialIndex == materials.Count )
            materialIndex = 0;

        print("ChangeMaterial");
        photonView.RPC("SetMaterial", RpcTarget.AllBuffered, materialIndex );
    }

    [PunRPC]
    void SetMaterial( int x )
    {   
        foreach( SkinnedMeshRenderer mesh in meshRenderers )
        {
            Material[] m = mesh.materials;
            m[0] = materials[x];
            mesh.materials = m;
        }
    }


 
}
