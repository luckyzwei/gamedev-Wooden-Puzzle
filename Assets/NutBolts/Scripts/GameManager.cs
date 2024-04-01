using System.Collections;
using System.Collections.Generic;
using System.IO;
using NutBolts.Scripts.Assistant;
using NutBolts.Scripts.Data;
using NutBolts.Scripts.Item;
using NutBolts.Scripts.Targets;
using NutBolts.Scripts.UI.UIGame;
using UnityEngine;
using VKSdk;
using VKSdk.Notify;
using VKSdk.UI;
using Zenject;

namespace NutBolts.Scripts
{
    public class GameManager : MonoBehaviour
    {
        [Inject] private VKAudioController _vkAudioController;
        [Inject] private VKNotifyController _vkNotifyController;
        [Inject] private VKLayerController _vkLayerController;
        [Inject] private ItemsController _itemsController;
        [Inject] private ItemController _itemController;
        [Inject] private DataMono _dataMono;
        public delegate void GameStateEvents();
        public static event GameStateEvents OnEnterGame;
        public static event GameStateEvents OnWin;
        public static bool flagTools;
        public static bool flagTips ;
        public static int indexTips;
        public static int collect;
        public static int level = 1;
        [SerializeField] private SpriteRenderer _bgSprite;
        [SerializeField] private SpriteRenderer _ground;
        [SerializeField] private CLevels _targetLevel;
        public LevelObject LevelObject { get; private set; }
        public GameBoard Field { get; private set; }
        
        private GameObject _litPrefab;
        private GameObject _itemPrefab;
        private Dictionary<string, Fire> _lits;
        private Dictionary<int,Blocks> _obstacles;
        private float _litOffset = 0.66f;

    
        public GameState Status { 
            get => _gameStatus;
            set
            {
                _gameStatus = value;
                switch (value)
                {
                    case GameState.Init when PlayerPrefs.GetInt("OpenLevelTest") <= 0:
                        Debug.LogWarning("OpenLevelTest failed");
                        _vkLayerController.ShowLayer("UIMenu");
                        break;
                    case GameState.Init:
                        Status = GameState.PrepareGame;
                        PlayerPrefs.SetInt("OpenLevelTest", 0);
                        PlayerPrefs.Save();
                        break;
                    case GameState.PrepareGame:
                        PrepairGame();
                        break;
                }
            }
        }
        private GameState _gameStatus;
        private void OnEnable()
        {
            OnWin += OnClose;
        }
        private void OnDisable()
        {
            OnWin -= OnClose;
        }

        private void Start()
        {
            Status = GameState.Init;
        }

        public void ConstructLevel()
        {
            _bgSprite.gameObject.SetActive(true);
            level = PlayerPrefs.GetInt("OpenLevel");
            if (level == 0)
            {
                level = 1;
                _dataMono.Data.Level = 1;
                _dataMono.SaveAll();
            }     
            LoadDataFromLocal(level);
        }

        private void PrepairGame()
        {
            Debug.Log(LevelObject);
            Initial(LevelObject);
            
        }

        private void LoadDataFromLocal(int currentLevel)
        {
            TextAsset mapText = Resources.Load("Level/" + currentLevel) as TextAsset;
            if (mapText == null)
            {
                mapText = Resources.Load("Level/" + currentLevel) as TextAsset;
            }
            LevelObject = JsonUtility.FromJson<LevelObject>(mapText.text);
            _targetLevel = Resources.Load("Target/Level" + currentLevel) as CLevels;
            if (_targetLevel == null)
            {
                _targetLevel = Resources.Load("Target/Level" + currentLevel) as CLevels;
            }
        }

        private void Initial(LevelObject levelObject)
        {
            Field = new GameBoard(levelObject);
            this.LevelObject = levelObject;
            GenerateBg();
            GenerateLit();
            Invoke("GenerateItem", 0.1f);
            Invoke("LoadLevelCompleted", 0.5f);
            var uiGame =(UIGameMenu) _vkLayerController.ShowLayer("UIGame");
            int second=300;
            if (_targetLevel.targets.Length > 1)
            {
                second = _targetLevel.targets[1].amount;
            }
            uiGame.Construct(second);
            OnEnterGame?.Invoke();
        }

        private void GenerateBg()
        {
            _ground.gameObject.SetActive(true);
      
            _bgSprite.transform.position = Vector3.zero;
            _ground.transform.position = Vector3.zero;
        }

