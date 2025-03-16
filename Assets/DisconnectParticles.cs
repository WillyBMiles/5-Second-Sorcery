using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DisconnectParticles : MonoBehaviour
{
    List<ParticleSystem> systems;

    private void Awake()
    {
        systems = GetComponentsInChildren<ParticleSystem>().ToList();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Disconnect()
    {

        foreach (ParticleSystem system in systems)
        {
            system.Stop(true);
            system.transform.parent = null;

        }
    }
}
