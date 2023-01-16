using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager instance;
        public static GameManager Instance
        {
            get
            {
                if (instance == null)
                {
                    Debug.LogError("GameManager is NULL!!");
                }
                return instance;
            }
        }
        
        #region Exposed_Variables

        #endregion

        #region Private_Variables
        
        private List<PlayerBodyEquipment> _playerBodyEquipments = new List<PlayerBodyEquipment>();

        #endregion

        #region Public_Variables

        #endregion

        #region Unity_Calls
        
        private void Awake()
        {
            instance = this;
        }
        
        private void OnEnable()
        {
            PlayerController.OnGetBodyEquipment += SetBodyEquipment;
        }

        private void OnDisable()
        {
            PlayerController.OnGetBodyEquipment -= SetBodyEquipment;
        }

        #endregion

        #region Private_Methods
        
        private void SetBodyEquipment(List<PlayerBodyEquipment> equipments)
        {
            _playerBodyEquipments = equipments;
        }

        #endregion

        #region Public_Methods

        public List<PlayerBodyEquipment> GetBodyEquipment()
        {
            return _playerBodyEquipments;
        }

        #endregion

        
    }
}