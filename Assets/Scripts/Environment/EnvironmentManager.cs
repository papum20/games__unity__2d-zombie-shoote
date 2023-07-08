using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnvironmentManager : MonoBehaviour
{

    public Transform player;

    public GameObject groundPrefab;
    public Transform groundParent;
    public EnemySpawner spawner;
    public Sprite[] groundSprites;

    public GameObject blockingWall;

    public Transform itemParent;


    //GROUND SPAWN POINT
    int spawnPoint = 0;

    //PLATFORMS AND CHESTS
    public GameObject platformPrefab;
    public GameObject chestPrefab;
    public Transform platformParent;
    public Transform chestParent;
    public int bonusRate = 5;
    int bonusSpawnPoint = 100;
    public float platfHeight;  //MAX HEIGHT DIFFERENCE BETWEEN PLATFORMS



    //BACKGROUND
    public GameObject[] backgrounds;


    //BOSSES
    int bossSpawnPoint = 400;
    int bossRate = 400;
    public GameObject[] bosses;
    public GameObject plantBallPrefab;
    public Transform projectileParent;

    public Camera dynamicCamera;
    public Camera staticCamera;
    public Slider bossHealthSlider;
    public GameObject bossHealthIcon;
    public GameObject invisibleWall;


    //SCORE
    public Text scoreUI;
    public Image newRecord;

    int score;





    private void Awake()
    {
        staticCamera.gameObject.SetActive(false);
        bossHealthSlider.gameObject.SetActive(false);
        bossHealthIcon.gameObject.SetActive(false);

        for (int i = -150; i <= 50; i += 100)
        {
            GameObject tmp = Instantiate(groundPrefab, new Vector2(i, -3.66f), Quaternion.identity, groundParent);
            tmp.GetComponent<GroundDestroy>().player = player;
            tmp.GetComponent<SpriteRenderer>().sprite = groundSprites[Random.Range(0, 4)];

            BackgroundSpawn(i, tmp.GetComponent<Transform>());
        }


        score = -1;
        scoreUI.text = score.ToString() + " pt";

    }



    private void Start()
    {
        OptionsSave res = SaveSystem.LoadOptions();
        Screen.SetResolution(res.resWidth, res.resHeight, true, 30);
    }



    void Update()
    {
        if(player.position.x >= spawnPoint)
        {
            spawnPoint += 100;

            //SCORE
            score++;
            scoreUI.text = score.ToString() + " pt";

            HighScoreData highScore = SaveSystem.LoadScore();
            int highScoreValue = highScore.highScore;
            if (score > highScoreValue)
            {
                SaveSystem.SaveScore(score);
                StartCoroutine(NewRecordPanel());
            }
            

            //GROUND SPAWN
            GameObject tmp = Instantiate(groundPrefab, new Vector2(spawnPoint + 50, -3.66f), Quaternion.identity, groundParent);
            tmp.GetComponent<GroundDestroy>().player = player;
            tmp.GetComponent<SpriteRenderer>().sprite = groundSprites[Random.Range(0, 4)];
            
            /*
            //CEILING X BUG
            GameObject tmpCeiling = Instantiate(groundPrefab, new Vector2(spawnPoint + 50, 24f), Quaternion.identity, groundParent);
            tmpCeiling.GetComponent<GroundDestroy>().player = player;
            */

            //BACKGROUND SPAWN
            BackgroundSpawn(spawnPoint + 50, tmp.GetComponent<Transform>());

            //ENEMY SPAWNER
            spawner.SpawnFunction(spawner.GetSpawnNumber(), spawnPoint);
            //            spawner.IncreaseSpawnPosition(100);


            //MOVES INVISIBLE WALL
            if (player.position.x - blockingWall.transform.position.x >= 300)
                blockingWall.transform.Translate(new Vector2(100f, 0f));
            
        }


        //BOSS SPAWN

        if (spawnPoint == bossSpawnPoint)
        {
            if (spawnPoint == bonusSpawnPoint)
                bonusSpawnPoint += 100;

            BossSpawn();
            bossSpawnPoint += bossRate;
            spawner.IncreaseSpawnNumber(1);
        }


        //BONUS SPAWN

        else if(spawnPoint == bonusSpawnPoint)
        {
            PlatformSpawn();
            bonusSpawnPoint += bonusRate * 100;
        }

    }




    void PlatformSpawn()
    {
        int platformNumber = Random.Range(1, 5);

        float xSpawn = spawnPoint;
        float ySpawn = 2f;
        Vector3 xScale = new Vector3(5, 0, 0);

        for (int i = 1; i <= platformNumber; i++)
        {
            Vector3 xScaleNew = new Vector3(Random.Range(3f, 4.5f), 0f, 0f);
            if (i == platformNumber) xScaleNew.x = 5f;

            float deltaPos = Random.Range(5f - xScale.x / 2f - xScaleNew.x / 2f, 8f);  //real scale = 5-xScale (standard scale = 5)
            if (Random.Range(0, 2) == 1) deltaPos = -deltaPos;

            Vector2 spawnPos = new Vector2(xSpawn + deltaPos, ySpawn + Random.Range(1f, Mathf.Min(platfHeight, 12f - ySpawn) ));   
            xSpawn = spawnPos.x;
            ySpawn = spawnPos.y;

            GameObject platf = Instantiate(platformPrefab, spawnPos, Quaternion.identity, platformParent);
            platf.GetComponent<GroundDestroy>().player = player;

            xScale = xScaleNew;
            if (i != platformNumber)
                platf.transform.localScale -= xScale;

            //CHEST ON TOP
            else
            {
                GameObject chest = Instantiate(chestPrefab, new Vector2(xSpawn, ySpawn + 1.4f), Quaternion.identity, chestParent);
                chest.GetComponent<Chest>().itemParent = itemParent;
                chest.GetComponent<Chest>().player = player;
            }
        }
    }





    void BackgroundSpawn(int spawnCenter, Transform parent)
    {
        for(int x = -45; x <= 45; x+=10)
            Instantiate(backgrounds[Random.Range(0, backgrounds.Length)], new Vector3(spawnCenter + x, -0.5f, 0f), Quaternion.identity, parent);
    }




    void BossSpawn()
    {
        GameObject boss = Instantiate(bosses[Random.Range(0, bosses.Length)], new Vector2(bossSpawnPoint, 7f), Quaternion.identity);
        if(boss.tag == "plantBoss")
        {
            PlantBoss plantScript = boss.GetComponent<PlantBoss>();
            plantScript.player = player;
            plantScript.ballParent = projectileParent;
            plantScript.ballPrefab = plantBallPrefab;
        }

        else if(boss.tag == "whiteBoss")
        {
            WhiteBoss whiteScript = boss.GetComponent<WhiteBoss>();
            whiteScript.player = player.gameObject;
        }

        BossCamera bossCamera = boss.GetComponent<BossCamera>();
        bossCamera.player = player;
        bossCamera.bossHealthIcon = bossHealthIcon;
        bossCamera.bossHealthSlider = bossHealthSlider;
        bossCamera.staticCamera = staticCamera;
        bossCamera.dynamicCamera = dynamicCamera;
        bossCamera.invisibleWall = invisibleWall;
    }




    IEnumerator NewRecordPanel()
    {
        newRecord.gameObject.SetActive(true);
        yield return new WaitForSeconds(4f);
        newRecord.gameObject.SetActive(false);
    }

}
