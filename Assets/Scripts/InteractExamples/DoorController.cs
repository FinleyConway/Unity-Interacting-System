using System.Collections;
using UnityEngine;

public class DoorController : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject _pivot;
    [SerializeField] private float _timeToOpenDoor = 0.5f;
    private bool _isDoorOpen = false;
    private Transform _victim;

    // open door
    private IEnumerator OnRotateDoor()
    {
        float time = 0;
        Quaternion startRotation = _pivot.transform.rotation;
        Quaternion rotateTo = Quaternion.identity;

        if (_isDoorOpen)
            _isDoorOpen = false;
        else
        {
            rotateTo = DirectionHandler() ? Quaternion.Euler(0, -90, 0) : Quaternion.Euler(0, 90, 0);
            _isDoorOpen = true;
        }

        while (time < _timeToOpenDoor)
        {

            _pivot.transform.rotation = Quaternion.Slerp(startRotation, rotateTo, (time / _timeToOpenDoor));
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    // checks if player is behind or infront of the door
    // infront = true
    // else false
    private bool DirectionHandler()
    {
        bool facing = false;
        Vector3 direction = (_victim.position - transform.position).normalized;
        float facingDirection = Vector3.Dot(transform.forward, direction);
        return facing ? facingDirection >= 0.5f : facingDirection <= -0.5;
    }

    // the interface that the interact script will look when this object gets triggered
    public void Interact(GameObject owner)
    {
        _victim = owner.transform;
        StartCoroutine(OnRotateDoor());
    }
}
