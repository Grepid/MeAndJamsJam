using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockDrop : MonoBehaviour,Iinteractable
{
    private void Start()
    {
        Debug.DrawRay(transform.position, PlayerController.instance.transform.position - transform.position,Color.red,5f);
        if(Physics.Raycast(transform.position,PlayerController.instance.transform.position-transform.position, out RaycastHit hit))
        {
            if(hit.collider.gameObject.name != "Player")
            {
                //print(hit.collider.gameObject.name);
                NudgeRock();
                
            }
            //print("ray hit player");
            if(Physics.Raycast(PlayerController.instance.transform.position, transform.position - PlayerController.instance.transform.position, out RaycastHit hit2))
            {
                if(hit2.collider.gameObject.name != gameObject.name)
                {
                    //print(hit2.collider.gameObject.name);
                    NudgeRock();
                }
            }
        }
    }

    private void NudgeRock()
    {
        transform.position += (PlayerController.instance.transform.position - transform.position).normalized * 2f;
    }

    public void Interact()
    {
        PlayerController.instance.HoldObject(gameObject);
    }

    private void Update()
    {
        if(transform.position.y < -10f)
        {
            transform.position = Camera.main.transform.position + Camera.main.transform.forward * 2;
        }
    }

}
