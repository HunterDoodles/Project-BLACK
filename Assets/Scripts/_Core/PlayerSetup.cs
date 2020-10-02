using UnityEngine;
using UnityEngine.UI;
using Mirror;
using BLACK.NetworkedUI;

public class PlayerSetup : NetworkBehaviour
{
    // Index to disable any arbitrary amount of scripts that will be needed to allow multiplayer to function properly.
    [SerializeField]
    Behaviour[] componenetsToDisable;

    Camera sceneCamera;

    public int maxHealth = 100;
    public int currentHealth;

    public HealthBar healthBar;

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
            GameObject.Find("Player UI/Radar").GetComponent<Radar>().playerPos = this.transform;
        }

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }
    void Update() 
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            TakeDamage(17);
            Debug.Log("Taking Damage!");
        }
    }
    void OnDisable ()
    {
        if (sceneCamera != null)
        {
            sceneCamera.gameObject.SetActive(true);
        }
    }
    void TakeDamage(int damage)
    {
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);
    }

}
