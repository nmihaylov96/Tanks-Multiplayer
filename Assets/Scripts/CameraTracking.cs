using Photon.Pun;
using UnityEngine;

public class CameraTracking : MonoBehaviour
{
    public Transform playerCharacter;
    private Vector3 cameraOffset;
    private bool isMultiplayer;

    void Start()
    {
        isMultiplayer = PhotonNetwork.IsConnectedAndReady;

        if (!isMultiplayer)
        {
            if (playerCharacter != null)
            {
                cameraOffset = transform.position - playerCharacter.position;
                Debug.Log("Player character transform is assigned: " + playerCharacter.name);
            }
            else
            {
                Debug.LogError("Player character transform is not assigned to CameraTracking script!");
            }
        }
    }

    void LateUpdate()
    {
        if (isMultiplayer)
        {
            if (playerCharacter == null)
            {
                var localPlayer = FindLocalPlayer();
                if (localPlayer != null)
                {
                    playerCharacter = localPlayer.transform;
                    cameraOffset = transform.position - playerCharacter.position;
                    Debug.Log("Multiplayer local player character assigned: " + playerCharacter.name);
                }
            }
        }

        if (playerCharacter != null)
        {
            transform.position = playerCharacter.position + cameraOffset;
        }
    }

    private GameObject FindLocalPlayer()
    {
        var players = GameObject.FindGameObjectsWithTag("Player");
        foreach (var player in players)
        {
            var photonView = player.GetComponent<PhotonView>();
            if (photonView != null && photonView.IsMine)
            {
                return player;
            }
        }
        return null;
    }
}
