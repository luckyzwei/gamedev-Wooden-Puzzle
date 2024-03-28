using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using VKSdk.Notify;
using VKSdk.UI;
public class CLevelManager : MonoBehaviour
{
    public delegate void GameStateEvents();

    public static event GameStateEvents OnEnterGame;
    public static event GameStateEvents OnWin;
    
    public static event GameStateEvents OnHome;
    public static bool FLAG_TOOLS = false;
    public static bool FLAG_TIPS = false;
    public static int INDEX_TIPS = 0;
    public static int COLLECT = 0;
    public static int LEVEL = 1;
    public static CLevelManager THIS;
    public static CLevelManager Instance;
    public SpriteRenderer background;
    public SpriteRenderer foreground;
    [HideInInspector]
    public LevelObject levelObject;
    public CTargetLevels cTarget;
  
    public GameField gameField;
   
    private GameObject litFolder;
    private GameObject itemFolder;
    [HideInInspector]
    public Dictionary<string, Lit> lits;
    [HideInInspector]
    public Dictionary<int,Obstacle> obstacles;
    public float litoffset = 0.7f;

    
    public GameState GameStatus { 
        get {
            return gameStatus; 
        }
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
                    CLevelManager.THIS.GameStatus = GameState.PrepareGame;
                    PlayerPrefs.SetInt("OpenLevelTest", 0);
                    PlayerPrefs.Save();
                }
            }else if (value == GameState.PrepareGame)
            {
                PrepairGame();
            }
           
        }
    }
    private GameState gameStatus;
    public bool LevelLoaded = false;
    private void OnEnable()
    {
        CLevelManager.OnHome += GoHome;     
        CLevelManager.OnWin += OnClose;
    }
    private void OnDisable()
    {
        CLevelManager.OnHome -= GoHome;
        CLevelManager.OnWin -= OnClose;
    }
    void Start()
    {
        Instance = this;
        THIS = this;
        GameStatus = GameState.Init;
        // LoadLevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void LoadLevel()
    {
       
        background.gameObject.SetActive(true);
        CLevelManager.LEVEL = PlayerPrefs.GetInt("OpenLevel");
        if (CLevelManager.LEVEL == 0)
        {
            CLevelManager.LEVEL = 1;
            UserData.Instance.CGameData.CurrentLevel = 1;
            UserData.Instance.SaveLocalData();
        }     
        LoadDataFromLocal(CLevelManager.LEVEL);
        
        
    }  
    public void PrepairGame()
    {
        Initial(levelObject);
    }
    public void LoadDataFromLocal(int currentLevel)
    {
        LevelLoaded = false;
        //Read data from text file
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

        LevelLoaded = true;
    }
    public void Initial(LevelObject levelObject)
    {
        gameField = new GameField(levelObject);
        this.levelObject = levelObject;
        GenerateBG();
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
        OnEnterGame();
        
    }
    void GenerateBG()
    {
        //litoffset = StaticData.SIZE_BG * litoffset / (float)(gameField.W);
        foreground.gameObject.SetActive(true);
      
        background.transform.position = Vector3.zero;
        foreground.transform.position = Vector3.zero;
        //Vector2 size = background.size.normalized * (gameField.W*2);
       
        //background.size = size * (litoffset)+size.normalized*3f;
        //foreground.size = size * (litoffset)+size.normalized*1.5f;
    }
    void GenerateLit()
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
                    o = ContainAssistant.Instance.GetItem("LitEmpty", position);
                    o.name = "Lit_" + y + "x" + x;
                    o.transform.parent = litFolder.transform;
                    s = o.GetComponent<Lit>();
                    s.x = x;
                    s.y = y;                   
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
    void GenerateItem()
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
           
            Obstacle o = ContainAssistant.Instance.GetItem<Obstacle>(gameField.sides[i].prefabName);
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
    
    public void OnMoveToLit(Screw sc,Hole h)
    {
        string move=string.Format("{0}_{1}:{2}",gameField.Histories.Count, sc.GetLit().iIndex, h.GetLit().iIndex);
        gameField.UpdateHistories(move);
        foreach(var obstacle in obstacles.Values)       
        {
                obstacle.OnSave();
        }
        //h.GetLit().GetScrew().Reactivate();
        //sc.OnRelease();
        //sc.DeActivate();
        //ControlAssistant.Instance.SetStatus(h.GetLit().GetScrew());

        //sc.OnRelease();
        LeanTween.move(sc.animator.gameObject, h.transform.position, 0.3f).setOnComplete(() =>
        {
            h.GetLit().GetScrew().Reactivate();
            sc.OnRelease();
            sc.DeActivate();
            //ControlAssistant.Instance.SetStatus(h.GetLit().GetScrew());
            //sc.DeActivate();
            //h.GetLit().GetScrew().Reactivate();
            //ControlAssistant.Instance.SetStatus(h.GetLit().GetScrew());
        });
    }
  
    public Lit GetLit(int y, int x)
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
    public Screw GetNewScrew( Vector3 pos, int id)
    {
        GameObject o = ContainAssistant.Instance.GetItem("screw");
        o.transform.position = pos;
        o.name = "Screw_"+id;
        Screw screw = o.GetComponent<Screw>();
        
        return screw;
    }
    public Hole GetNewHole(Vector3 pos, int id)
    {
        GameObject o = ContainAssistant.Instance.GetItem("hole");
        o.transform.position = pos;
        o.name = "Hole_" + id;
        Hole h= o.GetComponent<Hole>();
        return h;
    }
    
    public void RemovePopulation()
    {
        collects.Clear();
        foreground.gameObject.SetActive(false);
        if (litFolder) Destroy(litFolder);
        if (itemFolder) Destroy(itemFolder);
        CLevelManager.FLAG_TIPS = false;
        CLevelManager.FLAG_TOOLS = false;
        CLevelManager.INDEX_TIPS = 0;
        CLevelManager.COLLECT = 0;
        
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
    public void Clear()
    {
        Reset();
        VKLayerController.Instance.ShowLayer("UIMenu");
    }
    int dem = 0;

    public void GoHome()
    {
        var uiMenu = VKLayerController.Instance.GetLayer("UIMenu");
        OnClose();
    }
    public void OnClose()
    {
        var uiGame = (UIGame)VKLayerController.Instance.GetLayer("UIGame");
        uiGame.Close();
    }
    List<string> collects = new List<string>();
    public void OnBarHitFloor(Obstacle obstacle)
    {
        if (collects.Contains(obstacle.name)) return;
        collects.Add(obstacle.name);
        CLevelManager.COLLECT++;
        if (CLevelManager.COLLECT >= cTarget.targets[0].amount)
        {
            if (PlayerPrefs.GetInt("OpenTipTest") != 0)
            {
                PlayerPrefs.SetInt("OpenTipTest", 0);
                SaveTip();
                ControlAssistant.Instance.Init();
            }
            if (OnWin != null)
            {
                OnWin();
                UserData.Instance.CGameData.CurrentLevel++;
                UserData.Instance.SaveLocalData();
            }
           
        }
        VKSdk.VKAudioController.Instance.PlaySound("Drop");
        StartCoroutine(WaitForRemove(obstacle));
    }
    IEnumerator WaitForRemove(Obstacle obstacle)
    {
        yield return new WaitUntil(() => ControlAssistant.Instance.screwState == ScrewState.Waiting);
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
        CLevelManager.COLLECT = cTarget.targets[0].amount - FindObjectsOfType<Obstacle>().Length;
        ControlAssistant.Instance.PreviouseControl();
        gameField.moves.RemoveAt(gameField.moves.Count - 1);
        gameField.Histories.Remove(move);
        
    }
    public void NextLevel()
    {
        Reset();
        CLevelManager.LEVEL++;
        PlayerPrefs.SetInt("OpenLevel",CLevelManager.LEVEL);
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
        CLevelManager.FLAG_TOOLS = true;
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
        CLevelManager.FLAG_TOOLS = false;
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
        CLevelManager.FLAG_TIPS = true;
        CLevelManager.INDEX_TIPS = 0;
        Lit lit = GetLit(levelObject.tipPaths[CLevelManager.INDEX_TIPS]);
        lit.TurnOn();
    }
    public void OnNextTip()
    {       
        foreach(var l in lits.Values)
        {
            l.TurnOff();
        }
        CLevelManager.INDEX_TIPS++;
        if (CLevelManager.INDEX_TIPS < levelObject.tipPaths.Count)
        {
            var lit = GetLit(levelObject.tipPaths[CLevelManager.INDEX_TIPS]);
            if (lit)
            {
                lit.TurnOn();
            }
        }
    }
    void SaveTip()
    {
        int length = levelObject.tipPaths.Count;
        List<int> History = ControlAssistant.Instance.GetHistory();
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
        if (UserData.Instance.CGameData.TotalCoin >= 100)
        {
            UserData.Instance.CGameData.TotalCoin -= 100;
            UserData.Instance.SaveLocalData();
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
        this.W = levelObj.maxCol;
        this.H = levelObj.maxRow;
        this.s = new int[H, W];
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
       // System.Array.Copy(levelObj.sides.ToArray(), this.sides, levelObj.sides.Count);
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
    public void AddSide(ObstacleSide obstacleSide)
    {
        for(int i=0; i<sides.Count; i++)
        {
            if (sides[i].id == obstacleSide.id) return;
        }
        Side side = new Side();
        side.id = obstacleSide.id;
        side.dots = new List<int>(obstacleSide.dots);
        sides.Add(side);
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
    Playing,
    Win,
    GameOver,
    Pause,
    Home
}

