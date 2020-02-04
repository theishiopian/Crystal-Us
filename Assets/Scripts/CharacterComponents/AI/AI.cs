using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AI : MonoBehaviour
{
    // Returns a random Vector2 in one of the four cardinal directions
    public Vector2 RandomDirection()
    {
        //Debug.Log("called");
        Vector2 direction = Random.insideUnitCircle.normalized;
        float angle = Mathf.Round(Mathf.Rad2Deg * Mathf.Atan2(direction.y, direction.x) / 90) * 90; // change the 90s to 45s if you want 8 cardinal directions
        //float sin = Mathf.Sin(angle);
        //float cos = Mathf.Cos(angle);
        //Debug.Log(angle);
        switch (angle)//hack to get the direction right based on angle TODO: generic solution?
        {
            case 0: return new Vector2(1.0f, 0.0f);
            case 90: return new Vector2(0.0f, 1.0f);
            case 180: return new Vector2(-1.0f, 0.0f);
            case -180: return new Vector2(1.0f, 0.0f);
            case -90: return new Vector2(0.0f, -1.0f);
            default: return new Vector2(0.0f, 0.0f);
        }

        //return new Vector2(cos, sin);
    }

    // Returns a Vector2 in the rough direction of the input target (world space) rounded to one of the four cardinal directions
    public Vector2 TargetDirection(Vector3 target)
    {
        Vector2 direction = (target - this.transform.position).normalized;
        float angle = Mathf.Round((Mathf.Rad2Deg * (Mathf.Atan2(direction.y, direction.x))) / 90) * 90; // change the 90s to 45s if you want 8 cardinal directions
        //float sin = Mathf.Sin(angle);
        //float cos = Mathf.Cos(angle);
        //var d = new Vector2(cos,sin);

        Debug.DrawRay(this.transform.position, direction, Color.blue);
        //Debug.DrawRay(this.transform.position, d, Color.green);
        //Debug.Log(angle);

        switch (angle)//hack to get the direction right based on angle TODO: generic solution?
        {
            case 0: return new Vector2(1.0f, 0.0f);
            case 90: return new Vector2(0.0f, 1.0f);
            case 180: return new Vector2(-1.0f, 0.0f);
            case -180: return new Vector2(-1.0f, 0.0f);//not sure why these two need to be  different betwqeen methods, needs further investigation
            case -90: return new Vector2(0.0f, -1.0f);
            default: return new Vector2(0.0f, 0.0f);
        }

        //return new Vector2(-1.0f,0.0f);
    }
}
