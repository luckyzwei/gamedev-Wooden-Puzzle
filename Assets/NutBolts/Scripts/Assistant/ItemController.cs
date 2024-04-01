using System.Collections.Generic;
using System.Linq;
using NutBolts.Scripts.Data;
using NutBolts.Scripts.Item;
using UnityEngine;
using VKSdk;
using Zenject;

namespace NutBolts.Scripts.Assistant
{
    public class ItemController : MonoBehaviour
    {
        [Inject] private DataMono _dataMono;
        [Inject] private VKAudioController _vkAudioController;
        [Inject] private GameManager _gameManager;
        private float _errorTime = 0.025f;
        private Screw _selectScrew;
        public ScrewState _screwState { get; private set; } = ScrewState.Normal;
        public List<int> Histories { get; private set; }

        private void Start()
        {
            if (PlayerPrefs.GetInt("OpenTipTest", 0)!=0)
            {
                Construct();
            }
        }

        private void Update()
        {
            if (Input.mousePosition.x == Mathf.Infinity) return;
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
                    OnHoleTouch(null, holeHit.collider.GetComponent<ScrewHole>());
                    return;
                }             
            }

            if (_screwState != ScrewState.Waiting || mainhit.collider) return;
            ScrewHole h = null;
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
                            h = hitAll[i].collider.GetComponent<ScrewHole>();
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
                    _vkAudioController.PlaySound("Wrong_match");
                    PlaceBackScrew();
                }
            }
            else if (hitAll.Length == 1)
            {
                if (!hitAll[0].collider.CompareTag("Hole") || itemHits.Length != 0) return;
                    
                h = hitAll[0].collider.GetComponent<ScrewHole>();
                OnHoleTouch(_selectScrew, h);
            }
        }

        private void OnHoleTouch(Screw screw, ScrewHole height)
        {
            _screwState = ScrewState.Normal;
            UseBuster(height.Lit);
            SaveTip(screw, height);
            screw.onMove.Invoke(screw, height);
            Shake();
            _screwState = ScrewState.Normal;
            _selectScrew = null;
        }

        private void ScrewSelect(Screw screw)
        {
            if (_selectScrew == null)
            {
                _selectScrew = screw;
                _selectScrew.StartWaiting();
                _screwState = ScrewState.Waiting;
                UseBuster(screw.Lit);
            }      
            else
            {
                if (_selectScrew.Lit.iIndex != screw.Lit.iIndex)
                {
                    _selectScrew.StopWaiting();
                
                }
                _selectScrew = screw;
                screw.StartWaiting();
                _screwState = ScrewState.Waiting;
                UseBuster(screw.Lit);
            }
        }

        private void PlaceBackScrew()
        {
            if (_selectScrew)
            {
                _screwState = ScrewState.Normal;
                _selectScrew.StopWaiting();
                _selectScrew = null;
            }
            if (GameManager.flagTips)
            {
                GameManager.indexTips--;
            }
        }
        public void Construct()
        {
            Histories = new List<int>();
        }

        private void UseBuster(Fire l)
        {
            if (GameManager.flagTools)
            {
                if (l.Screw)
                {
                    l.Screw.ReleaseScre();
                    l.Screw.MakeNotActive();
                    _gameManager.DisableTool();
                    _selectScrew = null;
                }
            }
            if (GameManager.flagTips)
            {

                _gameManager.OnNextTip();
            }
        }

        private void SaveTip(Screw screw, ScrewHole height)
        {
            if (PlayerPrefs.GetInt("OpenTipTest", 0) != 0)
            {
                Histories.Add(screw.Lit.iIndex);
                Histories.Add(height.Lit.iIndex);

            }       
        }
        private void Shake()
        {
            if (!_dataMono.SettingData.isShake) return;
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
