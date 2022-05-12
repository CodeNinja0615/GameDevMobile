using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SC_GroundGenerator : MonoBehaviour
{
    public Camera mainCamera;
    public Transform startPoint; //Point from where ground tiles will start
    public SC_PlatformTile tilePrefab;
    public float movingSpeed = 20;
    public int tilesToPreSpawn = 5; //How many tiles should be pre-spawned
    public int tilesWithoutObstacles = 3; //How many tiles at the beginning should not have obstacles, good for warm-up
    private GUIStyle guiStyle = new GUIStyle();
    List<SC_PlatformTile> spawnedTiles = new List<SC_PlatformTile>();
    int nextTileToActivate = -1;
    [HideInInspector]
    public bool gameOver = false;
    static bool gameStarted = false;
    public float score = 0;
    public GameObject tapToStartUI;
    public GameObject tapToRestartUI;
    public GameObject pauseUI;
    public static SC_GroundGenerator instance;
    public Acclerometer acclerometer;

    // Start is called before the first frame update
    void Start()
    {
        movingSpeed = PlayerPrefs.GetFloat("MovingSpeed");
        instance = this;

        Vector3 spawnPosition = startPoint.position;
        int tilesWithNoObstaclesTmp = tilesWithoutObstacles;
        for (int i = 0; i < tilesToPreSpawn; i++)
        {
            spawnPosition -= tilePrefab.startPoint.localPosition;
            SC_PlatformTile spawnedTile = Instantiate(tilePrefab, spawnPosition, Quaternion.identity) as SC_PlatformTile;
            if (tilesWithNoObstaclesTmp > 0)
            {
                spawnedTile.DeactivateAllObstacles();
                tilesWithNoObstaclesTmp--;
            }
            else
            {
                spawnedTile.ActivateRandomObstacle();
            }


            spawnPosition = spawnedTile.endPoint.position;
            spawnedTile.transform.SetParent(transform);
            spawnedTiles.Add(spawnedTile);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Move the object upward in world space x unit/second.
        //Increase speed the higher score we get




        if (!gameOver && gameStarted)
        {

            transform.Translate(-spawnedTiles[0].transform.forward * Time.deltaTime * (movingSpeed + (score / 500)), Space.World);
            score += Time.deltaTime * movingSpeed;


        }
        if (score > 100 && score % 100 == 0)
        {
            movingSpeed += 1f;
        
        }
        //if (score >= 300)
        //{
        //    score = score + 5* Time.deltaTime;
        //}
        if (mainCamera.WorldToViewportPoint(spawnedTiles[0].endPoint.position).z < 0)
        {
            //Move the tile to the front if it's behind the Camera
            SC_PlatformTile tileTmp = spawnedTiles[0];
            spawnedTiles.RemoveAt(0);
            tileTmp.transform.position = spawnedTiles[spawnedTiles.Count - 1].endPoint.position - tileTmp.startPoint.localPosition;
            tileTmp.ActivateRandomObstacle();
            spawnedTiles.Add(tileTmp);
        }

        if (gameOver || !gameStarted)
        {
            //Time.timeScale = 1f;
            acclerometer.enabled = false;
            pauseUI.SetActive(false);
            if (gameOver)
            {

                tapToRestartUI.SetActive(true);

            }
            else
            {

                tapToStartUI.SetActive(true);
            }

                //if (Input.GetKeyDown(KeyCode.Space))
                if (Input.touchCount > 0)
                {

                    acclerometer.enabled = true;
                    if (gameOver)
                    {

                        tapToRestartUI.SetActive(false);
                        pauseUI.SetActive(true);

                        //Restart current scene
                        Scene scene = SceneManager.GetActiveScene();
                        SceneManager.LoadScene(scene.name);
                        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

                    }

                    else
                    {

                        tapToStartUI.SetActive(false);
                        pauseUI.SetActive(true);
                        //Start the game
                        gameStarted = true;
                    }
                }
            }
        }


    } 