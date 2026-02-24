using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using TMPro;



public class PlayerStats : MonoBehaviour
{
    CharacterData characterData;
    public CharacterData.Stats baseStats;
    [SerializeField] CharacterData.Stats actualStats;

    float health;
    CharacterScriptableObject characterData;
    float currentHealth;
    float currentMight;
    float currentProjectileSpeed;
    float currentMoveSpeed;
    float currentRecovery;
    float currentMagnet;

    public ParticleSystem damageEffect;

    #region Current Stats Properties
    public float CurrentHealth
    {
        //Check if the value has changed
        get { return health; }
        set 
        { 
            if(health != value)
            {
                health = value; 
                if(GameManager.instance != null)
                {
                    GameManager.instance.currentHealthDisplay.text = string.Format(
                        "Health: {0} / {1}",
                        health, actualStats.maxHealth
                    );
                }
                //Update the real time value of the stat
                //Add any additional logic here that needs to be executed when value changes
            }       
        }
    }

    public float MaxHealth
    {
        get {return actualStats.maxHealth;}

        //If we try and set the max health, the UI interface
        // on the pause screen will also be updated.
        set
        {
            // Check if the value has changed
            if(actualStats.maxHealth != value)
            {
                actualStats.maxHealth = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentHealthDisplay.text = string.Format(
                        "Health: {0} / {1}",
                        health, actualStats.maxHealth
                    );
                }
                //Update the real time value of the stat
                //Add any additional logic here that needs to be executed when the value changes
            }
        }
    }

    public float CurrentRecovery
    {
        //Check if the value has changed
        get { return Recovery; }
        set { Recovery= value;}
    }
    public float Recovery
    {
        get {return actualStats.recovery;}
        set
        {
            //Check if the value has changed
            if(actualStats.recovery != value)
            {
                actualStats.recovery = value;
                if(GameManager.instance != null)
                {
                    GameManager.instance.currentRecoveryDisplay.text = "Recovery" + actualStats.recovery;
                }
            }
        }
    }

    public float CurrentMoveSpeed
    {
        get { return MoveSpeed;}
        set { MoveSpeed = value;}
    }
        
    

    public float MoveSpeed   
    {
        //Check if the value has changed
        get { return actualStats.moveSpeed; }
        set 
        { 
            if(actualStats.moveSpeed != value)
            {
                actualStats.moveSpeed = value; 
                if(GameManager.instance != null)
                {
                    GameManager.instance.currentMoveSpeedDisplay.text = "Move Speed: " + actualStats.moveSpeed;
                }
                //Update the real time value of the stat
                //Add any additional logic here that needs to be executed when value changes
            }       
        }
    }

    public float CurrentMight
    {
        get {return Might;}
        set {Might = value;}
    }

    public float Might
    {
        //Check if the value has changed
        get { return actualStats.might; }
        set 
        { 
            if(actualStats.might != value)
            {
                actualStats.might = value; 
                if(GameManager.instance != null)
                {
                    GameManager.instance.currentMightDisplay.text = "Might: " + actualStats.might;
                }
                //Update the real time value of the stat
                //Add any additional logic here that needs to be executed when value changes
            }       
        }
    }

    public float CurrnetProjectileSpeed
    {
        get {return Speed;}
        set { Speed = value;}
    }

    public float Speed
    {
        //Check if the value has changed
        get { return actualStats.speed; }
        set 
        { 
            if(actualStats.speed != value)
            {
                actualStats.speed = value; 
                if(GameManager.instance != null)
                {
                    GameManager.instance.currentProjectileSpeedDisplay.text = "Projectile Speed: " + actualStats.speed;
                }
                //Update the real time value of the stat
                //Add any additional logic here that needs to be executed when value changes
            }       
        }
    }

    public float CurrentMagnet
    {
        get { return Magnet;}
        set { Magnet = value;}
    }
    public float Magnet
    {
        //Check if the value has changed
        get { return actualStats.magnet; }
        set 
        { 
            if(actualStats.magnet != value)
            {
                actualStats.magnet = value; 
                if(GameManager.instance != null)
                {
                    GameManager.instance.currentMagnetDisplay.text = "Magnet: " + actualStats.magnet;
                }
                //Update the real time value of the stat
                //Add any additional logic here that needs to be executed when value changes
            }       
        }
    }
    #endregion

    //experience and level system can be added later
    [Header("Experience/Level")]
    public int experience =0;
    public int level =1;
    public int experienceCap;
    //public int experienceCapIncrease;

    [System.Serializable]

    public class LevelRange
    {
        public int startLevel;
        public int endLevel;
        public int experienceCapIncrease;
    }

    [Header("I-Frames")]
    public float invincibilityDuration;
    float invincibilityTimer;
    bool isInvincible;

    public List<LevelRange> levelRanges;

    PlayerInventory inventory;
    public int passiveItemIndex;
    public int weaponIndex;

    [Header("UI")]
    public Image HealthBar;
    public Image expBar;
    public TextMeshProUGUI levelText;


    void Awake()
    {
        if (CharacterSelector.instance != null)
        {
            characterData = CharacterSelector.GetData();
            CharacterSelector.instance.DestroySingleton();
        }
        else
        {
            Debug.LogWarning("CharacterSelector not found. Using default character data from this object.");
            characterData = GetComponent<CharacterScriptableObject>();
            if (characterData == null)
            {
                // 기본값으로 테스트용 데이터 설정이 필요할 수 있음
                Debug.LogError("No CharacterData found! Please start from the character selection scene.");
                return;
            }
        }

        inventory = GetComponent<PlayerInventory>();

        //Assign the variables

        baseStats = actualStats = characterData.stats;
        health = actualStatsStats.maxHealth;

        // CurrentHealth = characterData.MaxHealth;
        // CurrentMight = characterData.Might;
        // CurrentProjectileSpeed = characterData.ProjectileSpeed;
        // CurrentMoveSpeed = characterData.MoveSpeed;
        // CurrentRecovery = characterData.Recovery;
        // CurrentMagnet = characterData.Magnet;

        // //Start the starting weapons
        // SpawnWeapon(characterData.StartingWeapon);
    }

    void Start()
    {
        //Spawn the starting weapon
        inventory.Add(characterData.StartingWeapon);

        if (levelRanges != null && levelRanges.Count > 0)
        {
            experienceCap = levelRanges[0].experienceCapIncrease;
        }
        else
        {
            Debug.LogWarning("LevelRanges is empty! Setting default experienceCap.");
            experienceCap = 100; // 기본값 설정
        }

        GameManager.instance.currentHealthDisplay.text = "Health: " + currentHealth;
        GameManager.instance.currentRecoveryDisplay.text = "Recovery: " + currentRecovery;
        GameManager.instance.currentMightDisplay.text = "Might: " + currentMight;
        GameManager.instance.currentMoveSpeedDisplay.text = "Move Speed: " + currentMoveSpeed;
        GameManager.instance.currentProjectileSpeedDisplay.text = "Projectile Speed: " + currentProjectileSpeed;
        GameManager.instance.currentMagnetDisplay.text = "Magnet: " + currentMagnet;

        GameManager.instance.AssignChosenCharacterUI(characterData);

        UpdateHealthBar();
        UpdateExpBar();
        UpdateLevelText();
    }

    void Update()
    {
        if(invincibilityTimer > 0)
        {
            invincibilityTimer -= Time.deltaTime;
        }
        else if (isInvincible)
        {
            isInvincible = false;
        }

        Recover();
    }

    public void RecalculateStats()
    {
        actualStats = baseStats;
        foreach (PlayerInventory.Slot s in inventory.passiveSlots)
        {
            Passive p = s.item as Passive;
            if(p)
            {
                actualStats += p.GetBoosts();
            }
        }
    }

    public void IncreaseExperience(int amount)
    {
        experience += amount;
        LevelUpChecker();       
        UpdateExpBar();
    }

    void LevelUpChecker()
    {
        if(experience >= experienceCap)
        {
            level++;
            experience -= experienceCap;
            int experienceCapIncrease = 0;
            foreach (LevelRange range in levelRanges)
            {
                if (level >= range.startLevel && level <= range.endLevel)
                {
                    experienceCap += range.experienceCapIncrease;
                    break;
                }
            }
            experienceCap += experienceCapIncrease;
            //Level up effects can be added here
            UpdateLevelText();

            GameManager.instance.StartLevelUP();
        }
    }

    void UpdateExpBar()
    {
        expBar.fillAmount = (float)experience / experienceCap;
    }

    void UpdateLevelText()
    {
        levelText.text = "Level " + level.ToString();
    }

    public void TakeDamage(float damage)
    {
        if(!isInvincible)
        {
            CurrentHealth -= damage;
            if(damageEffect) Destroy(Instantiate(damageEffect, transform.position, Quaternion.identity),5f);

            isInvincible = true;
            invincibilityTimer = invincibilityDuration;
            //추가 효과들 여기에

            if(CurrentHealth <= 0)
            {
                Kill();
            }
            UpdateHealthBar();
        }
    }

    void UpdateHealthBar()
    {
        if(HealthBar != null)
        {
            HealthBar.fillAmount = CurrentHealth / actualStats.maxHealth;
        }
    }
    public void Kill()
    {
        if(!GameManager.instance.isGameOver)
        {
            GameManager.instance.AssignLevelReachedUI(level);
            //GameManager.instance.AssignChosenWeaponAndPassiveItemUI(inventory.weaponSlotsUI, inventory.passiveItemSlotsUI);
            GameManager.instance.GameOver();
        }

    }

    public void RestoreHealth(float amount)
    {
        if(CurrentHealth < actualStats.maxHealth)
        {
            CurrentHealth += amount;
            if(CurrentHealth > actualStats.maxHealth)
            {
                CurrentHealth = actualStats.maxHealth;
            }
        }
    }

    void Recover()
    {
        if(CurrentHealth < actualStats.maxHealth)
        {
            CurrentHealth += CurrentRecovery * Time.deltaTime;
            if(CurrentHealth > actualStats.maxHealth)
            {
                CurrentHealth = actualStats.maxHealth;
            }
        }
    }
    [System.Obsolete("Old function that is kept to maintain compativibility with the InventoryManager. Will be removed soon.")]
    public void SpawnWeapon(GameObject weapon)
    {
        //Checking if the slots are full, and returning if it is
        if(weaponIndex >= inventory.weaponSlots.Count -1 ) // Must be -1 because a list starts from 0
        {
            Debug.Log("Inventory Slots Full");
            return;
        }

        GameObject spawnedWeapon = Instantiate(weapon, transform.position, Quaternion.identity);
        spawnedWeapon.transform.SetParent(transform); // Set the weapon as a child of the player
        inventory.AddWeapon(weaponIndex, spawnedWeapon.GetComponent<WeaponController>());//Add the weapon th it's inventory slot
        weaponIndex++;

    }
    [System.Obsolete("No need to spawn passive items directly now.")]
        public void SpawnPassiveItem(GameObject passiveItem)
    {
        //Checking if the slots are full, and returning if it is
        if(passiveItemIndex >= inventory.passiveSlots.Count -1 ) // Must be -1 because a list starts from 0
        {
            Debug.Log("Inventory Slots Full");
            return;
        }

        GameObject spawnedPassiveItem = Instantiate(passiveItem, transform.position, Quaternion.identity);
        spawnedPassiveItem.transform.SetParent(transform); // Set the weapon as a child of the player
        inventory.AddPassiveItem(passiveItemIndex, spawnedPassiveItem.GetComponent<PassiveItem>());//Add the weapon th it's inventory slot
        passiveItemIndex++;

    }
}
