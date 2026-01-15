using UnityEngine;
using System.Collections.Generic;
using System.Collections;


public class PlayerStats : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    CharacterScriptableObject characterData;
    float currentHealth;
    float currentMight;
    float currentProjectileSpeed;
    float currentMoveSpeed;
    float currentRecovery;
    float currentMagnet;

    #region Current Stats Properties
    public float CurrentHealth
    {
        //Check if the value has changed
        get { return currentHealth; }
        set 
        { 
            if(currentHealth != value)
            {
                currentHealth = value; 
                if(GameManager.instance != null)
                {
                    GameManager.instance.currentHealthDisplay.text = "Health: " + currentHealth;
                }
                //Update the real time value of the stat
                //Add any additional logic here that needs to be executed when value changes
            }       
        }
    }

    public float CurrentRecovery
    {
        //Check if the value has changed
        get { return currentRecovery; }
        set 
        { 
            if(currentRecovery != value)
            {
                currentRecovery = value; 
                if(GameManager.instance != null)
                {
                    GameManager.instance.currentRecoveryDisplay.text = "Recovery: " + currentRecovery;
                }
                //Update the real time value of the stat
                //Add any additional logic here that needs to be executed when value changes
            }       
        }
    }

    public float CurrentMoveSpeed   
    {
        //Check if the value has changed
        get { return currentMoveSpeed; }
        set 
        { 
            if(currentMoveSpeed != value)
            {
                currentMoveSpeed = value; 
                if(GameManager.instance != null)
                {
                    GameManager.instance.currentMoveSpeedDisplay.text = "Move Speed: " + currentMoveSpeed;
                }
                //Update the real time value of the stat
                //Add any additional logic here that needs to be executed when value changes
            }       
        }
    }

    public float CurrentMight
    {
        //Check if the value has changed
        get { return currentMight; }
        set 
        { 
            if(currentMight != value)
            {
                currentMight = value; 
                if(GameManager.instance != null)
                {
                    GameManager.instance.currentMightDisplay.text = "Might: " + currentMight;
                }
                //Update the real time value of the stat
                //Add any additional logic here that needs to be executed when value changes
            }       
        }
    }

    public float CurrentProjectileSpeed
    {
        //Check if the value has changed
        get { return currentProjectileSpeed; }
        set 
        { 
            if(currentProjectileSpeed != value)
            {
                currentProjectileSpeed = value; 
                if(GameManager.instance != null)
                {
                    GameManager.instance.currentProjectileSpeedDisplay.text = "Projectile Speed: " + currentProjectileSpeed;
                }
                //Update the real time value of the stat
                //Add any additional logic here that needs to be executed when value changes
            }       
        }
    }

    public float CurrentMagnet
    {
        //Check if the value has changed
        get { return currentMagnet; }
        set 
        { 
            if(currentMagnet != value)
            {
                currentMagnet = value; 
                if(GameManager.instance != null)
                {
                    GameManager.instance.currentMagnetDisplay.text = "Magnet: " + currentMagnet;
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

    InventoryManager inventory;
    public int passiveItemIndex;

    public int weaponIndex;


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

        inventory = GetComponent<InventoryManager>();

        CurrentHealth = characterData.MaxHealth;
        CurrentMight = characterData.Might;
        CurrentProjectileSpeed = characterData.ProjectileSpeed;
        CurrentMoveSpeed = characterData.MoveSpeed;
        CurrentRecovery = characterData.Recovery;
        CurrentMagnet = characterData.Magnet;

        //Start the starting weapons
        SpawnWeapon(characterData.StartingWeapon);
    }

    void Start()
    {
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

    public void IncreaseExperience(int amount)
    {
        experience += amount;
        LevelUpChecker();       
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
        }
    }

    public void TakeDamage(float damage)
    {
        if(!isInvincible)
        {
            CurrentHealth -= damage;
            isInvincible = true;
            invincibilityTimer = invincibilityDuration;
            //추가 효과들 여기에

            if(CurrentHealth <= 0)
            {
                Kill();
            }
        }
    }
    public void Kill()
    {
        if(!GameManager.instance.isGameOver)
        {
            GameManager.instance.AssignLevelReachedUI(level);
            GameManager.instance.AssignChosenWeaponAndPassiveItemUI(inventory.weaponSlotsUI, inventory.passiveItemSlotsUI);
            GameManager.instance.GameOver();
        }

    }

    public void RestoreHealth(float amount)
    {
        if(CurrentHealth < characterData.MaxHealth)
        {
            CurrentHealth += amount;
            if(CurrentHealth > characterData.MaxHealth)
            {
                CurrentHealth = characterData.MaxHealth;
            }
        }
    }

    void Recover()
    {
        if(CurrentHealth < characterData.MaxHealth)
        {
            CurrentHealth += CurrentRecovery * Time.deltaTime;
            if(CurrentHealth > characterData.MaxHealth)
            {
                CurrentHealth = characterData.MaxHealth;
            }
        }
    }

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

        public void SpawnPassiveItem(GameObject passiveItem)
    {
        //Checking if the slots are full, and returning if it is
        if(passiveItemIndex >= inventory.passiveItemSlots.Count -1 ) // Must be -1 because a list starts from 0
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
