using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleController : MonoBehaviour
{
    public TentacleJoint[] Joints;

    public float SamplingDistance;
    public float LearningRate;

    public GameObject target;

    float[] angles = new float[7];

    private void Update()
    {
        int i = 0;
        foreach(TentacleJoint j in Joints)
        {
            angles[i] = j.transform.localEulerAngles.z;
            i++;
        }
        InverseKinematics(target.transform.position, angles);
    }

    public void InverseKinematics(Vector3 target, float[] angles)
    {
        
        for (int i = 0; i < Joints.Length; i++)
        {
            // Gradient descent
            // Update : Solution -= LearningRate * Gradient
            float gradient = PartialGradient(target, angles, i);
            angles[i] -= LearningRate * gradient;
            Joints[i].transform.localEulerAngles = new Vector3(0,0,angles[i]);
        }
    }


    public float PartialGradient(Vector3 target, float[] angles, int i)
    {
        // Saves the angle,
        // it will be restored later
        float angle = angles[i];
        
        // Gradient : [F(x+SamplingDistance) - F(x)] / h
        float f_x = DistanceFromTarget(target, angles);

        angles[i] += SamplingDistance;
        float f_x_plus_d = DistanceFromTarget(target, angles);

        float gradient = (f_x_plus_d - f_x) / SamplingDistance;

        // Restores
        angles[i] = angle;

        return gradient;
    }

    public float DistanceFromTarget(Vector3 target, float[] angles)
    {
        Vector3 point = ForwardKinematics(angles);
        
        float d = Vector3.Distance(point, target);

        return d;
    }

    public Vector3 ForwardKinematics(float[] angles)
    {
        Vector3 prevPoint = Joints[0].transform.position;
        Quaternion rotation = Quaternion.identity;
        for (int i = 1; i < Joints.Length; i++)
        {
            // Rotates around a new axis
            rotation *= Quaternion.AngleAxis(angles[i - 1], Joints[i - 1].Axis);
            Vector3 nextPoint = prevPoint + rotation * Joints[i].StartOffset;

            prevPoint = nextPoint;
        }

        return prevPoint;
    }
}
