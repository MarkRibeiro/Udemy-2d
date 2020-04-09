using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{

	public float waitToRespawn;
	public PlayerController thePlayer;

	public GameObject deathSplosion;

	public int coinCount;
    private int coinBonusLifecount;
    public int bonusLifeThreshold;

    public AudioSource coinSound;

	public Text coinText;

    public Image heart1;
    public Image heart2;
    public Image heart3;

    public Sprite heartFull;
    public Sprite heartHalf;
    public Sprite heartEmpty;

    public int maxHealth;
    public int healthCount;

    private bool respawning;
    
    public ResetOnRespawn[] objectsToReset;

    public bool invincible;

    public Text livesText;
    public int startingLives;
    public int currentLives;

    public GameObject gameOverScreen;

    public AudioSource levelMusic;
    public AudioSource gameOverMusic;
    public AudioSource endLevelMusic;



    // Start is called before the first frame update
    void Start()
    {
        thePlayer = FindObjectOfType<PlayerController>();

        healthCount = maxHealth;

        objectsToReset = FindObjectsOfType<ResetOnRespawn>();

        if (PlayerPrefs.HasKey("CoinCount")){
            coinCount = PlayerPrefs.GetInt("CoinCount");
        }

        coinText.text = "x " + coinCount;

        if (PlayerPrefs.HasKey("PlayerLives"))
        {
            currentLives = PlayerPrefs.GetInt("PlayerLives");
        } else {
            currentLives = startingLives;
        }

        livesText.text = "x " + currentLives;
    }

    // Update is called once per frame
    void Update()
    {
        if(healthCount <= 0)
        {
            Respawn();
        }

        if(coinBonusLifecount >= bonusLifeThreshold) {
            currentLives += 1;
            livesText.text = "x " + currentLives;
            coinBonusLifecount -= bonusLifeThreshold;
            //coinCount -= bonusLifeThreshold;
            coinText.text = "x " + coinCount;
        }
    }

    public void Respawn(){
        if (!respawning) {
            currentLives -= 1;

            if (currentLives >= 0)
            {
                respawning = true;
                livesText.text = "x " + currentLives;
                StartCoroutine("RespawnCo");
            } else {
                //Instantiate(deathSplosion, thePlayer.transform.position, thePlayer.transform.rotation);
                thePlayer.gameObject.SetActive(false);
                gameOverScreen.SetActive(true);
                levelMusic.Stop();
                gameOverMusic.Play();

                //levelMusic.volume = levelMusic.volume / 2f;

            }
        }
    }

    public IEnumerator RespawnCo(){
    	thePlayer.gameObject.SetActive(false);

    	Instantiate(deathSplosion, thePlayer.transform.position, thePlayer.transform.rotation);

    	yield return new WaitForSeconds(waitToRespawn);

        healthCount = maxHealth;
        respawning = false;
        UpdateHeartMeter();

        coinCount = 0;
        coinText.text = "x " + coinCount;

        thePlayer.transform.position = thePlayer.respawnPosition;
    	thePlayer.gameObject.SetActive(true);

        for(int i=0; i < objectsToReset.Length; i++){
            objectsToReset[i].gameObject.SetActive(true);
            objectsToReset[i].ResetObject();
        }

    }

    public void AddCoins(int coinsToAdd){
    	coinCount += coinsToAdd;
        coinBonusLifecount += coinsToAdd;

    	coinText.text = "x " + coinCount;
        coinSound.Play();
    }

    public void GiveHealth(int healthToGive){
        healthCount += healthToGive;

        if(healthCount > maxHealth){
            healthCount = maxHealth; 
        }

        coinSound.Play();

        UpdateHeartMeter();
    }

    public void HurtPlayer(int damageToTake)
    {
        if (!invincible){
            healthCount -= damageToTake;
            UpdateHeartMeter();

            thePlayer.Knockback();

            thePlayer.hurtSound.Play();
        }
    }

    public void UpdateHeartMeter()
    {
        switch (healthCount)
        {
            case 6:
                heart1.sprite = heartFull;
                heart2.sprite = heartFull;
                heart3.sprite = heartFull;
                return;

            case 5:
                heart1.sprite = heartFull;
                heart2.sprite = heartFull;
                heart3.sprite = heartHalf;
                return;

            case 4:
                heart1.sprite = heartFull;
                heart2.sprite = heartFull;
                heart3.sprite = heartEmpty;
                return;

            case 3:
                heart1.sprite = heartFull;
                heart2.sprite = heartHalf;
                heart3.sprite = heartEmpty;
                return;

            case 2:
                heart1.sprite = heartFull;
                heart2.sprite = heartEmpty;
                heart3.sprite = heartEmpty;
                return;

            case 1:
                heart1.sprite = heartHalf;
                heart2.sprite = heartEmpty;
                heart3.sprite = heartEmpty;
                return;

            case 0:
                heart1.sprite = heartEmpty;
                heart2.sprite = heartEmpty;
                heart3.sprite = heartEmpty;
                return;

            default:
                heart1.sprite = heartEmpty;
                heart2.sprite = heartEmpty;
                heart3.sprite = heartEmpty;
                return;
        }
    }

    public void AddLives(int livesToAdd)
    {
        coinSound.Play();

        currentLives += livesToAdd;
        livesText.text = "x " + currentLives;

    }
}
