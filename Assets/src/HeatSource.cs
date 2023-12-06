using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatSource : MonoBehaviour {
    public static HeatSource instance;

    void Awake() {
        instance = this;
    }

    public void setScale(float scale) {
        transform.localScale = new Vector3(scale, scale, scale);
    }

    public void setX(float x) {
        transform.position = new Vector3(x, transform.position.y, transform.position.z); 
    }

    public void setY(float y) {
        transform.position = new Vector3(transform.position.x, y, transform.position.z); 
    }

    public void setZ(float z) {
        transform.position = new Vector3(transform.position.x, transform.position.y, z); 
    }

    public void Hide() {
        GetComponent<MeshRenderer>().enabled = false;
    }

    public void Show() {
        GetComponent<MeshRenderer>().enabled = true;
    }
}
