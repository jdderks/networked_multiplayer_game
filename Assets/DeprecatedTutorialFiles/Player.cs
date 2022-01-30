using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private Rigidbody rb;
    public Client client;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float xMovement = Input.GetAxis("Horizontal");
        float yMovement = Input.GetAxis("Vertical");
        transform.Translate(xMovement,0,yMovement);

        if (xMovement != 0 || yMovement != 0)
        {
            string msg = "MOVE|" + xMovement.ToString() + "|" + yMovement.ToString();
            client.sendMessage(msg);
        }
    }
}
