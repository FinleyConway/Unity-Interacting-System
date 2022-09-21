using UnityEngine;

public class InteractController : MonoBehaviour
{
    [Header("Interact Holder And Collision Mask")]
    [SerializeField] private Transform _objectHoldingPoint;
    [SerializeField] private LayerMask _grabbables;

    [Header("Interact Attributes")]
    [Tooltip("The distance away the player can be to interact with an object")]
    [SerializeField] private float _interactDistance = 1.5f;
    [Tooltip("How far the player can hold the object away from the player")]
    [SerializeField] private float _maxDistanceFromPlayer = 1f;
    private Vector3 _defaultHoldingPosition;
    private bool _isHoldingObject;

    [Header("Input")]
    [SerializeField] private KeyCode _interactKey;

    private IGrabable _grabable;
    private Camera _cam;

    private void Start()
    {
        // get a reference to the main camera
        _cam = Camera.main;

        // store the default holding point
        _defaultHoldingPosition = _objectHoldingPoint.localPosition;
    }

    private void Update()
    {
        /// if object currently exists
        if (_grabable != null)
        {
            CarryObject();
        }
        // pick up the object
        else if (Input.GetKeyDown(_interactKey) && !_isHoldingObject)
        {
            Interact();
            return;
        }

        // drop if input is pressed
        if (_grabable != null && Input.GetKeyDown(_interactKey) && _isHoldingObject)
        {
            _grabable.Drop();
            _isHoldingObject = false;
            _grabable = null;
        }
    }

    // interact with door
    private void Interact()
    {
        // if raycast hits
        if (Physics.Raycast(_cam.transform.position, _cam.transform.forward, out RaycastHit hit, _interactDistance, _grabbables))
        {
            // if hit is interactable
            if (hit.transform.TryGetComponent(out IInteractable interactable))
            {
                // interactable with object
                interactable.Interact(gameObject);
            }

            // if interacted with object
            if (hit.transform.TryGetComponent(out IGrabable grabable))
            {
                // pick up object
                _isHoldingObject = true;
                _grabable = grabable;
                _grabable.Grab(_objectHoldingPoint);
            }
        }
    }

    // carry the object
    private void CarryObject()
    {
        float maxDistance = _defaultHoldingPosition.z + _maxDistanceFromPlayer;
        float scrollDelta = Input.mouseScrollDelta.y;

        if (scrollDelta == 1)
        {
            if (!(_objectHoldingPoint.localPosition.z >= maxDistance))
                _objectHoldingPoint.localPosition += 1 * Vector3.forward;
        }
        if (scrollDelta == -1)
        {
            if (!(_objectHoldingPoint.localPosition.z <= _defaultHoldingPosition.z))
                _objectHoldingPoint.localPosition -= 1 * Vector3.forward;
        }

        _objectHoldingPoint.localPosition = new Vector3(0, 0, _objectHoldingPoint.localPosition.z);

        // tell it to be carried 
        _grabable.Carry(_objectHoldingPoint);
    }

    // show the reach distance
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * _interactDistance);
    }
}