using Cinemachine;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    #region Tooltip
    [Tooltip("Populate with the child MinimapPlayer GameObject")]
    #endregion
    [SerializeField] private GameObject miniMapPlayer;

    private Transform playerTransform;

    private void Start()
    {
        playerTransform = GameManager.Instance.GetPlayer().transform;

        // Populatye player as cinemachine camera target
        CinemachineVirtualCamera cinemachineVirtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        cinemachineVirtualCamera.Follow = playerTransform;

        // Set minimap player icon
        SpriteRenderer spriteRenderer = miniMapPlayer.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = GameManager.Instance.GetPlayerMinimapIcon();
        }
    }

    private void Update()
    {
        // Move the minimap player to follow the player
        if (playerTransform != null && miniMapPlayer != null)
        {
            miniMapPlayer.transform.position = playerTransform.position;
        }


    }

        #region Validation
    #if UNITY_EDITOR

        private void OnValidate()
        {
            HelperUtilities.ValidateCheckNullValue(this, nameof(miniMapPlayer), miniMapPlayer);
        }

    #endif
        #endregion

}