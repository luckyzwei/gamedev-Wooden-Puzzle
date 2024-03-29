using System.Collections;
using System.Collections.Generic;
using System.IO;
using NutBolts.Scripts.Assistant;
using NutBolts.Scripts.Data;
using NutBolts.Scripts.Item;
using NutBolts.Scripts.Targets;
using NutBolts.Scripts.UI.UIGame;
using UnityEngine;
using VKSdk.Notify;
using VKSdk.UI;

namespace NutBolts.Scripts
{
    public class CLevelManager : MonoBehaviour
    {
        public delegate void GameStateEvents();
        public static event GameStateEvents OnEnterGame;
        public static event GameStateEvents OnWin;
        public static bool FLAG_TOOLS;
        public static bool FLAG_TIPS ;
        public static int INDEX_TIPS;
        public static int COLLECT;
        public static int LEVEL = 1;
        public static CLevelManager THIS;
        public static CLevelManager Instance;
        public SpriteRenderer background;
        public SpriteRenderer foreground;
        
        public LevelObject levelObject { get; private set; }
    
        public CTargetLevels cTarget;
  
        public GameField gameField;
   
        private GameObject litFolder;
        private GameObject itemFolder;

        private Dictionary<string, Lit> lits;
        private Dictionary<int,Obstacle> obstacles;
        private float litoffset = 0.66f;

    
        public GameState GameStatus { 
            get => gameStatus;
            set
            {
                gameStatus = value;
                if (value == GameState.Init)
                {
                    if (PlayerPrefs.GetInt("OpenLevelTest") <= 0)
                    {
                        Debug.LogWarning("OpenLevelTest failed");
                        VKLayerController.Instance.ShowLayer("UIMenu");
                    }
                    else
                    {
                        THIS.GameStatus = GameState.PrepareGame;
                        PlayerPrefs.SetInt("OpenLevelTest", 0);
                        PlayerPrefs.Save();
                    }
                }
                else if (value == GameState.PrepareGame)
                {
                    PrepairGame();
                }
           
            }
        }
        private GameState gameStatus;
        private void OnEnable()
        {
            OnWin += OnClose;
        }
        private void OnDisable()
        {
            OnWin -= OnClose;
        }
        void Start()
        {
            Instance = this;
            THIS = this;
            GameStatus = GameState.Init;
        }

        public void LoadLevel()
        {
       
            background.gameObject.SetActive(true);
            LEVEL = PlayerPrefs.GetInt("OpenLevel");
            if (LEVEL == 0)
            {
                LEVEL = 1;
                DataMono.Instance.CGameData.Level = 1;
                DataMono.Instance.SaveLocalData();
            }     
            LoadDataFromLocal(LEVEL);
        
        
        }

        private void PrepairGame()
        {
            Initial(levelObject);
        }

        private void LoadDataFromLocal(int currentLevel)
        {
            TextAsset mapText = Resources.Load("Level/" + currentLevel) as TextAsset;
            if (mapText == null)
            {
                mapText = Resources.Load("Level/" + currentLevel) as TextAsset;
            }
            levelObject = JsonUtility.FromJson<LevelObject>(mapText.text);
            cTarget = Resources.Load("Target/Level" + currentLevel) as CTargetLevels;
            if (cTarget == null)
            {
                cTarget = Resources.Load("Target/Level" + currentLevel) as CTargetLevels;
            }
        }

        private void Initial(LevelObject levelObject)
        {
            gameField = new GameField(levelObject);
            this.levelObject = levelObject;
            GenerateBg();
            GenerateLit();
            Invoke("GenerateItem", 0.1f);
            Invoke("CompleteLoadLevel", 0.5f);
            var uiGame =(UIGame) VKLayerController.Instance.ShowLayer("UIGame");
            int second=300;
            if (cTarget.targets.Length > 1)
            {
                second = cTarget.targets[1].amount;
            }
            uiGame.Init(second);
            OnEnterGame?.Invoke();
        }

