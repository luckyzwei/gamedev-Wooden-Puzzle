using System;
using System.Collections.Generic;
using System.IO;
using NutBolts.Scripts.Assistant;
using NutBolts.Scripts.Data;
using NutBolts.Scripts.Targets;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace NutBolts.Scripts.Editor
{
    public class CLevelMakerEditor : EditorWindow
    {
        private int levelNumber = 1;
        private string[] toolbarStrings =  { "Editor", "Helps", };
        private static int selected;

        Vector2 scrollViewVector;
        private static CLevelMakerEditor window;
        private string fileName = "1.txt";
        private static CSquare[] levelSquares = new CSquare[156];
  
        private static Vector2[] Centers = new Vector2[81];
        private Texture coinHoleTex;
        private Texture screwTex;
        private Texture holeTex;
        private Texture tText;
        private Texture videoHoleTex;
        private Texture tickTex;
        private Texture[] blockTextures;
        private GUIContent[] blockContents;
        private int rotBlock;
        private bool isVideoHole;
        private bool isCoinHole;
        private float tail = 1f;
        private float head = 1f;
        private int selSideInt;
        private string[] sideContents;
        private EditState editState = EditState.None;
        private Side currentSide;
        private static Color[] colors;
        private int maxRows = 6;
        private int maxCols = 6;
        private LevelObject levelObject;
        private bool update = false;
        private bool targetEdit = false;
        [MenuItem("NutAndBolt/Game editor")]
        public static void Init()
        {
            window = (CLevelMakerEditor)EditorWindow.GetWindow(typeof(CLevelMakerEditor));
            window.Show();
        }
        private void OnGUI()
        {
            GUI.changed = false;

            if (levelNumber < 1)
                levelNumber = 1;
            GUILayout.Space(20);
            GUILayout.BeginHorizontal();
            GUILayout.Space(30);
            int oldSelected = selected;
            selected = GUILayout.Toolbar(selected, toolbarStrings, new GUILayoutOption[] { GUILayout.Width(450) });
            GUILayout.EndHorizontal();

            scrollViewVector = GUI.BeginScrollView(new Rect(25, 45, position.width - 30, position.height), scrollViewVector, new Rect(0, 0, 400, 1600));
            GUILayout.Space(-30);

            if (oldSelected != selected)
                scrollViewVector = Vector2.zero;

            if (selected == 0)
            {
                if (EditorSceneManager.GetActiveScene().name == "game")
                {
                    GUILevelSelector();
                    GUILayout.Space(10);

                    GUILevelSize();
                    GUILayout.Space(10);

                    GUILimit();
                    GUILayout.Space(10);


                    GUILayout.Space(10);

                    GUIStars();
                    GUILayout.Space(10);

                    GUITarget();
                    GUILayout.Space(10);

                    GUIBlocks();
                    GUILayout.Space(20);

                    GUIGameField();
                    GUILayout.Space(20);

                    GUIReward();
                }
                else
                    GUIShowWarning();
            }
            else if (selected == 1)
            {
                GUIHelps();
            }

            GUI.EndScrollView();
            if (GUI.changed && !EditorApplication.isPlaying)
                EditorSceneManager.MarkAllScenesDirty();
        }
        GUIStyle style;
        private void OnEnable()
        {
            Debug.Log("On Enable");
        }
        private void OnFocus()
        {
            Debug.Log("OnFocus");
            GUIStyle style = new GUIStyle();

            style.normal.textColor = Color.white;
            style.onActive.textColor = Color.green;
            colors = new Color[100];
            for (int i = 0; i < 100; i++)
            {
                float r = 0.01f * (float)UnityEngine.Random.Range(0, 100);
                float g = 0.01f * (float)UnityEngine.Random.Range(0, 100);
                float b = 0.01f * (float)UnityEngine.Random.Range(0, 100);
                colors[i] = new Color(r, g, b);
            }

            if (maxRows <= 0)
                maxRows = 10;
            if (maxCols <= 0)
                maxCols = 10;
            tText = (Texture)Resources.Load("Item/t");
            screwTex = (Texture)Resources.Load("Item/screw");
            holeTex = (Texture)Resources.Load("Item/hole");
            tickTex = (Texture)Resources.Load("Item/tick");
            videoHoleTex = (Texture)Resources.Load("Item/videohole");
            coinHoleTex = (Texture)Resources.Load("Item/coinhole");
            Initialize();
            update = targetEdit;
            if (EditorSceneManager.GetActiveScene().name == "game")
            {

                var clm = FindObjectOfType<ItemsController>();
                blockTextures = new Texture[clm._mainItem.Count];
                blockContents = new GUIContent[clm._mainItem.Count];
                for (int i = 0; i < blockTextures.Length; i++)
                {
                    blockTextures[i] = clm._mainItem[i]._itemPrefab.GetComponent<SpriteRenderer>().sprite.texture;
                }
                for (int i = 0; i < blockTextures.Length; i++)
                {
                    blockContents[i] = new GUIContent(blockTextures[i].name, blockTextures[i], "");
                }

            }


        }
        
        float rotAngle = 10f;
        Vector2 pivotPoint;

        private void GUILevelSelector()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Level editor", EditorStyles.boldLabel, new GUILayoutOption[] { GUILayout.Width(150) });
            if (GUILayout.Button("Test level", new GUILayoutOption[] { GUILayout.Width(150) }))
            {
                if (update == false)
                {
                    Debug.LogError("Dont update");
                
                }
                if (levelObject == null)
                {
                    Debug.LogError("Error levelObject");
                    return;
                }
                if (levelObject.sides.Count == 0)
                {
                    Debug.LogError("Error levelObject");
                    return;
                }
                TestLevel();
                OpenTarget();
            }
            if (GUILayout.Button("Save level", new GUILayoutOption[] { GUILayout.Width(150) }))
            {
                if (update == false)
                {
                    Debug.LogError("Dont update");
                    return;
                }
                if (levelObject == null)
                {
                    Debug.LogError("Error levelObject");
                    return;
                }
                if (levelObject.sides.Count == 0)
                {
                    Debug.LogError("Error levelObject");
                    return;
                }
                SaveLevel();
                OpenTarget();
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Space(30);
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Level:", EditorStyles.boldLabel, new GUILayoutOption[] { GUILayout.Width(50) });
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Space(30);
            if (GUILayout.Button("<<", new GUILayoutOption[] { GUILayout.Width(50) }))
            {
                PreviousLevel();
                OpenTarget();
                update = true;
            }
            string changeLvl = GUILayout.TextField(" " + levelNumber, GUILayout.Width(50));
            if (int.Parse(changeLvl) != levelNumber)
            {
                if (LoadDataFromLocal(int.Parse(changeLvl)))
                {
                    levelNumber = int.Parse(changeLvl);
                    OpenTarget();
                }

            }

            if (GUILayout.Button(">>", GUILayout.Width(50)))
            {
                NextLevel();
                OpenTarget();
                update = true;
            }

            if (GUILayout.Button("New level", GUILayout.Width(100)))
            {
                AddLevel();
            }


            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Space(60);

            GUILayout.Label("Assets/NutBolts/Resouces/Level/", EditorStyles.label, new GUILayoutOption[] { GUILayout.Width(200) });
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            GUILayout.EndHorizontal();
        }



        private void GUILevelSize()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(60);
            GUILayout.BeginVertical();

            int oldValue = maxRows + maxCols;
            maxRows = EditorGUILayout.IntField("Rows", maxRows, new GUILayoutOption[] {
                GUILayout.Width (50),
                GUILayout.MaxWidth (200)
            });
            maxCols = EditorGUILayout.IntField("Columns", maxCols, new GUILayoutOption[] {
                GUILayout.Width (50),
                GUILayout.MaxWidth (200)
            });
            if (maxRows < 3)
                maxRows = 3;
            if (maxCols < 3)
                maxCols = 3;
            if (maxRows > 14)
                maxRows = 14;
            if (maxCols > 14)
                maxCols = 14;
            if (oldValue != maxRows + maxCols)
            {
                levelObject = null;
          
                currentSide = new Side();
                Initialize();
            }
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }

        private void GUILimit()
        {

        }



        private void GUIStars()
        {

        }
        private void GUITarget()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(30);
            GUILayout.BeginVertical();
            GUILayout.Label("Target:", EditorStyles.boldLabel);
            GUILayout.BeginHorizontal();
            GUILayout.Space(30);
            GUILayout.BeginVertical();


            if (GUILayout.Button("Target editor"))
            {
                targetEdit = true;
                OpenTarget();
            }

            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }

        private int selBlockInt;
        private void GUIBlocks()
        {
            if (update)
            {
                GUI.DrawTexture(new Rect(Screen.width / 2, Screen.height / 2, 50, 50), tickTex);
            }
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("None Dot", GUILayout.Width(100)))
            {
                editState = EditState.None;
                update = true;
            }
            if (GUILayout.Button("Add Hole", GUILayout.Width(100)))
            {
                isVideoHole = false;
                isCoinHole = false;
                editState = EditState.AvailableAddHole;
                update = true;
            }
            if (GUILayout.Button("Video Hole", GUILayout.Width(100)))
            {
                editState = EditState.AvailableAddHole;
                isVideoHole = true;
                isCoinHole = false;
                update = true;
            }
            if (GUILayout.Button("Coin Hole", GUILayout.Width(100)))
            {
                editState = EditState.AvailableAddHole;
                isCoinHole = true;
                isVideoHole = false;
                update = true;
            }
            if (GUILayout.Button("New Side", GUILayout.Width(100)))
            {
                currentSide = new Side();
                int iIndex = -1;
                int max = -1000;
                for(int i=0; i<levelObject.sides.Count; i++)
                {
                    if (levelObject.sides[i].id > max)
                    {
                        max = levelObject.sides[i].id;
                        iIndex = i;
                    }
                }
                if (iIndex < 0)
                {
                    max = levelObject.sides.Count;
                }
                currentSide.id = max+1;
                editState = EditState.AvailableAddSide;
                currentSide.dots = new List<int>();

            }
            if ((editState == EditState.BlockAddSide || editState == EditState.AvailableAddSide) && currentSide.dots != null)
                if (GUILayout.Button("Select Prefab", GUILayout.Width(200)))
                {
                    SetBlock();                          
                }


            if (GUILayout.Button("Save Side", GUILayout.Width(100)))
            {

                if (currentSide.dots == null)
                {
                    Debug.LogError("you must add screw to side");
                    return;
                }
                if (currentSide.prefabName == string.Empty)
                {
                    Debug.LogError("you must add block of side");
                }
                if (currentSide.dots.Count == 0)
                {
                    Debug.LogError("you must add screw to side");
                    return;
                }

                else
                {
                    currentSide.Head = head;
                    currentSide.Tail = tail;
                    levelObject.sides.Add(currentSide);
                }

                Initialize();
                SaveLevel();
                update = true;
            }
            if (levelObject != null)
                if (GUILayout.Button("Clear Tip", GUILayout.Width(150)))
                {
                    levelObject.tipPaths.Clear();
                    update = true;
                }


            GUILayout.EndHorizontal();

            GUILayout.BeginVertical();

            selBlockInt = GUILayout.SelectionGrid(selBlockInt, blockContents, 6, GUILayout.Height(50));

            if (levelObject != null)
            {
                sideContents = System.Array.ConvertAll(levelObject.sides.ToArray(), s => "_ " + s.id + "  X");
            }

            if (sideContents.Length >= 1)
            {
                int r = sideContents.Length / 8;
                r = Mathf.Max(1, r);
                int c = sideContents.Length;
                c = Mathf.Min(c, 8);
                for (int iIndex = 0; iIndex < r; iIndex++)
                {
                    GUILayout.BeginHorizontal();
                    for (int jIndex = 0; jIndex < c; jIndex++)
                    {
                        int k = jIndex + 8 * iIndex;
                        GUI.color = colors[k];
                        if (GUILayout.Button( sideContents[k], GUILayout.Width(100)))
                        {
                            var side = levelObject.sides[k];
                            levelObject.sides.RemoveAt(k);                     
                        }
                    }

                    GUILayout.EndHorizontal();
                }

            }

            GUILayout.EndVertical();
            GUI.color = Color.white;
            rotBlock = EditorGUILayout.IntField("Degree Rot", rotBlock, new GUILayoutOption[] {
                GUILayout.Width (50),
                GUILayout.MaxWidth (200)
            });
            if (levelObject != null)
            {
                GUILayout.Label("Target: " + levelObject.sides.Count, new GUILayoutOption[] {
                    GUILayout.Width (50),
                    GUILayout.MaxWidth (200)
                });
            }
            GUILayout.BeginHorizontal();
            GUILayout.Label("Length of Head:", GUILayout.Width(150));
            head = EditorGUILayout.Slider(head, 1, 10, new GUILayoutOption[] {
                GUILayout.Width (50),
                GUILayout.MaxWidth (200) });
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Length of Tail:", GUILayout.Width(150));
            tail = EditorGUILayout.Slider(tail, 1, 10, new GUILayoutOption[] {
                GUILayout.Width (50),
                GUILayout.MaxWidth (200) });
            GUILayout.EndHorizontal();
        }


        private void GUIGameField()
        {

            GUILayout.BeginVertical();

            if (levelObject != null)
            {

                string str = "";
                for (int i = 0; i < levelObject.tipPaths.Count; i++)
                    str += levelObject.tipPaths[i] + ",";
                GUILayout.Label("List Hint :" + str, new GUILayoutOption[] {
                    GUILayout.Width (500),
                    GUILayout.MaxWidth (600)
                });

            }



            for (int row = 0; row < maxRows; row++)
            {
                GUILayout.BeginHorizontal();
                for (int col = 0; col < maxCols; col++)
                {
                    Color squareColor = new Color(0.8f, 0.8f, 0.8f);

                    var imageButton = new object();
                    imageButton = "X";
                    if (levelSquares[row * maxCols + col] == CSquare.Nut)
                    {
                        squareColor = Color.white;
                        imageButton = screwTex;
                    }
                    else if (levelSquares[row * maxCols + col] == CSquare.Hole)
                    {
                        imageButton = holeTex;
                        squareColor = new Color(0.8f, 0.8f, 0.8f);
                    }
                    else if (levelSquares[row * maxCols + col] == CSquare.HoleVideo)
                    {
                        imageButton = videoHoleTex;
                        squareColor = new Color(0.8f, 0.8f, 0.8f);
                    }
                    else if (levelSquares[row * maxCols + col] == CSquare.HoleCoin)
                    {
                        imageButton = coinHoleTex;
                        squareColor = new Color(0.8f, 0.8f, 0.8f);
                    }
                

                    GUI.color = squareColor;

                    if (GUILayout.Button(imageButton as Texture, new GUILayoutOption[] {
                            GUILayout.Width (25),
                            GUILayout.Height (25)
                        }))
                    {
                        if (editState == EditState.AvailableAddSide)
                        {
                            SetType(col, row);
                        }                  
                        else if (editState == EditState.AvailableAddHole)
                        {
                            SetHole(col, row, isVideoHole, isCoinHole);
                        }
                        else if (editState == EditState.None)
                        {
                            levelSquares[row * maxCols + col] = CSquare.None;
                        }

                    }
                
                
                    if(row * maxCols + col < Centers.Length)
                    {
                        var lastRect = GUILayoutUtility.GetLastRect();
                        Centers[row * maxCols + col] = lastRect.center;
                    }
               
                    GUILayout.Space(5f);
                }
            
                GUILayout.EndHorizontal();
                GUILayout.Space(5);
            }

            GUILayout.EndVertical();
            if (levelObject.sides != null)
            {
                if (levelObject.sides.Count >= 1)
                {
                    for (int i = 0; i < levelObject.sides.Count; i++)
                    {
                        GUI.color = colors[i];
                        int iIndex = 0;
                   
                        if (levelObject.sides[i].dots.Count >= 2)
                        {
                            while (iIndex < levelObject.sides[i].dots.Count - 1)
                            {
                                if (levelObject.sides[i].dots[iIndex+1] >= Centers.Length) break;
                                if (levelObject.sides[i].dots[iIndex] >= Centers.Length) break;
                                Vector2 begin = Centers[levelObject.sides[i].dots[iIndex]];
                                Vector2 end = Centers[levelObject.sides[i].dots[iIndex+1]];                           
                                Vector2 dir = end - begin;
                                float size = dir.magnitude+(levelObject.sides[i].Head+levelObject.sides[i].Tail)*(50+5)/2f;
                                Vector2 pos = begin.x < end.x ? begin : end;
                                Vector2 connect = begin.x < end.x ? end : begin;
                                float angle = Vector2.SignedAngle( Vector2.right,connect-pos);
                                Vector2 d = (connect - pos).normalized;
                                pos = pos - d*(levelObject.sides[i].Head + levelObject.sides[i].Tail) * (50+5)/4f;
                                pos = pos - d * (levelObject.sides[i].Head - levelObject.sides[i].Tail) * (50 + 5) / 4f;
                                GUIUtility.RotateAroundPivot(angle, pos);

                                GUI.DrawTexture(new Rect(pos, new Vector2(size, 5)), tText);
                                // GUI.color=Color.white;
                                GUI.Label(new Rect(pos, new Vector2(50, 50)), levelObject.sides[i].id.ToString());
                                GUI.matrix = Matrix4x4.identity;
                                iIndex++;
                           
                            }
                        }

                    }
                }

            }
            GUI.color = Color.white;
            GUI.matrix = Matrix4x4.identity;



        }
        private void GUIReward()
        {
            GUILayout.BeginVertical();
            GUILayout.Label("------List Reward:");

            if (levelObject != null)
            {

                for (int i = 0; i < levelObject.rewards.Count; i++)
                {
                    GUILayout.BeginHorizontal();
                    levelObject.rewards[i].rewardType = (RewardType)EditorGUILayout.EnumFlagsField("Reward Type:", levelObject.rewards[i].rewardType, new GUILayoutOption[] {
                        GUILayout.Width (50),
                        GUILayout.MaxWidth (200)
                    });
                    GUILayout.Space(10);
                    levelObject.rewards[i].amount = EditorGUILayout.IntField("Number:", levelObject.rewards[i].amount, new GUILayoutOption[] {
                        GUILayout.Width (50),
                        GUILayout.MaxWidth (200)
                    });
                    GUILayout.EndHorizontal();
                }

                if (GUILayout.Button("+", GUILayout.Width(50)))
                {
                    levelObject.rewards.Add(new RewardObj { rewardType = RewardType.Coin, amount = 50 });
                    update = true;
                }
                if (levelObject.rewards.Count > 0)
                {
                    if (GUILayout.Button("-", GUILayout.Width(50)))
                    {
                        levelObject.rewards.RemoveAt(levelObject.rewards.Count - 1);
                        update = true;
                    }
                }
            }
            GUILayout.EndVertical();

        }
        private void GUIHelps()
        {

            GUILayout.Label("---------------------------Please Check Map Editor only active when you are open scene game------------------");
            GUILayout.Label("---------------------------Please Go Our Website See More Detail!--------------- ");
            GUILayout.Label("---------------------------Thank you so much!--------------- ");

            if (GUILayout.Button("Go Blue Thunder Game"))
            {
                Application.OpenURL("https://bluethundergame.online/nuts-and-bolts");
            }
        }
        private void GUIShowWarning()
        {
            Debug.LogWarning("Please Select Scene Game To Edit map");
        }
        private void SetType(int col, int row)
        {
            CLevelMakerEditor.levelSquares[row * maxCols + col] = CSquare.Nut;
            if (!currentSide.dots.Contains(row * maxCols + col))
            {
                currentSide.dots.Add(row * maxCols + col);
            }
        }
        private void SetBlock()
        {
            currentSide.prefabName = blockContents[selBlockInt].text;
            currentSide.rotBlockDegree = rotBlock;
        }
        private void SetHole(int col, int row, bool isVideo = false, bool isCoin = false)
        {
            if (isVideo)
                CLevelMakerEditor.levelSquares[row * maxCols + col] = CSquare.HoleVideo;
            else if (isCoin)
                CLevelMakerEditor.levelSquares[row * maxCols + col] = CSquare.HoleCoin;
            else
                CLevelMakerEditor.levelSquares[row * maxCols + col] = CSquare.Hole;
        }

        void Initialize()
        {
            AssetDatabase.Refresh();
            update = false;
            rotAngle = 0;
            fileName = levelNumber.ToString();
            if (levelObject == null)
            {
                levelSquares = new CSquare[maxCols * maxRows];
                Centers = new Vector2[levelSquares.Length];
                levelObject = new LevelObject();
                levelObject.maxCol = maxCols;
                levelObject.maxRow = maxRows;

            }
            else
            {
                sideContents = System.Array.ConvertAll(levelObject.sides.ToArray(), s => s.id + " Side " + s.prefabName);
            }
        }
        int GetIndex(int index)
        {
            for (int j = 0; j < levelObject.sides.Count; j++)
            {
                for (int i = 0; i < levelObject.sides[j].dots.Count; i++)
                {
                    if (levelObject.sides[j].dots[i] == index)
                    {
                        return j;
                    }
                }
            }
            return 0;
        }
        void SaveLevel()
        {
            if (!fileName.Contains(".txt"))
                fileName += ".txt";
            SaveMap(fileName);
        }
        void SaveMap(string fileName)
        {
            if (levelObject == null) return;

            levelObject.nuts = System.Array.ConvertAll(levelSquares, c => (int)c);
            string jsonStr = JsonUtility.ToJson(levelObject);
            if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsEditor)
            {
                //Write to file
                string activeDir = Application.dataPath + @"/NutBolts/Resources/Level/";
                string newPath = System.IO.Path.Combine(activeDir, levelNumber + ".txt");
                StreamWriter sw = new StreamWriter(newPath);
                sw.Write(jsonStr);
                sw.Close();
            }
            AssetDatabase.Refresh();
        }

        private bool LoadDataFromLocal(int currentLevel)
        {
            //Read data from text file
            TextAsset mapText = Resources.Load("Level/" + currentLevel) as TextAsset;
            if (mapText == null)
            {
                return false;

            }
            ProcessGameDataFromString(mapText.text);
            return true;
        }
        void ProcessGameDataFromString(string str)
        {
            levelObject = JsonUtility.FromJson<LevelObject>(str);
            maxCols = levelObject.maxCol;
            maxRows = levelObject.maxRow;
            levelSquares = System.Array.ConvertAll(levelObject.nuts, c => (CSquare)c);
            Centers = new Vector2[levelSquares.Length];           
            Initialize();
        }
        private void TestLevel()
        {
            targetEdit = false;
            PlayerPrefs.DeleteAll();
            SaveLevel();
            GameManager.collect = 0;
            GameManager lm = Camera.main.GetComponent<GameManager>();
            PlayerPrefs.SetInt("OpenLevelTest", levelNumber);
            PlayerPrefs.SetInt("OpenLevel", levelNumber);
            PlayerPrefs.SetInt("OpenTipTest", levelNumber);
            PlayerPrefs.Save();

            if (EditorApplication.isPlaying)
                EditorApplication.isPlaying = false;
            else
                EditorApplication.isPlaying = true;

            lm.ConstructLevel();
        }
        private void OpenTarget()
        {
            var asset = Resources.Load<CLevels>("Target/Level" + levelNumber);
            if (asset == null)
            {
                asset = CreateInstance<CLevels>();
                asset.name = "Level" + levelNumber;
                string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath("Assets/NutBolts/Resources/Target/Level" + levelNumber + ".asset");
                AssetDatabase.CreateAsset(asset, assetPathAndName);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
           
            }

            Selection.activeObject = asset;
        }
        private void PreviousLevel()
        {
            levelNumber--;
            if (levelNumber < 1)
                levelNumber = 1;
            if (!LoadDataFromLocal(levelNumber))
                levelNumber++;

        }
        private void NextLevel()
        {
            levelNumber++;
            if (!LoadDataFromLocal(levelNumber))
                levelNumber--;
        }
        private void AddLevel()
        {
            SaveLevel();
            levelNumber = GetLastLevel() + 1;
            levelObject = null;
            currentSide = new Side();
            Initialize();
            SaveLevel();
        }
        int GetLastLevel()
        {
            TextAsset mapText = null;
            for (int i = levelNumber; i < 50000; i++)
            {
                mapText = Resources.Load("Level/" + i) as TextAsset;
                if (mapText == null)
                {
                    return i - 1;
                }
            }
            return 0;
        }
        void ClearSide()
        {
            if (levelObject != null)
            {
                for (int i = 0; i < levelObject.sides.Count; i++)
                {
                    if (levelObject.sides[i].id == currentSide.id)
                    {
                        levelObject.sides.RemoveAt(i);
                    }
                }
            }
            currentSide = new Side();
        }
        void ClearNuts()
        {
            levelSquares = new CSquare[maxCols * maxRows];
            Centers = new Vector2[levelSquares.Length];
            levelObject.sides.Clear();
            currentSide = new Side();

        }

        void LoadSide(int id)
        {
            for (int i = 0; i < levelObject.sides.Count; i++)
            {
                if (levelObject.sides[i].id == id)
                {
                    currentSide = levelObject.sides[i];
                }
            }
        }
    }
}
