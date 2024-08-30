using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipZone : MonoBehaviour
{
    [Tooltip("In seconds per second")]
    public float FlashlightChargeSpeed;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            PlayerController.instance.ChargeFlashlight(FlashlightChargeSpeed * Time.deltaTime);
        }
    }
}