        private void GenerateBg()
        {
            foreground.gameObject.SetActive(true);
      
            background.transform.position = Vector3.zero;
            foreground.transform.position = Vector3.zero;
        }

        private void GenerateLit()
        {
            litFolder = new GameObject();
            litFolder.name = "Lits";
            GameObject o;
            Lit s;
            Vector3 position;      
            lits = new Dictionary<string, Lit>();
            for (int y = 0; y < gameField.H; y++)
            for (int x = 0; x < gameField.W; x++)
            {
                if (gameField.s[y, x] >= 1)
                {
                    position = new Vector3();
                    position.x = -litoffset * (0.5f * (gameField.W - 1) - x);
                    position.y = -litoffset * (-0.5f * (gameField.H - 1) + y);
                    o = ItemsController.Instance.TakeItem("LitEmpty", position);
                    o.name = "Lit_" + y + "x" + x;
                    o.transform.parent = litFolder.transform;
                    s = o.GetComponent<Lit>();
                    s.iIndex = y * gameField.W + x;
                    
                    
                    
                    if (gameField.s[y, x] <= 4 )
                    {
                        Hole h = GetNewHole(position, s.iIndex);
                        h.isCoin = (gameField.s[y, x] == 4);
                        h.isVideo = (gameField.s[y, x] == 3);
                        h.transform.SetParent(s.transform, true);
                        s.SetHole(h);
                        h.SetLit(s);

                        Screw sc = GetNewScrew(position, s.iIndex);
                        sc.transform.SetParent(s.transform, true);
                        s.SetScrew(sc);
                        sc.OnMove += OnMoveToLit;
                        sc.SetLit(s);
                        if (gameField.s[y, x] == 1)
                        {
                            sc.Activate();
                        }
                        else
                        {
                            sc.DeActivate();
                        }
                    }
                    
                    lits.Add(y + "x" + x, s);
                }
            }
        }

        private void GenerateItem()
        {
            itemFolder = new GameObject("Items");
            if (obstacles == null)
            {
                obstacles = new Dictionary<int, Obstacle>();
            }
            obstacles.Clear();
            for(int i=0; i<gameField.sides.Count; i++)
            {
                var pos = Vector2.zero;
                Vector2 start;
                Vector2 second;
                Vector2 end;
                float maxC = -10000f;
                int C = gameField.sides[i].dots.Count;
                float degreeRot = (float)gameField.sides[i].rotBlockDegree;
                start = GetLit(gameField.sides[i].dots[0]).transform.position;
                if (C < 2)
                {
                    second = start;
                }
                else
                {
                    second = GetLit(gameField.sides[i].dots[1]).transform.position;
                }          
            
                Vector3 dir = (second - start);
                float angle = Vector2.Angle(Vector2.up,dir);
                Vector3 cross = Vector3.Cross(Vector3.up, (Vector3)dir).normalized;
           
                Obstacle o = ItemsController.Instance.TakeItem<Obstacle>(gameField.sides[i].prefabName);
                o.name = "Item_" + i.ToString();
                o.transform.SetParent(itemFolder.transform, true);
                bool oneDirect = (!o.ScaleX || !o.ScaleY) && (o.ScaleY || o.ScaleX);
                if (oneDirect)
                {
                    o.transform.eulerAngles = cross * angle;
                }
                else
                {
                    o.transform.localEulerAngles = new Vector3(0, 0, gameField.sides[i].rotBlockDegree);
                }
                end = start;
                List<Rigidbody2D> rigids = new List<Rigidbody2D>();
                for (int j = 0; j < C; j++)
                {

                    var s = GetLit(gameField.sides[i].dots[j]);
                    pos += (Vector2)s.transform.position;
                    if (s.GetScrew())
                    {
                        rigids.Add(s.GetScrew().rigidboy2D);
                        s.GetScrew().SetJoints(o);
                    }
                    if (Vector2.Distance(start, (Vector2)s.transform.position)>maxC)
                    {
                        end = (Vector2)s.transform.position;
                        maxC = Vector2.Distance(start, end);
                  
                    }
               
                }

                var head = Mathf.Max(gameField.sides[i].Head, 1);
                var tail = Mathf.Max(gameField.sides[i].Tail, 1);
                pos = pos / ((float)(gameField.sides[i].dots.Count))+o.offset;         
                o.transform.position = pos;
            
                float length = (end-start).magnitude + tail /2+head/2;
                o.Init(gameField.sides[i],rigids,length,i,dir.normalized,head,tail);
                o.HitFloor += OnBarHitFloor;
           
            
                obstacles.Add(i+1, o);           
            }
        }

