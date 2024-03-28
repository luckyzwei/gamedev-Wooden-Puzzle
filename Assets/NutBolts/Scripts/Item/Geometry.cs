using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Geometry : MonoBehaviour
{
    [SerializeField] GameObject objectPrefab;
    [SerializeField] PolygonCollider2D objectBound;

    public float radius = 1f;
    private SpriteRenderer spriteRend;
    private CompositeCollider2D compositeCollider;
    private Vector2 mPosClick = new Vector2(-1000, -1000);
    private Vector2[] mVertex;
 
    // Start is called before the first frame update
    void Start()
    {
        spriteRend = gameObject.GetComponent<SpriteRenderer>();
        compositeCollider = gameObject.GetComponent<CompositeCollider2D>();
        compositeCollider.GenerateGeometry();
        mVertex = objectBound.GetPath(0);
       // GetComponent<PolygonCollider2D>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    mPosClick = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    if (CreatePolygon(mPosClick))
        //    {
        //        Debug.Log("Create a point");

        //    }
        //    return;
        //}
    }
    private bool CreatePolygon(Vector2 point)
    {
        if (ContainsPoint(point) == false)
        {
            return false;
        }
           
        GameObject objectClick = Instantiate(objectPrefab, point, Quaternion.identity);
        objectClick.transform.parent = transform;
        compositeCollider.GenerateGeometry();
        
        return true;
    }
    bool ContainsPoint(Vector2 point)
    {
        Vector2 p = transform.InverseTransformPoint(point);
        int size = mVertex.Length;
        int j = (size - 1);
        bool result = false;
        for (int i = 0; i < size; j = i++)
        {
            var pi = mVertex[i];
            var pj = mVertex[j];
            if (((pi.y <= p.y && p.y < pj.y) || (pj.y <= p.y && p.y < pi.y)) &&
                (p.x < (pj.x - pi.x) * (p.y - pi.y) / (pj.y - pi.y) + pi.x))
                result = !result;
        }
        return result;
    }
    public void Init(List<Vector2> points)
    {
        for(int i=0; i<points.Count; i++)
        {
            CreatePolygon(points[i]);
        }
    }
}
