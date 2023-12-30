using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecieveContactDamage : MonoBehaviour
{
    #region Header
    [Header("The contact damage amount to receive")]
    #endregion
    [SerializeField] private int contactDamageAmount;
    private Health health;
    private void Awake ()
    {
        //Load components
        health = GetComponent<Health>();
    }

    public void TakeContactDamage(int damageAmount = 0)
    {
        if (contactDamageAmount > 0){
            damageAmount = contactDamageAmount;
        }
        Debug.Log("Potential contact damage of " + damageAmount);
        health.TakeDamage (damageAmount);
    }

    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckPositiveValue(this, nameof (contactDamageAmount), contactDamageAmount, true);
    }
#endif
    #endregion
}