        private void OnMoveToLit(Screw sc,Hole h)
        {
            string move=string.Format("{0}_{1}:{2}",gameField.Histories.Count, sc.GetLit().iIndex, h.GetLit().iIndex);
            gameField.UpdateHistories(move);
            foreach(var obstacle in obstacles.Values)       
            {
                obstacle.OnSave();
            }

            LeanTween.move(sc.animator.gameObject, h.transform.position, 0.3f).setOnComplete(() =>
            {
                h.GetLit().GetScrew().Reactivate();
                sc.OnRelease();
                sc.DeActivate();
            });
        }

        private Lit GetLit(int y, int x)
        {
            if (lits.ContainsKey(y + "x" + x))
            {
                return lits[y + "x" + x];
            }
            return null;
        }
        public Lit GetLit(int iIndex)
        {
            int x = iIndex % gameField.W;
            int y = iIndex / gameField.W;
            return GetLit(y, x);
        }

        private Screw GetNewScrew( Vector3 pos, int id)
        {
            GameObject o = ItemsController.Instance.TakeItem("screw");
            o.transform.position = pos;
            o.name = "Screw_"+id;
            Screw screw = o.GetComponent<Screw>();
        
            return screw;
        }

        private Hole GetNewHole(Vector3 pos, int id)
        {
            GameObject o = ItemsController.Instance.TakeItem("hole");
            o.transform.position = pos;
            o.name = "Hole_" + id;
            Hole h= o.GetComponent<Hole>();
            return h;
        }

        private void RemovePopulation()
        {
            _collects.Clear();
            foreground.gameObject.SetActive(false);
            if (litFolder) Destroy(litFolder);
            if (itemFolder) Destroy(itemFolder);
            FLAG_TIPS = false;
            FLAG_TOOLS = false;
            INDEX_TIPS = 0;
            COLLECT = 0;
        
        }
        void CompleteLoadLevel()
        {
            VKLayerController.Instance.HideLoading();
        
        }
        public void NextLevelCompleted()
        {
            VKLayerController.Instance.HideLoading();
        }
        public void Reset()
        {
            foreground.gameObject.SetActive(false);
            RemovePopulation();
        }

        private int _dem = 0;

        private void GoHome()
        {
            var uiMenu = VKLayerController.Instance.GetLayer("UIMenu");
            OnClose();
        }

        private void OnClose()
        {
            var uiGame = (UIGame)VKLayerController.Instance.GetLayer("UIGame");
            uiGame.Close();
        }

        private readonly List<string> _collects = new ();