        private void GenerateLit()
        {
            _litPrefab = new GameObject();
            _litPrefab.name = "Lits";
            GameObject o;
            Fire s;
            Vector3 position;      
            _lits = new Dictionary<string, Fire>();
            for (int y = 0; y < Field.Height; y++)
            for (int x = 0; x < Field.Width; x++)
            {
                if (Field.boadMatrix[y, x] >= 1)
                {
                    position = new Vector3();
                    position.x = -_litOffset * (0.5f * (Field.Width - 1) - x);
                    position.y = -_litOffset * (-0.5f * (Field.Height - 1) + y);
                    o = _itemsController.TakeItem("LitEmpty", position);
                    o.name = "Lit_" + y + "x" + x;
                    o.transform.parent = _litPrefab.transform;
                    s = o.GetComponent<Fire>();
                    s.iIndex = y * Field.Width + x;
                    
                    
                    
                    if (Field.boadMatrix[y, x] <= 4 )
                    {
                        ScrewHole h = SpawnHole(position, s.iIndex);
                        h.transform.SetParent(s.transform, true);
                        h.Lit = s;

                        Screw sc = ScrewTake(position, s.iIndex);
                        sc.transform.SetParent(s.transform, true);
                        s.Screw = sc;
                        sc.onMove += OnMoveToLit;
                        sc.Lit = s;
                        if (Field.boadMatrix[y, x] == 1)
                        {
                            sc.Activate();
                        }
                        else
                        {
                            sc.MakeNotActive();
                        }
                    }
                    
                    _lits.Add(y + "x" + x, s);
                }
            }
        }

        private void GenerateItem()
        {
            _itemPrefab = new GameObject("Items");
            if (_obstacles == null)
            {
                _obstacles = new Dictionary<int, Blocks>();
            }
            _obstacles.Clear();
            for(int i=0; i<Field.Sides.Count; i++)
            {
                var pos = Vector2.zero;
                Vector2 start;
                Vector2 second;
                Vector2 end;
                float maxC = -10000f;
                int C = Field.Sides[i].dots.Count;
                float degreeRot = (float)Field.Sides[i].rotBlockDegree;
                start = FindLit(Field.Sides[i].dots[0]).transform.position;
                if (C < 2)
                {
                    second = start;
                }
                else
                {
                    second = FindLit(Field.Sides[i].dots[1]).transform.position;
                }          
            
                Vector3 dir = (second - start);
                float angle = Vector2.Angle(Vector2.up,dir);
                Vector3 cross = Vector3.Cross(Vector3.up, (Vector3)dir).normalized;
           
                Blocks o = _itemsController.TakeItem<Blocks>(Field.Sides[i].prefabName);
                o.name = "Item_" + i.ToString();
                o.transform.SetParent(_itemPrefab.transform, true);
                bool oneDirect = (!o._xScale || !o._yScale) && (o._yScale || o._xScale);
                if (oneDirect)
                {
                    o.transform.eulerAngles = cross * angle;
                }
                else
                {
                    o.transform.localEulerAngles = new Vector3(0, 0, Field.Sides[i].rotBlockDegree);
                }
                end = start;
                List<Rigidbody2D> rigids = new List<Rigidbody2D>();
                for (int j = 0; j < C; j++)
                {

                    var s = FindLit(Field.Sides[i].dots[j]);
                    pos += (Vector2)s.transform.position;
                    if (s.Screw)
                    {
                        rigids.Add(s.Screw.rigidboy2D);
                        s.Screw.AssignJoints(o);
                    }
                    if (Vector2.Distance(start, (Vector2)s.transform.position)>maxC)
                    {
                        end = (Vector2)s.transform.position;
                        maxC = Vector2.Distance(start, end);
                  
                    }
               
                }

                var head = Mathf.Max(Field.Sides[i].Head, 1);
                var tail = Mathf.Max(Field.Sides[i].Tail, 1);
                pos = pos / ((float)(Field.Sides[i].dots.Count))+o.Offset;         
                o.transform.position = pos;
            
                float length = (end-start).magnitude + tail /2+head/2;
                o.Construct(Field.Sides[i], rigids, length, i, dir.normalized, head, tail);
                o.OnFloorHit += BarHit;
           
            
                _obstacles.Add(i+1, o);           
            }
        }

        private void OnMoveToLit(Screw sc,ScrewHole h)
        {
            string move=string.Format("{0}_{1}:{2}",Field.Histories.Count, sc.Lit.iIndex, h.Lit.iIndex);
            Field.HistoryNew(move);
            foreach(var obstacle in _obstacles.Values)       
            {
                obstacle.Save();
            }

            LeanTween.move(sc.animator.gameObject, h.transform.position, 0.3f).setOnComplete(() =>
            {
                h.Lit.Screw.Reincornate();
                sc.ReleaseScre();
                sc.MakeNotActive();
            });
        }

