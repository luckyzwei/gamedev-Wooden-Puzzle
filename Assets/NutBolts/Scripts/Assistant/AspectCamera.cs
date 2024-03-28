using System.Collections;
using System.Linq;
using UnityEngine;
// [ExecuteInEditMode]
//2.2.2
public class AspectCamera : MonoBehaviour
{  
    private Rect fieldRect;
    float fieldHeight;
    private float fielgWidth;
    private float fraq;
    public float topY = 10;
    public float bottomY = -10;
    public float leftX = -6;
    public float rightX = 6;
    private void OnEnable()
    {
        CLevelManager.OnHome += UpdateAspect;
        CLevelManager.OnEnterGame += UpdateAspect;
    }

    private void OnDisable()
    {
        CLevelManager.OnHome -= UpdateAspect;
        CLevelManager.OnEnterGame -= UpdateAspect;
    }
    private void Update()
    {
       
    }
    void UpdateAspect()
    {
        StartCoroutine(Wait());

    }

    IEnumerator Wait()
    {
        yield return new WaitWhile(() => !CLevelManager.THIS);
        if (CLevelManager.THIS.GameStatus == GameState.PrepareGame)
        {
            fieldHeight = topY - bottomY ;
            fielgWidth = rightX - leftX;
            fieldRect = new Rect(leftX, topY, fielgWidth, fieldHeight);
            fraq = (fielgWidth > fieldHeight ? fielgWidth : fieldHeight);
            float width = (float)Screen.width;
            float height = (float)Screen.height;
            var h = 0.5f * fieldRect.width * height / width;
            var w = (fieldRect.height) / 2;
            var maxLength = Mathf.Max(h, w)+StaticData.HEIGHT_BANNER/200f;
            Camera.main.orthographicSize = Mathf.Clamp(h, maxLength, maxLength);

        }
        else
            Camera.main.orthographicSize = 6.5f / Screen.width * Screen.height / 2f;

        transform.position = Vector3.down * StaticData.HEIGHT_BANNER / 200f;
    }

}