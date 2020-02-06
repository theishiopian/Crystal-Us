using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVFXComponent : MonoBehaviour, ICharacterComponent
{
    //vfx for the charging animation
    public ParticleSystem chargingEffect;
    public GameObject chargeLevel1;
    public GameObject chargeLevel2;

    public void InitAttack(float attackPower)
    {
        if (!chargingEffect.isPlaying && attackPower == 0)
        {
            StartCoroutine("playChargeEffectAfterDelay", attackPower);
        }
    }

    public void ResetAttack()
    {
        chargingEffect.Stop();
    }

    public void SetAttackLevel(float attackPower)
    {
        //Debug.Log(attackPower);
        if (attackPower >= 2)
        {
            chargeLevel2.SetActive(true);
            chargeLevel1.SetActive(false);
            if (!chargingEffect.isPlaying && attackPower < 2) chargingEffect.Play();
        }
        else if (attackPower >= 1)
        {
            chargeLevel1.SetActive(true);
            chargeLevel2.SetActive(false);
            if (!chargingEffect.isPlaying) chargingEffect.Play();
        }
        else
        {
            chargeLevel1.SetActive(false);
            chargeLevel2.SetActive(false);
        }
    }

    IEnumerator playChargeEffectAfterDelay(float attackPower)
    {

        yield return new WaitForSeconds(0.2f);

        if(Input.GetMouseButton(0))//dirty hack
        {
            chargingEffect.Play();
        }
        yield return null;
    }
}
