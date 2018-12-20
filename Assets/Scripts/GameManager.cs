using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public List<Ally> allies;
    public List<EnemyCop> enemies;
    public List<Civilian> civilians;
    public static GameManager instance = null;
    private Player player;
    private float scoref;
    private int score;
    private bool gameStarted;
    private int highScore;
    private Text text;
    private Camera cam;
    private float height;
    private float width;
    private Civilian civilian;
    private EnemyCop enemy;
    private Ally ally;

    //Awake is always called before any Start functions
    void Awake()
    {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
        gameStarted = false;
        highScore = 0;
    }

    // Use this for initialization
    void Start () {
        cam = Camera.main;
        allies = new List<Ally>();
        enemies = new List<EnemyCop>();
        civilians = new List<Civilian>();

        height = cam.orthographicSize + 1;
        width = cam.orthographicSize * cam.aspect + 1;

        score = 0;
        scoref = 0;
        text = FindObjectOfType<Text>();
        text.text = "Highscore: " + highScore;
    }

    void Update()
    {
        #if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_EDITOR
        if (!gameStarted && Input.anyKey)
        {
            StartGame();
        }
        #elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
        if (!gameStarted && (Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began))
        {
            Ray raycast = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit raycastHit;
            if (Physics.Raycast(raycast, out raycastHit))
            {
                if (raycastHit.collider.CompareTag("Button"))
                {
                    StartGame();
                }
            }
        }
        #endif
    }

    // Update is called once per frame
    void FixedUpdate () {
        if (gameStarted)
        {
            scoref += Time.deltaTime;
            score = (int)scoref;
            text.text = "Score: " + score;

            for (int i = 0; i < civilians.Count; i++)
            {
                if ((player.transform.position - civilians[i].gameObject.transform.position).magnitude > 40)
                {
                    civilians[i].gameObject.SetActive(false);
                    civilians.RemoveAt(i);
                }
            }

            for (int i = 0; i < enemies.Count; i++)
            {
                if ((player.transform.position - enemies[i].gameObject.transform.position).magnitude > 40)
                {
                    enemies[i].gameObject.SetActive(false);
                    enemies.RemoveAt(i);
                }
            }

            for (int i = 0; i < allies.Count; i++)
            {
                if ((player.transform.position - allies[i].gameObject.transform.position).magnitude > 40)
                {
                    allies[i].gameObject.SetActive(false);
                    allies.RemoveAt(i);
                }
            }

            if (score > 60 && enemies.Count < 4)
            {

            }
            else if (score > 40 && enemies.Count < 3)
            {

            }
            else if (score > 20 && enemies.Count < 2)
            {

            }
            else if (score > 4 && enemies.Count < 1)
            {

            }

            if (civilians.Count < 6)
            {
                Instantiate(civilian, new Vector3(cam.transform.position.x + width + Random.Range(10, 30), cam.transform.position.y + height + Random.Range(10, 30), 1), Quaternion.identity);
            }
        }
    }

    public void GameOver()
    {
        highScore = score;
        SceneManager.LoadScene("Menu");
        text = FindObjectOfType<Text>();
        text.text = "Highscore: " + highScore;
        allies = new List<Ally>();
        enemies = new List<EnemyCop>();
        civilians = new List<Civilian>();
        player = null;
    }

    private void StartGame()
    {
        player = FindObjectOfType<Player>();
        gameStarted = true;
        score = 0;
        SceneManager.LoadScene("City");
        text = FindObjectOfType<Text>();
        text.text = "Score: " + score;
        civilians = new List<Civilian>(FindObjectsOfType<Civilian>());
    }
}
