using System.Collections;
using UnityEngine;

public class Hole : MonoBehaviour
{
    private bool _isVideo, _isCoin;
    [SerializeField] GameObject videoObj;
    [SerializeField] GameObject coinObj;
    private Lit lit;
    public bool isVideo
    {
        get { return _isVideo; }
        set
        {
            _isVideo = value;
            videoObj.SetActive(value);
            
        }
    }
    public bool isCoin
    {
        get { return _isCoin; }
        set {
            _isCoin = value;
            coinObj.SetActive(value);
          
        }
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SetLit(Lit l)
    {
        this.lit = l;
    }
    public Lit GetLit()
    {
        return lit;
    }
}