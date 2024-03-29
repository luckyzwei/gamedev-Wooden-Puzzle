using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using NutBolts.Scripts;
using NutBolts.Scripts.Assistant;
using NutBolts.Scripts.Data;
using NutBolts.Scripts.Item;

public class Obstacle : MonoBehaviour
{
    public Vector2 offset;
    private Dictionary<string,HingeJoint2D> hingleJoint2DList;
    private List<int> keys;
    private List<GameObject> maskCircleList;
    private bool isActive;
    public Rigidbody2D attractRigid;
    public bool ScaleX;
    public bool ScaleY;

    private float f;
    public Action<Obstacle> HitFloor;
    public List<ObstacleSide> obstacleSides;
    public ObstacleSide obstacleSide;

    private void Awake()
    {
        isActive = true;
        attractRigid.isKinematic = true;
        maskCircleList = new List<GameObject>();
        
        hingleJoint2DList = new Dictionary<string,HingeJoint2D>();
    }

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        if (collision.gameObject.CompareTag("Floor"))
        {
            HitFloor(this);
            isActive = false;
            gameObject.SetActive(false);

        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        VKSdk.VKAudioController.Instance.PlaySound(string.Format("drop{0}", UnityEngine.Random.Range(1, 3)));
       
    }
    public void Init(Side side,List<Rigidbody2D> screwRigidbody,float maxLength, int Index,Vector3 dir, float head, float tail)
    {
        obstacleSides = new List<ObstacleSide>();
        GetComponent<SpriteRenderer>().sortingOrder = Index;
        hingleJoint2DList = new Dictionary<string,HingeJoint2D>();
        keys = new List<int>();
        f = maxLength;
        SpriteRenderer sp = GetComponent<SpriteRenderer>();
        Vector2 size = sp.size;
        if(ScaleX && ScaleY)
        {
           
           sp.size = size.normalized * (maxLength) * 2f;
        }else if (ScaleX)
        {
            sp.size = new Vector2(maxLength, size.y);
            transform.localPosition += dir * (head - tail) / 4f;
            
        }else if(ScaleY)
        {
            sp.size = new Vector2(size.x, maxLength );
            transform.localPosition += dir * (head - tail) / 4f;
        }
       
        for(int i=0; i < screwRigidbody.Count; i++)
        {
            var hingleJoint = gameObject.AddComponent<HingeJoint2D>();

            var key = screwRigidbody[i].GetComponent<Screw>().GetLit().iIndex;
            hingleJoint.connectedBody = screwRigidbody[i];
            keys.Add(key);
            hingleJoint2DList.Add(key.ToString(), hingleJoint);
            var maskCircle = ItemsController.Instance.TakeItem("circle", screwRigidbody[i].transform.position+Vector3.forward,Quaternion.identity);
            maskCircle.transform.SetParent(transform, true);
            maskCircle.GetComponent<Circle>().Init(Index);
            maskCircleList.Add(maskCircle);
           
        }
        obstacleSide = new ObstacleSide();
        obstacleSide.dots = new List<int>(side.dots);
        obstacleSide.id = side.id;
        obstacleSide.eulerAngle = transform.localEulerAngles;
        obstacleSide.position = transform.localPosition;
        
        Invoke("FixedHingleJoint",0.1f);
    }
    void FixedHingleJoint()
    {
        for(int i=0; i<maskCircleList.Count; i++)
        {
            string key = keys[i].ToString();          
          
            hingleJoint2DList[key].connectedAnchor = maskCircleList[i].transform.localPosition;
            hingleJoint2DList[key].anchor = maskCircleList[i].transform.localPosition;

        }
        attractRigid.isKinematic = false;
        attractRigid.bodyType = RigidbodyType2D.Dynamic;
        UpdateObstacleSide();
    }

    public void ReleaseScrew(Screw sc) {
        UpdateObstacleSide();
        int key = -1;
        foreach(var pair in hingleJoint2DList)
        {
            if (pair.Value.connectedBody.GetComponent<Screw>().GetLit().iIndex == sc.GetLit().iIndex)
            {
                key = int.Parse(pair.Key);
                break;
            }
        }
        if (key != -1)
        {
            Destroy(hingleJoint2DList[key.ToString()]);
            hingleJoint2DList.Remove(key.ToString());
            keys.Remove(key);
        }
        attractRigid.AddTorque(1f);       
    }

    private Quaternion qua;
    private Vector3 euler;
    public void JointScrew(Screw sc,Vector3 pos)
    {
      
        var point = transform.InverseTransformPoint(sc.transform.position);
        var hingleJoint = GetHingleJoint();
        hingleJoint.breakAction = JointBreakAction2D.Ignore;
        hingleJoint.connectedBody = sc.rigidboy2D;
        hingleJoint.connectedAnchor = point;
        hingleJoint.anchor = point;
        var keyJoint = sc.GetLit().iIndex;
        keys.Add(keyJoint);
        hingleJoint2DList.Add(keyJoint.ToString(), hingleJoint);
        UpdateObstacleSide();
    }
 
   
    HingeJoint2D GetHingleJoint()
    {       
        
        return gameObject.AddComponent<HingeJoint2D>();
    }
    
    void UpdateObstacleSide()
    {
        obstacleSide.dots.Clear();
        foreach(HingeJoint2D hingeJoint in hingleJoint2DList.Values)
        {
            obstacleSide.dots.Add(hingeJoint.connectedBody.GetComponent<Screw>().GetLit().iIndex);
        }             
        obstacleSide.eulerAngle = transform.localEulerAngles;
        obstacleSide.position = transform.localPosition;
    }
    public void OnSave()
    {
        if (!isActive) return;
        UpdateObstacleSide();
        var ob = new ObstacleSide();
        ob.dots = new List<int>(obstacleSide.dots.ToArray());
        ob.id = this.obstacleSide.id;
        ob.eulerAngle = this.obstacleSide.eulerAngle;
        ob.position = this.obstacleSide.position;
        obstacleSides.Add(ob);
    }
    public void Previous()
    {
        gameObject.SetActive(true);
        if (!isActive)
        {
            isActive = true;
            gameObject.SetActive(true);
            Side side = CLevelManager.Instance.GetSideById(obstacleSide.id);
            if (side.prefabName != string.Empty)
            {
                side.dots = new List<int>(obstacleSide.dots);
                CLevelManager.Instance.gameField.AddSide(side, side.id);
            }
        }
        if (obstacleSides.Count > 0)
        {
            obstacleSide = obstacleSides[obstacleSides.Count - 1];
            obstacleSides.RemoveAt(obstacleSides.Count - 1);
           
            for(int i=0; i<hingleJoint2DList.Count; i++)
            {
                var key = keys[i];
                Destroy(hingleJoint2DList[key.ToString()]);
            }
            hingleJoint2DList.Clear();
            keys.Clear();
            transform.localEulerAngles = obstacleSide.eulerAngle;
            transform.position = obstacleSide.position;
            foreach(int d in obstacleSide.dots)
            {
                var hingleJoint = gameObject.AddComponent<HingeJoint2D>();
                var sc = CLevelManager.Instance.GetLit(d).GetScrew();
                var p= transform.InverseTransformPoint(sc.transform.position);
                hingleJoint.connectedBody = sc.rigidboy2D;
                hingleJoint.connectedAnchor= p;
                hingleJoint.anchor = p;
              
                int key = sc.GetLit().iIndex;
                keys.Add(key);
                hingleJoint2DList.Add(key.ToString(), hingleJoint);
            }
        }
    }
}
