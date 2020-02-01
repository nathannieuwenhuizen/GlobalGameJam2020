using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FancyCam : MonoBehaviour
{
    public GameObject[] targets;
    float MinFOV = 10;
    float MaxFOV = 100;

    float distanceFOVMultip = 1;

    Camera cam;
    private void Start() {
        cam = GetComponent<Camera>();
    }

    void Update() {
        if (targets != null) {
            transform.LookAt(AveragePosition(targets));
        }

        cam.fieldOfView = MinFOV + MaxDistance(targets) * distanceFOVMultip;
        if (cam.fieldOfView > MaxFOV) {
            cam.fieldOfView = MaxFOV;
        }
    }

    Vector3 AveragePosition(GameObject[] targets) {
        Vector3 avrg = Vector3.zero;
        for (int i = 0; i < targets.Length; i++) {
            avrg += targets[i].transform.position;
        }
        return avrg / targets.Length;
    }

    float MaxDistance(GameObject[] targets) {
        float dist = 0;
        for (int i = 0; i < targets.Length; i++) {
            for (int x = 0; x < targets.Length; x++) {
                float d = Vector3.Distance(targets[i].transform.position , targets[x].transform.position);
                if (d > dist) {
                    dist = d;
                }
            }
        }
        return dist;
    }
}