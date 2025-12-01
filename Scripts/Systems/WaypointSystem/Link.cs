using System;
using UnityEngine;

public class Link : MonoBehaviour
{
    [SerializeField] private Waypoint start;
    [SerializeField] private Waypoint end;
    [SerializeField] private GameObject streetLight;
    [SerializeField] private GameObject house;
    [SerializeField] private Material streetLightsOn;
    [SerializeField] private Material streetLightsOff;
    [SerializeField] private Material houseLightsOn;
    [SerializeField] private Material houseLightsOff;

    public Waypoint Start => start;
    public Waypoint End => end;

    public void Reset()
    {
        start.Reset();
        end.Reset();
        var streetMat = streetLight.GetComponent<MeshRenderer>().materials;
        streetMat[2] = streetLightsOff;
        streetLight.GetComponent<MeshRenderer>().materials = streetMat;

        var houseMat = house.GetComponent<MeshRenderer>().materials;
        houseMat[2] = houseLightsOff;
        houseMat[3] = houseLightsOff;
        house.GetComponent<MeshRenderer>().materials = houseMat;
    }

    public void Ignite()
    {
        start.Ignite();
        end.Ignite();
        var streetMat = streetLight.GetComponent<MeshRenderer>().materials;
        streetMat[2] = streetLightsOn;
        streetLight.GetComponent<MeshRenderer>().materials = streetMat;

        var houseMat = house.GetComponent<MeshRenderer>().materials;
        houseMat[2] = houseLightsOn;
        houseMat[3] = houseLightsOn;
        house.GetComponent<MeshRenderer>().materials = houseMat;
    }

    public void IgniteStart()
    {
        start.Ignite();
        end.Reset();
        var streetMat = streetLight.GetComponent<MeshRenderer>().materials;
        streetMat[2] = streetLightsOn;
        streetLight.GetComponent<MeshRenderer>().materials = streetMat;

        var houseMat = house.GetComponent<MeshRenderer>().materials;
        houseMat[2] = houseLightsOff;
        houseMat[3] = houseLightsOff;
        house.GetComponent<MeshRenderer>().materials = houseMat;
    }
}