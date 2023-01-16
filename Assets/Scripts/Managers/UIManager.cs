using System.Collections;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("UI Manager is NULL!!");
            }
            return instance;
        }
    }

    #region Exposed_Variables
    
    [SerializeField] private  GameObject dialoguePanel;
    [SerializeField] private GameObject buyPanel;
    [SerializeField] private GameObject sellPanel;
    [SerializeField] private TextMeshProUGUI errorText;
    [SerializeField] private float errorDisplayDelay = 2f;
    [SerializeField] private TextMeshProUGUI coins;
    [SerializeField] private PlayerInventory playerInventory = null;

    #endregion

    #region Private_Variables

    private WaitForSeconds _errorDelayWaitForSeconds;
    
    #endregion

    #region Public_Variables

    public PlayerInventory Inventory => playerInventory;

    #endregion

    #region Unity_Calls
        
    private void Awake()
    {
        instance = this;
        _errorDelayWaitForSeconds = new WaitForSeconds(errorDisplayDelay);
    }
    
    #endregion

    #region Private_Methods

    #endregion

    #region Public_Methods
    public void UpdateCoinsUI(int amount)
    {
        coins.text = $"Coins: {amount}";
    }

    public void ActivateDialogPanel(bool activate)
    {
        dialoguePanel.SetActive(activate);
    }

    public void OpenBuyShop()
    {
        buyPanel.SetActive(true);
    }

    public void OpenSellShop()
    {
        sellPanel.SetActive(true);
    }

    public void OpenPlayerInventory()
    {
        playerInventory.MainPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void DisplayError(string error)
    {
        StartCoroutine(Delay());
        IEnumerator Delay()
        {
            var parent = errorText.transform.parent;
            parent.gameObject.SetActive(true);
            errorText.text = error;
            yield return _errorDelayWaitForSeconds;
            parent.gameObject.SetActive(false);
        }
    }

    #endregion
    
    
    

    
}
