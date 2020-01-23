using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUDComponent : MonoBehaviour, ICharacterComponent
{
    public List<GameObject> forceArrows;
    public List<GameObject> forceLevels;

    public Slider HPBar;
    public Slider XPBar;

    public Text levelText;

    private CharacterHealthComponent health;
    private CharacterLevelComponent level;

    // Start is called before the first frame update
    void Start()
    {
        health = this.gameObject.GetComponent<CharacterHealthComponent>();
        level = this.gameObject.GetComponent<CharacterLevelComponent>();
    }

    // Update is called once per frame
    void Update()
    {
        if(HPBar.maxValue != health.maxHealth)
        {
            HPBar.maxValue = health.maxHealth;
        }
        else if(HPBar.value != health.health)
        {
            HPBar.value = health.health;
        }

        if(XPBar.maxValue != level.GetNextLevelXP())
        {
            XPBar.maxValue = level.GetNextLevelXP();
        }
        else if(XPBar.value != level.XP)
        {
            XPBar.value = level.XP;
        }

        levelText.text = "LV: " + level.level;
    }

    private void ClearArrows()
    {
        foreach (GameObject arrow in forceArrows)
        {
            arrow.SetActive(false);
        }
    }

    private void ClearLevels()
    {
        foreach (GameObject level in forceLevels)
        {
            level.SetActive(false);
        }
    }

    int attackLevel = 0; int oldPower = 0;
    int arrowLevel = 0;
    bool hasCharged = false;

    public void InitAttack(float attackPower)
    {
        if (attackPower < 2)
        {
            arrowLevel += Mathf.FloorToInt(attackPower * 10 / 2);
            arrowLevel *= 2;
            if (arrowLevel > oldPower && oldPower < 30 && attackLevel < 3)
            {
                forceArrows[oldPower / 3].SetActive(true);
                oldPower++;
            }
            else if (oldPower >= 15 && attackLevel > 0 && attackLevel < 2 && !hasCharged)
            {
                oldPower = 0;
                attackLevel++;
                arrowLevel = 0;
                ClearArrows();
                //Debug.Log("called");
                hasCharged = true;
            }
        }
    }

    public void ResetAttack()
    {
        oldPower = 0;
        arrowLevel = 0;
        hasCharged = false;
        ClearArrows();
    }

    public void SetAttackLevel(float attackPower)
    {
        if (attackPower >= 2)
        {
            attackLevel = 2;
            forceLevels[1].SetActive(true);
        }
        else if (attackPower >= 1)
        {
            attackLevel = 1;
            forceLevels[0].SetActive(true);
        }
        else
        {
            attackLevel = 0;
            ClearLevels();
        }
    }
}
