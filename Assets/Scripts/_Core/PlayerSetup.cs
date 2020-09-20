using UnityEngine;
using Mirror;

public class PlayerSetup : NetworkBehaviour
{
    // Index to disable any arbitrary amount of scripts that will be needed to allow multiplayer to function properly.
    [SerializeField]
    Behaviour[] componenetsToDisable;

    Camera sceneCamera;

    void Start()
    {
        // If you aren't the local player of this object, disable all components that aren't a part of YOUR gameobject in the listed index.
        if (!isLocalPlayer)
        {
            for (int i = 0; i < componenetsToDisable.Length; i++)
            {
                componenetsToDisable[i].enabled = false;
            }
        } 
        else
        {
            // Disables the scene camera (The camera that has an overview of the level before you currently connect to a server)
            sceneCamera = Camera.main;
            if (sceneCamera != null)
            {
                sceneCamera.gameObject.SetActive(false);
            }
        }
    }

    void OnDisable ()
    {
        if (sceneCamera != null)
        {
            sceneCamera.gameObject.SetActive(true);
        }
    }

}
