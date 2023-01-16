using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    private static CoinManager instance;
    public static CoinManager Instance 
    {
        get 
        { 
        if( instance == null )
        {
            Debug.LogError("Coin Manager is null!!!");
        }
        return instance;
        }
    }
    
    #region Exposed_Variables

    #endregion

    #region Private_Variables
    
    private int _coins;

    #endregion

    #region Public_Variables

    #endregion

    #region Unity_Calls
    
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        _coins = 500;
        UIManager.Instance.UpdateCoinsUI(_coins);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            AddCoins(50);
        }
    }

    #endregion

    #region Private_Methods

    #endregion

    #region Public_Methods
    
    public void AddCoins(int amount)
    {
        _coins += amount;

        UIManager.Instance.UpdateCoinsUI(_coins);
    }

    public void DeductCoins(int amount)
    {
        _coins -= amount;

        if(_coins < 1)
        {
            _coins = 0;
        }
        UIManager.Instance.UpdateCoinsUI(_coins);
    }

    public int GetCoinAmount()
    {
        return _coins;
    }

    #endregion


    

    
}
