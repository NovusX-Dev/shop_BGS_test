using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class PlayerController : MonoBehaviour
{
    public static event Action<List<PlayerBodyEquipment>> OnGetBodyEquipment;
    
    #region Exposed_Variables
    
    [SerializeField] private float moveSpeed = 70;
    [SerializeField] private float movementSmoothing = 0.1f;
    [SerializeField] private bool normalizedMovement = true;
    [SerializeField] private GameObject modelObject;
    
    #endregion

    #region Private_Variables

    private float _speed;
    private Vector2 _axisVector = Vector2.zero;
    private Vector3 _currentVelocity = Vector3.zero;
    private bool _canInteractNPC = false;
    private bool _canMove = true;
    private Rigidbody2D _rb2D;
    private Animator _currentAnimator;
    private Animator _modelAnimator;
    private ShopKeeper _currentShopKeeper = null;
    private PlayerInventory _playerInventory = null;
    private List<PlayerBodyEquipment> _bodyEquipments = new List<PlayerBodyEquipment>();

    #endregion

    #region Unity_calls

        private void Start()
    	{
    		_rb2D = GetComponent<Rigidbody2D>();
    		modelObject.SetActive(true);
    		_modelAnimator = modelObject.GetComponent<Animator>();
    		_currentAnimator = _modelAnimator;
            _bodyEquipments = GetComponentsInChildren<PlayerBodyEquipment>().ToList();
            _playerInventory = UIManager.Instance.Inventory;
            OnGetBodyEquipment?.Invoke(_bodyEquipments);
        }
    
        private void Update()
        {
            if(_canMove)
            {
                CalculateMovement();
                SetAnimations();
            }
    
            if (_canInteractNPC)
            {
                if (Input.GetKeyDown(KeyCode.E) && _currentShopKeeper != null)
                {
                    UIManager.Instance.ActivateDialogPanel(true);
                    SetCanMove(false);
                }
            }
            
            if (Input.GetKeyDown(KeyCode.I))
            {
                var active = _playerInventory.MainPanel.gameObject.activeInHierarchy;
                _playerInventory.MainPanel.gameObject.SetActive(!active);
                SetCanMove(active);
            }
        }
    
        private void FixedUpdate()
        {
            // Move our character
            Move();
        }

        private void OnEnable()
        {
            Shop.OnCloseBuyShop += SetCanMove;
            SellShop.OnCloseSellShop += SetCanMove;
            PlayerInventory.OnInventoryClosed += SetCanMove;
        }

        private void OnDisable()
        {
            Shop.OnCloseBuyShop -= SetCanMove;
            SellShop.OnCloseSellShop -= SetCanMove;
            PlayerInventory.OnInventoryClosed -= SetCanMove;
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if(other.CompareTag("Shop Keeper"))
            {
                if(!other.TryGetComponent(out ShopKeeper keeper)) return;
                _canInteractNPC = true;
                keeper.ActivateButton(true);
                _currentShopKeeper = keeper;
            }
        }
    
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Shop Keeper"))
            {
                if(!other.TryGetComponent(out ShopKeeper keeper)) return;
                _canInteractNPC = false;
                keeper.ActivateButton(false);
                keeper.ButtonPressed(false);
                _currentShopKeeper = null;
            }
        }

    #endregion

    #region Private_Methods

    private void CalculateMovement()
    {
        // get speed from the rigid body to be used for animator parameter Speed
        _speed = _rb2D.velocity.magnitude;

        // Get input axis
        _axisVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        //normalize it for good topdown diagonal movement
        if (normalizedMovement)
        {
            _axisVector.Normalize();
        }
    }

    private void SetAnimations()
    {
        // Set speed parameter to the animator
        _currentAnimator.SetFloat("Speed", _speed);

        // Check keys for actions and use appropiate function
        //
        if (Input.GetKey(KeyCode.Space))  // SWING ATTACK
        {
            PlayAnimation("Swing");
        }

        else if (Input.GetKey(KeyCode.C))  // THRUST ATTACK
        {
            PlayAnimation("Thrust");
        }

        else if (Input.GetKey(KeyCode.X))  // BOW ATTACK
        {
            PlayAnimation("Bow");
        }
    }

	private void PlayAnimation(string animationName)
	{
		// Play given animation in the current directions animator
		_currentAnimator.Play(animationName, 0);
	}

	private void Move()
	{
        if (!_canMove) return;

        // Set target velocity to smooth towards
        Vector2 targetVelocity = new Vector2(_axisVector.x * moveSpeed, _axisVector.y * moveSpeed) * Time.fixedDeltaTime;

		// Smoothing out the movement
		_rb2D.velocity = Vector3.SmoothDamp(_rb2D.velocity, targetVelocity, ref _currentVelocity, movementSmoothing);
	}

    private void SetCanMove(bool status)
    {
        _canMove = status;
    }
    
    #endregion

    #region Public_Methods

    public bool SetUIInteraction(bool interact)
    {
        return _canInteractNPC = interact;
    }
    
    #endregion

}