        private Fire FindLit(int y, int x)
        {
            if (_lits.ContainsKey(y + "x" + x))
            {
                return _lits[y + "x" + x];
            }
            return null;
        }
        public Fire FindLit(int iIndex)
        {
            int x = iIndex % Field.Width;
            int y = iIndex / Field.Width;
            return FindLit(y, x);
        }

        private Screw ScrewTake( Vector3 pos, int id)
        {
            GameObject o = _itemsController.TakeItem("screw");
            o.transform.position = pos;
            o.name = "Screw_"+id;
            Screw screw = o.GetComponent<Screw>();
        
            return screw;
        }

        private ScrewHole SpawnHole(Vector3 pos, int id)
        {
            GameObject o = _itemsController.TakeItem("hole");
            o.transform.position = pos;
            o.name = "Hole_" + id;
            ScrewHole h= o.GetComponent<ScrewHole>();
            return h;
        }

        private void RemovePeoplePopulation()
        {
            _collects.Clear();
            _ground.gameObject.SetActive(false);
            if (_litPrefab) Destroy(_litPrefab);
            if (_itemPrefab) Destroy(_itemPrefab);
            flagTips = false;
            flagTools = false;
            indexTips = 0;
            collect = 0;
        
        }

        private void LoadLevelCompleted()
        {
            _vkLayerController.HideLoading();
        }
        public void NextLevelCompleted()
        {
            _vkLayerController.HideLoading();
        }
        public void Reset()
        {
            _ground.gameObject.SetActive(false);
            RemovePeoplePopulation();
        }

        private int _dem = 0;

        private void GoHome()
        {
            var uiMenu = _vkLayerController.GetLayer("UIMenu");
            OnClose();
        }

        private void OnClose()
        {
            var uiGame = (UIGameMenu)_vkLayerController.GetLayer("UIGame");
            uiGame.Close();
        }

        private readonly List<string> _collects = new ();

        private void BarHit(Blocks obstacle)
        {
            if (_collects.Contains(obstacle.name)) return;
            _collects.Add(obstacle.name);
            collect++;
            if (collect >= _targetLevel.targets[0].amount)
            {
                if (PlayerPrefs.GetInt("OpenTipTest") != 0)
                {
                    PlayerPrefs.SetInt("OpenTipTest", 0);
                    TipStore();
                    _itemController.Construct();
                }
                if (OnWin != null)
                {
                    OnWin();
                    _dataMono.Data.Level++;
                    _dataMono.SaveAll();
                }
           
            }
            _vkAudioController.PlaySound("Drop");
            StartCoroutine(RemoveTimer(obstacle));
        }

        private IEnumerator RemoveTimer(Blocks obstacle)
        {
            yield return new WaitUntil(() => _itemController._screwState == ScrewState.Waiting);
            Field.RemoveSide(obstacle.ObstacleSide.id);
        }
        public void OnMove()
        {
            if (Field.Moves.Count == 0)
            {
                Field = new GameBoard(LevelObject);
                _vkNotifyController.AddNotify("Previous is empty",
                    VKNotifyController.TypeNotify.Error);
                return;
            }
            string move = Field.Moves[Field.Moves.Count - 1];
       
            string[] tmp = move.Split('_');
            string[] Arr = tmp[1].Split(':');
            int begin = int.Parse(Arr[0]);
            int end = int.Parse(Arr[1]);
            Field.BoadNew(begin, CSquare.Nut);
            Field.BoadNew(end, CSquare.Hole);
            FindLit(begin).Screw.Activate();
            FindLit(end).Screw.MakeNotActive();
            foreach(var pair in _obstacles)
            {
                if (Field.SideCheck(move, pair.Key))
                {
                    pair.Value.PositionBefore();
                }
            }
            collect = _targetLevel.targets[0].amount - FindObjectsOfType<Blocks>().Length;
            _itemController.Control();
            Field.Moves.RemoveAt(Field.Moves.Count - 1);
            Field.Histories.Remove(move);
        
        }
        public void LoadNextLevel()
        {
            Reset();
            level++;
            PlayerPrefs.SetInt("OpenLevel", level);
            ConstructLevel();
            Status = GameState.PrepareGame;
            _vkLayerController.ShowLoading();
            Invoke("NextLevelCompleted",0.3f);
        }
        public void PlayTip()
        {
            RemovePeoplePopulation();
            ConstructLevel();
            _vkLayerController.ShowLoading();
            Status = GameState.PrepareGame;
            Invoke("TipRevert", 0.5f);
        }
        public void ResetAllBusters()
        {
            OnReplayLevel();
        }
        public void OnReplayLevel()
        {
            var uiGame = _vkLayerController.GetLayer("UIGame");
            uiGame.Close();
            RemovePeoplePopulation();
            ConstructLevel();
            _vkLayerController.ShowLoading();
            Status = GameState.PrepareGame;
            Invoke("LoadLevelCompleted", 0.5f);
        }
        public void OnTool()
        {
            flagTools = true;
            foreach(var lit in _lits.Values)
            {
                if (lit.Screw)
                {
                    lit.Screw.Light = true;
                }
            }
        }
        public void DisableTool()
        {
            flagTools = false;
            foreach (var lit in _lits.Values)
            {
                if (lit.Screw)
                {
                    lit.Screw.Light = false;
                }
            }
        }

