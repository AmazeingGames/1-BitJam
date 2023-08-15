using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Singleton<Player>
{
    public CircleCollider2D Collider { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        Collider = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
