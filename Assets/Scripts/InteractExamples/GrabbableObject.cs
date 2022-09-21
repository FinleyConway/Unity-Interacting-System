using UnityEngine;

public class GrabbableObject : MonoBehaviour, IGrabable
{
    [Header("Grabbable Attributes")]
    [Tooltip("How fast the object travels to the holding point")]
    [SerializeField] private float _maxSpeed = 3;
    [Tooltip("The max distance away from the holding point before it hits the max speed")]
    [SerializeField] private float _maxSpeedDistance = 10;
    private float _currentSpeed;
    private float _currentDistance;

    private Transform _objectHoldingPoint;

    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // while the object is not null
        if (_objectHoldingPoint != null)
        {
            float minSpeed = 0;
            Vector3 direction = _objectHoldingPoint.position - transform.position;

            // follows the holding point
            // futher away the object is to the point the faster it travels back to the point
            _currentDistance = Vector3.Distance(_objectHoldingPoint.position, transform.position);
            _currentSpeed = Mathf.SmoothStep(minSpeed, _maxSpeed, _currentDistance / _maxSpeedDistance);
            _currentSpeed *= Time.fixedDeltaTime;

            _rb.velocity = direction.normalized * (_currentSpeed * 1000);
        }
    }

    // follow holding point
    public void Grab(Transform holdingPoint)
    {
        _objectHoldingPoint = holdingPoint;
        holdingPoint.position = transform.position;
        _rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    // stop following holding point
    public void Drop()
    {
        _rb.constraints = RigidbodyConstraints.None;
        _objectHoldingPoint = null;
        _currentDistance = 0;
    }

    // allows the player to maniplate the object when being carried
    public void Carry(Transform holdingPoint)
    {
        _objectHoldingPoint = holdingPoint;
    }
}
