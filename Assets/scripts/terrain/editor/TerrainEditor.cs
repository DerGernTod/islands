using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections;

namespace scripts.terrain {
    public class TerrainEditor : EditorWindow {
        private string terrainName = "";
        private bool groupEnabled;
        private bool myBool;
        private float myFloat;
        private Vector2 scrollPosition = Vector2.zero;
        private Texture[] textures = new Texture2D[0];
        private Sprite[] sprites = new Sprite[0];
        private int enabledTextureId = -1;
        private int prevEnabledTextureId = -1;
        private TerrainTile currentSceneObject;
        private TerrainTile terrainTilePrefab;
        private Color whiteAlpha = new Color(1, 1, 1, .5f);
        private Color whiteVisible = Color.white;
        private GameObject currentTerrain;
        private string spritePath = "Isometric Block";
        private float curReloaded = 0;
        private IEnumerator resourceLoader;
        [MenuItem("Window/Terrain Editor")]
        public static void Init() {
            TerrainEditor editor = GetWindow<TerrainEditor>();
            editor.Show();
        }

        public void Awake() {
            string terrainTilePrefabPath = "prefabs/terrain/TerrainTile";
            Debug.Log("[Terrain Editor] reading terrain tile prefab from '" + terrainTilePrefabPath + "'");
            terrainTilePrefab = Resources.Load<TerrainTile>(terrainTilePrefabPath);
            reloadResources();
        }

        public void Update() {
            if (resourceLoader != null) {
                if (!resourceLoader.MoveNext()) {
                    resourceLoader = null;
                }
                Repaint();
            }
        }

        private void reloadResources() {
            sprites = Resources.LoadAll<Sprite>(spritePath).Where(e => {
                return !e.name.EndsWith("_00");
            }).ToArray();
            resourceLoader = reloadResourcesEnum();
        }

        private IEnumerator reloadResourcesEnum() {
            textures = new Texture2D[sprites.Length];
            for(int i = 0; i < sprites.Length; i++) {
                Sprite sprite = sprites[i];
                var croppedTexture = new Texture2D((int)sprite.textureRect.width, (int)sprite.textureRect.height);
                var pixels = sprite.texture.GetPixels(
                    (int)sprite.textureRect.x,
                    (int)sprite.textureRect.y,
                    (int)sprite.textureRect.width,
                    (int)sprite.textureRect.height);
                croppedTexture.SetPixels(pixels);
                croppedTexture.Apply();
                textures[i] = croppedTexture;
                curReloaded = i;
                yield return null; 
            }
        }

        public void OnFocus() {
            SceneView.onSceneGUIDelegate -= onSceneGUI;
            SceneView.onSceneGUIDelegate += onSceneGUI;
        }

        public void OnDestroy() {
            SceneView.onSceneGUIDelegate -= onSceneGUI;
        }
        public void OnGUI() {
            GUILayout.Label("Terrain Settings", EditorStyles.boldLabel);
            terrainName = EditorGUILayout.TextField("Terrain name", terrainName);

            if (GUILayout.Button("Create Terrain")) {
                string curTerrainName = terrainName.Equals("") ? "Unnamed" : terrainName;
                currentTerrain = new GameObject(curTerrainName);
                currentTerrain.transform.position = Vector3.zero;
            }

            spritePath = EditorGUILayout.TextField("Resource path to sprites", spritePath);
            EditorGUI.BeginDisabledGroup(resourceLoader != null);
            if (GUILayout.Button(resourceLoader == null ? "Reload Sprites" : "Reloading " + (int)((curReloaded/sprites.Length)*100) + "%")) {
                reloadResources();
            }
            EditorGUI.EndDisabledGroup();

            groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
            myBool = EditorGUILayout.Toggle("Toggle", myBool);
            myFloat = EditorGUILayout.Slider("Slider", myFloat, -3, 3);
            EditorGUILayout.EndToggleGroup();
            GUIStyle btnStyle = new GUIStyle(GUI.skin.button) {
                fixedWidth = 50,
                fixedHeight = 50
            };
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            enabledTextureId = GUILayout.SelectionGrid(enabledTextureId, textures, 5, btnStyle);
            if (enabledTextureId >= 0 && enabledTextureId != prevEnabledTextureId) {
                if (currentTerrain) {
                    if (currentSceneObject != null) {
                        DestroyImmediate(currentSceneObject.gameObject);
                    }
                    overrideCurrentSceneTile();
                    Debug.Log("Leftclick to place, rightclick to cancel");
                } else {
                    Debug.LogWarning("Please create a terrain first!");
                }
            }
            EditorGUILayout.EndScrollView();

            prevEnabledTextureId = enabledTextureId;

        }
        private Vector2 snapToSpriteSize(Vector2 val) {
            
            float x = val.x;
            float y = val.y;
            Vector2 extents = currentSceneObject.GetComponent<SpriteRenderer>().sprite.bounds.extents;
            float height = extents.y / 2;
            float width = extents.x * 2;
            float offset = 0;
            float yPosResult = Mathf.Round((y + height / 2) / height) * height;
            if (Mathf.Abs(Mathf.Round(yPosResult / height) % 2) == 1) {
                offset = extents.x;
                x -= offset;
            }

            return new Vector2(Mathf.Round(x / width) * width + offset, yPosResult);
        }

        private void onSceneGUI(SceneView sceneView) {
            Vector3 mousePosition = Event.current.mousePosition;
            mousePosition.y = sceneView.camera.pixelHeight - mousePosition.y;
            mousePosition = sceneView.camera.ScreenToWorldPoint(mousePosition);
            if (currentSceneObject) {
                Vector2 roundedPosition = snapToSpriteSize(mousePosition);
                currentSceneObject.transform.position = roundedPosition;
                currentSceneObject.GetComponent<SpriteRenderer>().sortingOrder = -(int)roundedPosition.y;


                Event current = Event.current;
                if (current.type == EventType.MouseUp) {
                    if (current.button == 0) {
                        currentSceneObject.GetComponent<SpriteRenderer>().color = whiteVisible;
                        currentSceneObject.transform.parent = currentTerrain.transform;
                        overrideCurrentSceneTile();
                    } else if (current.button == 1) {
                        DestroyImmediate(currentSceneObject.gameObject);
                        currentSceneObject = null;
                        enabledTextureId = -1;
                        Selection.activeGameObject = null;
                    }
                    current.Use();
                }
            }

        }

        private void overrideCurrentSceneTile() {
            currentSceneObject = Instantiate(terrainTilePrefab);
            currentSceneObject.GetComponent<SpriteRenderer>().sprite = sprites[enabledTextureId];
            currentSceneObject.GetComponent<SpriteRenderer>().color = whiteAlpha;
            Selection.activeGameObject = currentSceneObject.gameObject;
        }

        private void reset() {
            GUI.SetNextControlName("");
            GUI.FocusControl("");
            terrainName = "";
            groupEnabled = false;
            myBool = false;
            myFloat = 0;
        }
    }
}