        private void TipRevert()
        {      
            flagTips = true;
            indexTips = 0;
            Fire lit = FindLit(LevelObject.tipPaths[indexTips]);
            lit.IsActive = true;
        }
        public void OnNextTip()
        {       
            foreach(var l in _lits.Values)
            {
                l.IsActive = false;
            }
            indexTips++;
            if (indexTips < LevelObject.tipPaths.Count)
            {
                var lit = FindLit(LevelObject.tipPaths[indexTips]);
                if (lit)
                {
                    lit.IsActive = true;
                }
            }
        }

        private void TipStore()
        {
            int length = LevelObject.tipPaths.Count;
            List<int> History = _itemController.Histories;
            if (History.Count < length || length==0)
            {
                LevelObject.tipPaths = new List<int>(History);
                string jsonStr = JsonUtility.ToJson(LevelObject);
                if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsEditor)
                {
                    string activeDir = Application.dataPath + @"/NutBolts/Resources/Level/";
                    string newPath = System.IO.Path.Combine(activeDir, PlayerPrefs.GetInt("OpenLevel") + ".txt");
                    StreamWriter sw = new StreamWriter(newPath);
                    sw.Write(jsonStr);
                    sw.Close();
                }
           
            }
        }
    
        public Side GetSideById(int id)
        {
            foreach(var side in LevelObject.sides)
            {
                if (side.id == id)
                {
                    return side;
                }
            }
            return new Side();
        }
    
    }
    public class GameBoard
    {
        public int Width { get;}
        public int Height { get; }
        public int[,] boadMatrix  { get; }
        public List<Side> Sides { get; }
        public Dictionary<string,int[]> Histories { get; }
        public List<string> Moves { get; }
        public GameBoard(LevelObject levelObj)
        {
            Width = levelObj.maxCol;
            Height = levelObj.maxRow;
            boadMatrix = new int[Height, Width];
            for(int y=0; y<Height; y++)
            for(int x=0; x<Width; x++)
            {
                boadMatrix[y, x] = levelObj.nuts[y * Width + x];
            }
            Sides = new List<Side>();
            for(int i=0; i<levelObj.sides.Count; i++)
            {
                levelObj.sides[i].id = i + 1;
                Sides.Add(levelObj.sides[i]);         
            }

            Histories = new Dictionary<string, int[]>();
            Moves = new List<string>();
        }
        public void RemoveSide(int iIndex)
        {
            for(int i=0; i<Sides.Count; i++)
            {
                if (Sides[i].id == iIndex)
                {
                    Sides.RemoveAt(i);
                    break;
                }
            }
        }
        public void GenerateSide(Side side ,int iIndex)
        {
            foreach (var sie in Sides)
            {
                if (sie.id == iIndex)
                {
                    return;
                }
            }

            Sides.Add(side);
        }
        public void HistoryNew(string newMove)
        {
            Moves.Add(newMove);
            var arr = new int[Sides.Count];
            for(int i=0; i<Sides.Count; i++)
            {
                arr[i] = Sides[i].id;
            }
            Histories.Add(newMove, arr);
        }
        public void BoadNew(int iIndex, CSquare cSquare)
        {
            int x = iIndex % this.Width;
            int y = iIndex / this.Width;
            boadMatrix[y, x] = (int)cSquare;
        }

        public bool SideCheck(string id, int iIndex)
        {
            foreach(int element in (int[])Histories[id])
            {
                if (element == iIndex) return true;
            }
            return false;
        }   
    }
    public enum GameState
    {
        Init,
        PrepareGame,
    }
}