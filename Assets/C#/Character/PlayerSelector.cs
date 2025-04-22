using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerSelector : MonoBehaviour
{
    public int playerIndex;
    public Sprite[] characterSprites;
    public Transform[] positions; // 存放各圖片的位置

    private int currentIndex = 0;
    private bool inputLocked = false;
    public bool isReady = false;
    private GameObject currentImageObject;

    private PlayerInput playerInput;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        var actionMap = playerInput.currentActionMap;
        actionMap.FindAction("Move").performed += OnMove;
        actionMap.FindAction("Confirm").performed += OnConfirm;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        //if (inputLocked || isReady) return;

        //Vector2 move = context.ReadValue<Vector2>();
        //if (move.x > 0.5f)
        //{
        //    OnRightMove();
        //}
        //else if (move.x < -0.5f)
        //{
        //    OnLeftMove();
        //}
    }

    private void OnLeftMove()
    {
        currentIndex = (currentIndex - 1 + characterSprites.Length) % characterSprites.Length;
        UpdateImage();
        StartCoroutine(UnlockInputDelay());
    }

    private void OnRightMove()
    {
        currentIndex = (currentIndex + 1) % characterSprites.Length;
        UpdateImage();
        StartCoroutine(UnlockInputDelay());
    }

    private void OnConfirm(InputAction.CallbackContext context)
    {
        //if (!isReady)
        //{
        //    isReady = true;
        //    MultiplayerManager.Instance.ConfirmPlayerSelection(playerIndex, currentIndex);
        //    MultiplayerManager.Instance.UpdateReady();
        //}
    }

    private void UpdateImage()
    {
        //if (currentImageObject != null)
        //{
        //    Destroy(currentImageObject);
        //}

        //GameObject newImageObj = new GameObject("PlayerImage_" + playerIndex);
        //var image = newImageObj.AddComponent<UnityEngine.UI.Image>();
        //image.sprite = characterSprites[currentIndex];

        //newImageObj.transform.SetParent(positions[playerIndex], false);
        //currentImageObject = newImageObj;
    }

    private IEnumerator UnlockInputDelay()
    {
        inputLocked = true;
        yield return new WaitForSeconds(0.3f);
        inputLocked = false;
    }
}
