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
        GameObject other = collision.gameObject;
        if (other.CompareTag("Body"))
        {
            agent.OnSwordHit();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