        private void OnBarHitFloor(Obstacle obstacle)
        {
            if (_collects.Contains(obstacle.name)) return;
            _collects.Add(obstacle.name);
            COLLECT++;
            if (COLLECT >= cTarget.targets[0].amount)
            {
                if (PlayerPrefs.GetInt("OpenTipTest") != 0)
                {
                    PlayerPrefs.SetInt("OpenTipTest", 0);
                    SaveTip();
                    ItemController.Instance.Construct();
                }
                if (OnWin != null)
                {
                    OnWin();
                    DataMono.Instance.CGameData.Level++;
                    DataMono.Instance.SaveLocalData();
                }
           
            }
            VKSdk.VKAudioController.Instance.PlaySound("Drop");
            StartCoroutine(WaitForRemove(obstacle));
        }
        IEnumerator WaitForRemove(Obstacle obstacle)
        {
            yield return new WaitUntil(() => ItemController.Instance._screwState == ScrewState.Waiting);
            gameField.RemoveSide(obstacle.obstacleSide.id);
        }
        public void OnPreviouseMove()
        {
            if (gameField.moves.Count == 0)
            {
                gameField = new GameField(levelObject);
                VKNotifyController.Instance.AddNotify("Previous is empty",
                    VKNotifyController.TypeNotify.Error);
                return;
            }
            string move = gameField.moves[gameField.moves.Count - 1];
       
            string[] tmp = move.Split('_');
            string[] Arr = tmp[1].Split(':');
            int begin = int.Parse(Arr[0]);
            int end = int.Parse(Arr[1]);
            gameField.UpdateBoard(begin, CSquare.Nut);
            gameField.UpdateBoard(end, CSquare.Hole);
            GetLit(begin).GetScrew().Activate();
            GetLit(end).GetScrew().DeActivate();
            var listofsides = gameField.Histories[move];
            foreach(var pair in obstacles)
            {
                if (gameField.CheckIsExistSide(move, pair.Key))
                {
                    pair.Value.Previous();
                }
            }
            COLLECT = cTarget.targets[0].amount - FindObjectsOfType<Obstacle>().Length;
            ItemController.Instance.Control();
            gameField.moves.RemoveAt(gameField.moves.Count - 1);
            gameField.Histories.Remove(move);
        
        }
        public void NextLevel()
        {
            Reset();
            LEVEL++;
            PlayerPrefs.SetInt("OpenLevel", LEVEL);
            LoadLevel();
            GameStatus = GameState.PrepareGame;
            VKLayerController.Instance.ShowLoading();
            Invoke("NextLevelCompleted",0.3f);
        }
        public void OnPlayTip()
        {
            RemovePopulation();
            LoadLevel();
            VKLayerController.Instance.ShowLoading();
            GameStatus = GameState.PrepareGame;
            Invoke("ReturnTip", 0.5f);
        }
        public void OnBoosterReset()
        {
            OnRetry();
        }
        public void OnRetry()
        {
            var uiGame = VKLayerController.Instance.GetLayer("UIGame");
            uiGame.Close();
            RemovePopulation();
            LoadLevel();
            VKLayerController.Instance.ShowLoading();
            GameStatus = GameState.PrepareGame;
            Invoke("CompleteLoadLevel", 0.5f);
        }
        public void OnTool()
        {
            FLAG_TOOLS = true;
            foreach(var lit in lits.Values)
            {
                if (lit.GetScrew())
                {
                    lit.GetScrew().TurnOnToolLight();
                }
            }
        }
        public void TurnOffTool()
        {
            FLAG_TOOLS = false;
            foreach (var lit in lits.Values)
            {
                if (lit.GetScrew())
                {
                    lit.GetScrew().TurnOffToolLight();
                }
            }
        }
        void ReturnTip()
        {      
            FLAG_TIPS = true;
            INDEX_TIPS = 0;
            Lit lit = GetLit(levelObject.tipPaths[INDEX_TIPS]);
            lit.TurnOn();
        }
        public void OnNextTip()
        {       
            foreach(var l in lits.Values)
            {
                l.TurnOff();
            }
            INDEX_TIPS++;
            if (INDEX_TIPS < levelObject.tipPaths.Count)
            {
                var lit = GetLit(levelObject.tipPaths[INDEX_TIPS]);
                if (lit)
                {
                    lit.TurnOn();
                }
            }
        }

