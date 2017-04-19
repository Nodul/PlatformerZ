using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    public GameObject target;
    private Vector3 targetPos;
    public float ZPosition = -10;

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update ()
    {
        targetPos = new Vector3(
            target.transform.position.x,
            target.transform.position.y,
            target.transform.position.z);

        this.transform.position = new Vector3(
            targetPos.x,
            targetPos.y,
            this.transform.position.z);
	}
}
