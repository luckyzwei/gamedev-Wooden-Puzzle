using System.Collections;
using UnityEngine;

public class Lit : MonoBehaviour
{
    public int x, y,iIndex;
    public GameObject lightObject;
    private Screw screw;
    private Hole hole;
    // Use this for initialization
    void Start()
    {
        TurnOff();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SetScrew(Screw sc)
    {
        this.screw = sc;
    }
    public void SetHole(Hole h)
    {
        this.hole = h;
    }
    public Screw GetScrew()
    {
        return screw;
    }
    public Hole GetHole()
    {
        return hole;
    }
    public void TurnOn()
    {
        lightObject.SetActive(true);
    }
    public void TurnOff()
    {
        lightObject.SetActive(false);
    }
}