        private void SaveTip()
        {
            int length = levelObject.tipPaths.Count;
            List<int> History = ItemController.Instance.Histories;
            if (History.Count < length || length==0)
            {
                levelObject.tipPaths = new List<int>(History);
                string jsonStr = JsonUtility.ToJson(levelObject);
                if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsEditor)
                {
                    //Write to file
                    string activeDir = Application.dataPath + @"/NutBolts/Resources/Level/";
                    string newPath = System.IO.Path.Combine(activeDir, PlayerPrefs.GetInt("OpenLevel") + ".txt");
                    StreamWriter sw = new StreamWriter(newPath);
                    sw.Write(jsonStr);
                    sw.Close();
                }
           
            }
        }
        public bool CheckIsExistTip(Lit l)
        {
            if (levelObject == null) return false;
            if (levelObject.tipPaths == null) return false;
            if (levelObject.tipPaths.Count == 0) return false;
            if (!levelObject.tipPaths.Contains(l.iIndex))
            {
                VKNotifyController.Instance.AddNotify("You did not follow tips which breaks the path to win the level",
                    VKNotifyController.TypeNotify.Error);
                return false;
            }
            if (levelObject.tipPaths.IndexOf(l.iIndex) != CLevelManager.INDEX_TIPS)
            {
                VKNotifyController.Instance.AddNotify("You did not follow tips which breaks the path to win the level",
                    VKNotifyController.TypeNotify.Error);
                return false;
            }
            return true;
        }
        public void ClickCoinHole(Hole h)
        {
            if (DataMono.Instance.CGameData.Coins >= 100)
            {
                DataMono.Instance.CGameData.Coins -= 100;
                DataMono.Instance.SaveLocalData();
                h.isCoin = false;
                VKNotifyController.Instance.AddNotify("-100 coin.", VKNotifyController.TypeNotify.Normal);
           
            }
            else
            {
                VKNotifyController.Instance.AddNotify("Not enough coin.", VKNotifyController.TypeNotify.Error);
            }
        }
        public Side GetSideById(int id)
        {
            foreach(var side in levelObject.sides)
            {
                if (side.id == id)
                {
                    return side;
                }
            }
            return new Side();
        }
    
    }
    public class GameField
    {
        public int W;
        public int H;
        public int[,] s;
        public List<Side> sides;
        public Dictionary<string,int[]> Histories;
        public List<string> moves;
        public GameField(LevelObject levelObj)
        {
            W = levelObj.maxCol;
            H = levelObj.maxRow;
            s = new int[H, W];
            for(int y=0; y<H; y++)
            for(int x=0; x<W; x++)
            {
                s[y, x] = levelObj.nuts[y * W + x];
            }
            sides = new List<Side>();
            for(int i=0; i<levelObj.sides.Count; i++)
            {
                levelObj.sides[i].id = i + 1;
                sides.Add(levelObj.sides[i]);         
            }

            Histories = new Dictionary<string, int[]>();
            moves = new List<string>();
        }
        public void RemoveSide(int iIndex)
        {
            for(int i=0; i<sides.Count; i++)
            {
                if (sides[i].id == iIndex)
                {
                    sides.RemoveAt(i);
                    break;
                }
            }
        }
        public void AddSide(Side side ,int iIndex)
        {
            for (int i = 0; i < sides.Count; i++)
            {
                if (sides[i].id == iIndex)
                {
                    return;
                }
            }
            sides.Add(side);
        }
        public void UpdateHistories(string newMove)
        {
            moves.Add(newMove);
            var arr = new int[sides.Count];
            for(int i=0; i<sides.Count; i++)
            {
                arr[i] = sides[i].id;
            }
            Histories.Add(newMove, arr);
        }
        public void UpdateBoard(int iIndex, CSquare cSquare)
        {
            int x = iIndex % this.W;
            int y = iIndex / this.W;
            s[y, x] = (int)cSquare;
        }

        public bool CheckIsExistSide(string id, int iIndex)
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