using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCamera : MonoBehaviour
{
    Transform Player => PlayerController.Instance== null ? null : PlayerController.Instance.HostBase.transform;
    // Update is called once per frame
    private void OnValidate()
    {
        if (Player != null)
        {
            gameObject.transform.position = new Vector3(Player.position.x, transform.position.y, Player.transform.position.z);
        }
    }
    void Update()
    {
        if (Player == null) return;
        gameObject.transform.position=new Vector3(Player.position.x,transform.position.y,Player.transform.position.z);
    }
}
