using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ControlAssistant : MonoBehaviour
{
    public static ControlAssistant Instance;
    public float Err = 0.015f;
    private Screw selectedScrew;
    public ScrewState screwState = ScrewState.Normal;
    private List<int> Histories;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        if (PlayerPrefs.GetInt("OpenTipTest", 0)!=0)
        {
            Init();
        }
       
    }

    // Update is called once per frame
    void Update()
    {
       
        var point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
       
        RaycastHit2D mainhit = Physics2D.Raycast(point, Vector2.zero, 10, 1 << 9 );
        RaycastHit2D[] hitAll = Physics2D.RaycastAll(point, Vector2.zero, 10, 1 << 10);
        RaycastHit2D holeHit = Physics2D.Raycast(point, Vector2.zero, 10, 1 << 10);
        RaycastHit2D[] itemHits = Physics2D.CircleCastAll(point,0.17f, Vector2.zero, 10, 1 << 8);
        
        if (Input.GetMouseButtonDown(0))
        {
           
            if (mainhit.collider)
            {
                SelectScrew(mainhit.collider.GetComponent<Screw>());              
                return;
            }
            if(screwState==ScrewState.Normal && holeHit.collider)
            {
                if (holeHit.collider.CompareTag("Hole"))
                {
                    OnSelectHole(null, holeHit.collider.GetComponent<Hole>());
                    return;
                }             
            }
            if (screwState == ScrewState.Waiting && !mainhit.collider)
            {
                Hole h = null;
                bool result = true;
                if (hitAll.Length >= 2)
                {
                    var pos = (Vector2)hitAll[0].collider.transform.position;                    
                    foreach (RaycastHit2D hit in hitAll)
                    {
                        var posHit = (Vector2)hit.collider.transform.position;                      
                        if (Vector2.Distance(posHit, pos) > Err)
                        {
                            result = false;
                            break;
                        }
                        
                    }
                    var circleArray = hitAll.Where(H => !H.collider.CompareTag("Hole")).ToArray();
                    if (circleArray.Length != itemHits.Length)
                    {
                        result = false;
                    }
                    //foreach(RaycastHit2D hit in circleArray)
                    //{
                    //    bool check = false;
                    //    for (int j = 0; j < itemHits.Length; j++)
                    //    {                           
                    //        if (itemHits[j].collider.name == hit.collider.transform.parent.name)
                    //        {
                    //            check = true;
                    //            break;
                    //        }
                    //    }
                    //    if (!check)
                    //    {
                    //        result = false;
                    //        break;
                    //    }
                    //}
                    if (result)
                    {

                        for (int i = 0; i < hitAll.Length; i++)
                        {
                            if (hitAll[i].collider.CompareTag("Hole"))
                            {
                                h = hitAll[i].collider.GetComponent<Hole>();
                                break;
                            }
                        }
                        if (h)
                        {
                            //Debug.LogWarning("Exactly");
                            OnSelectHole(selectedScrew, h);                           
                            return;
                        }

                    }
                    else
                    {
                        VKSdk.VKAudioController.Instance.PlaySound("Wrong_match");
                        //  Debug.LogWarning("Click failed");
                        RestoreSelectScrew();
                    }
                }else
                if (hitAll.Length == 1 )
                {
                    if (hitAll[0].collider.CompareTag("Hole") && itemHits.Length == 0)
                    {
                        h = hitAll[0].collider.GetComponent<Hole>();
                        OnSelectHole(selectedScrew, h);
                    }
                }
            }
         
        }

    }
    void OnSelectHole(Screw sc, Hole h)
    {

        if (sc == null)
        {
            if (h.isVideo)
            {
                h.isVideo = false;
                return;
            }
            else if (h.isCoin)
            {
                CLevelManager.Instance.ClickCoinHole(h);
                return;
            }
            return;
        }
        if (h.isVideo)
        {
            h.isVideo = false;
            return;
        }
        else if (h.isCoin)
        {
            CLevelManager.Instance.ClickCoinHole(h);
            return;
        }
        screwState = ScrewState.Normal;
        //if (!CLevelManager.Instance.CheckIsExistTip(h.GetLit())) return;
        OnBooster(h.GetLit());
        OnSaveTip(sc, h);
        sc.OnMove(sc, h);
        PlayShake();
        screwState = ScrewState.Normal;
        selectedScrew = null;
    }
    public void SelectScrew(Screw sc)
    {
       
       
        if (selectedScrew == null)
        {
            selectedScrew = sc;
            selectedScrew.OnWait();
            screwState = ScrewState.Waiting;
            OnBooster(sc.GetLit());
        }      
        else
        {
            if (selectedScrew.GetLit().iIndex != sc.GetLit().iIndex)
            {
                selectedScrew.DeWait();
                
            }
            selectedScrew = sc;
            sc.OnWait();
            screwState = ScrewState.Waiting;
            OnBooster(sc.GetLit());
        }
       

    }
    public void RestoreSelectScrew()
    {
        if (selectedScrew)
        {
            screwState = ScrewState.Normal;
            selectedScrew.DeWait();
            selectedScrew = null;
        }
        if (CLevelManager.FLAG_TIPS)
        {
            CLevelManager.INDEX_TIPS--;
        }
    }
    public void Init()
    {
        Histories = new List<int>();
    }
    public void SetStatus(Screw sc)
    {
        screwState = ScrewState.Normal;
        selectedScrew = sc;
       
    }
   
    void OnBooster(Lit l)
    {
            
        if (CLevelManager.FLAG_TOOLS)
        {
            if (l.GetScrew())
            {
                l.GetScrew().OnRelease();
                l.GetScrew().DeActivate();
                CLevelManager.Instance.TurnOffTool();
                selectedScrew = null;
            }
        }
        if (CLevelManager.FLAG_TIPS)
        {

            CLevelManager.Instance.OnNextTip();
        }
    }
    void OnSaveTip(Screw sc, Hole h)
    {
        if (PlayerPrefs.GetInt("OpenTipTest", 0) != 0)
        {
            Histories.Add(sc.GetLit().iIndex);
            Histories.Add(h.GetLit().iIndex);

        }       
    }
    public List<int> GetHistory()
    {
        return Histories;
    }
    public void PlayShake()
    {
        if (!UserData.Instance.CSettingData.isShake) return;
#if !UNITY_EDITOR
        Handheld.Vibrate();
#endif
    }
    public void PreviouseControl()
    {
        screwState = ScrewState.Normal;
        selectedScrew = null;
    }
}
