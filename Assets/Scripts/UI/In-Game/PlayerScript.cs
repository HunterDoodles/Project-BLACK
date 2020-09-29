using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace BLACK.NetworkedUI
{
public class PlayerScript : NetworkBehaviour
{
    public TextMesh playerNameText;
    public GameObject floatingName;

    private Material playerMaterialClone;
    private SceneScript sceneScript;

    [SyncVar(hook = nameof(OnNameChanged))]
    public string playerName;

    [SyncVar(hook = nameof(OnColorChanged))]
    public Color playerColor = Color.white;

    void OnNameChanged(string _Old, string _New)
    {
        playerNameText.text = playerName;
    }

    void OnColorChanged(Color _OLd, Color _New)
    {
        playerNameText.color = _New;
        playerMaterialClone = new Material(GetComponent<Renderer>().material);
        playerMaterialClone.color = _New;
        GetComponent<Renderer>().material = playerMaterialClone;
    }

    

    public override void OnStartLocalPlayer()
    {
        Camera.main.transform.SetParent(transform);
        Camera.main.transform.localPosition = new Vector3(0,0,0);

        floatingName.transform.localPosition = new Vector3(0, -0.3f, 0.6f);
        floatingName.transform.localScale = new Vector3(0,0,0);

        string name = "Player" + Random.Range(100, 999);
        Color color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        CmdSetupPlayer(name, color);
    }

    [Command]
    public void CmdSetupPlayer(string _name, Color _col)
    {
        // Player name sent to server, then server updates sync vars which handles it on all clients...
        playerName = _name;
        playerColor = _col;
        sceneScript.statusText = $"{playerName} joined.";
    }

    void Awake()
    {
        //allow all players to run this
        sceneScript = GameObject.FindObjectOfType<SceneScript>();
    }
    [Command]
    public void CmdSendPlayerMessage()
    {
        if (sceneScript)
        {
            sceneScript.statusText = $"{playerName} says hello {Random.Range(10, 99)}";
        }
    }

    void Update() 
    {
        if (!isLocalPlayer)
        {
            // Make non-local players run this...
            floatingName.transform.LookAt(Camera.main.transform);
            return;
        }
    }         
}
}
