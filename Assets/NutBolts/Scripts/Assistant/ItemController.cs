using System.Collections.Generic;
using System.Linq;
using NutBolts.Scripts.Data;
using NutBolts.Scripts.Item;
using UnityEngine;

namespace NutBolts.Scripts.Assistant
{
    public class ItemController : MonoBehaviour
    {
        public static ItemController Instance;
        private float _errorTime = 0.025f;
        private Screw _selectScrew;
        public ScrewState _screwState { get; private set; } = ScrewState.Normal;
        public List<int> Histories { get; private set; }

        private void Start()
        {
            Instance = this;
            if (PlayerPrefs.GetInt("OpenTipTest", 0)!=0)
            {
                Construct();
            }
        }

        private void Update()
        {
            var point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
       
            RaycastHit2D mainhit = Physics2D.Raycast(point, Vector2.zero, 10, 1 << 9 );
            RaycastHit2D[] hitAll = Physics2D.RaycastAll(point, Vector2.zero, 10, 1 << 10);
            RaycastHit2D holeHit = Physics2D.Raycast(point, Vector2.zero, 10, 1 << 10);
            RaycastHit2D[] itemHits = Physics2D.CircleCastAll(point,0.17f, Vector2.zero, 10, 1 << 8);

            if (!Input.GetMouseButtonDown(0)) return;
            if (mainhit.collider)
            {
                ScrewSelect(mainhit.collider.GetComponent<Screw>());              
                return;
            }
            if(_screwState==ScrewState.Normal && holeHit.collider)
            {
                if (holeHit.collider.CompareTag("Hole"))
                {
                    OnHoleTouch(null, holeHit.collider.GetComponent<Hole>());
                    return;
                }             
            }

            if (_screwState != ScrewState.Waiting || mainhit.collider) return;
            Hole h = null;
            bool result = true;
            if (hitAll.Length >= 2)
            {
                var pos = (Vector2)hitAll[0].collider.transform.position;                    
                foreach (RaycastHit2D hit in hitAll)
                {
                    var posHit = (Vector2)hit.collider.transform.position;                      
                    if (Vector2.Distance(posHit, pos) > _errorTime)
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
                        OnHoleTouch(_selectScrew, h);
                    }

                }
                else
                {
                    VKSdk.VKAudioController.Instance.PlaySound("Wrong_match");
                    PlaceBackScrew();
                }
            }
            else if (hitAll.Length == 1)
            {
                if (!hitAll[0].collider.CompareTag("Hole") || itemHits.Length != 0) return;
                    
                h = hitAll[0].collider.GetComponent<Hole>();
                OnHoleTouch(_selectScrew, h);
            }
        }

        private void OnHoleTouch(Screw screw, Hole height)
        {
            if (screw == null)
            {
                if (height.isVideo)
                {
                    height.isVideo = false;
                    return;
                }

                if (!height.isCoin) return;
                CLevelManager.Instance.ClickCoinHole(height);
                return;
            }
            if (height.isVideo)
            {
                height.isVideo = false;
                return;
            }

            if (height.isCoin)
            {
                CLevelManager.Instance.ClickCoinHole(height);
                return;
            }
            _screwState = ScrewState.Normal;
            UseBuster(height.GetLit());
            SaveTip(screw, height);
            screw.OnMove(screw, height);
            Shake();
            _screwState = ScrewState.Normal;
            _selectScrew = null;
        }

        private void ScrewSelect(Screw screw)
        {
            if (_selectScrew == null)
            {
                _selectScrew = screw;
                _selectScrew.OnWait();
                _screwState = ScrewState.Waiting;
                UseBuster(screw.GetLit());
            }      
            else
            {
                if (_selectScrew.GetLit().iIndex != screw.GetLit().iIndex)
                {
                    _selectScrew.DeWait();
                
                }
                _selectScrew = screw;
                screw.OnWait();
                _screwState = ScrewState.Waiting;
                UseBuster(screw.GetLit());
            }
       

        }

        private void PlaceBackScrew()
        {
            if (_selectScrew)
            {
                _screwState = ScrewState.Normal;
                _selectScrew.DeWait();
                _selectScrew = null;
            }
            if (CLevelManager.FLAG_TIPS)
            {
                CLevelManager.INDEX_TIPS--;
            }
        }
        public void Construct()
        {
            Histories = new List<int>();
        }

        private void UseBuster(Lit l)
        {
            if (CLevelManager.FLAG_TOOLS)
            {
                if (l.GetScrew())
                {
                    l.GetScrew().OnRelease();
                    l.GetScrew().DeActivate();
                    CLevelManager.Instance.TurnOffTool();
                    _selectScrew = null;
                }
            }
            if (CLevelManager.FLAG_TIPS)
            {

                CLevelManager.Instance.OnNextTip();
            }
        }

        private void SaveTip(Screw screw, Hole height)
        {
            if (PlayerPrefs.GetInt("OpenTipTest", 0) != 0)
            {
                Histories.Add(screw.GetLit().iIndex);
                Histories.Add(height.GetLit().iIndex);

            }       
        }
        private void Shake()
        {
            if (!DataMono.Instance.CSettingData.isShake) return;
#if !UNITY_EDITOR
        Handheld.Vibrate();
#endif
        }
        public void Control()
        {
            _screwState = ScrewState.Normal;
            _selectScrew = null;
        }
    }
}
