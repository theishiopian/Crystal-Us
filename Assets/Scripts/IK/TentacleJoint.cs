using UnityEngine;

public class TentacleJoint : MonoBehaviour
{
    public Vector3 Axis;
    public Vector3 StartOffset;

    void Awake()
    {
        StartOffset = transform.localPosition;
    }
}