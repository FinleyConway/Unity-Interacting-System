using UnityEngine;

public interface IGrabable
{
    void Grab(Transform holdingPoint);
    void Drop();
    void Carry(Transform holdingPoint);
}
