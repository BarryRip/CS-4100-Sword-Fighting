using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject parent;
    private FighterAgent agent;

    void Start()
    {
        parent = transform.parent.gameObject;
        agent = parent.GetComponent<FighterAgent>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Calling CompareTag on the collider will check the tag of the sword.
        // If we call CompareTag on the gameobject itself, it checks the tag of the parent agent instead.
        if (collision.collider.CompareTag("Body"))
        {
            agent.OnSwordHit();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
