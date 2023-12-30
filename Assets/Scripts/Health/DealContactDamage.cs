using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class DealContactDamage : MonoBehaviour
{
    #region Header DEAL DAMAGE
    [Space (10)]
    [Header("DEAL DAMAGE")]
    #endregion
    #region Tooltip
    [Tooltip("The contact damage to deal (is overridden by the receiver)")]
    #endregion
    [SerializeField] private int contactDamageAmount;
    #region Tooltip
    [Tooltip("Specify what layers objects should be on to receive contact damage")]
    #endregion
    [SerializeField] private LayerMask layerMask;
    private bool isColliding = false;
    // Trigger contact damage when enter a collider
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If already colliding with something return
        if (isColliding) return;
        ContactDamage (collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isColliding) return;
        ContactDamage (collision);
    }
    
    private void ContactDamage(Collider2D collision)
    {
        int collisionObjectLayerMask = (1 << collision.gameObject.layer);

        if((layerMask.value & collisionObjectLayerMask) == 0) return;

        RecieveContactDamage recieveContactDamage = collision.gameObject.GetComponent<RecieveContactDamage>();

        if (recieveContactDamage != null)
        {
            isColliding = true;

            Invoke("ResetContactCollision", Settings.contactDamageCollisionResetDelay);

            recieveContactDamage.TakeContactDamage(contactDamageAmount);
        }
    }

    private void ResetContactCollision()
    {
        isColliding = false;
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
