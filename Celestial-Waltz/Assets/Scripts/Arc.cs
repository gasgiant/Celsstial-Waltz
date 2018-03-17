using System.Collections.Generic;
using UnityEngine;
using MarchingBytes;

public class Arc : MonoBehaviour {

    public Metronome metr;
    public GameObject pointPrefab;

    public float positionSnapTime = 0.4f;
    public float rotationSnapTime = 0.4f;
    public float maxSnapSpeed = 20;
    public float gather_threshold;
    public float magnet_threshold;
    public float magnet_velocity;
    public float fail_threshold = 7;
    public float ignore_distance = 30;
    public float ignore_angle = 90;
    public bool closed;

    public List<Bar> bars = new List<Bar>();

    ArcState state = ArcState.Normal;

    int nextIndex;
    [HideInInspector]
    public List<ArcPoint> points;
    [HideInInspector]
    public Vector3 startDirection;
    List<Vector3> initialPointsPositions = new List<Vector3>();
    Transform tr;

    Vector3 vel;
    float rotVel;
    float inRotation;

    void OnEnable()
    {
        Spawner.instance.arcs.Add(this);

        tr = transform;
        metr = Metronome.instance;
        inRotation = Vector3.Angle(Vector3.up, startDirection);
        if (Vector3.Cross(Vector3.up, startDirection).z > 0)
            inRotation = 360 - inRotation;

        foreach (var point in points)
        {
            initialPointsPositions.Add(point.GetLocalPosition());
        }
    }

    void FixedUpdate()
    {
        MovementUpdate();
        if (state == ArcState.Close)
            CheckPointsDistance();
        if (state == ArcState.Failed || state == ArcState.Ignored)
        {
            //EasyObjectPool.instance.ReturnObjectToPool(gameObject);
            gameObject.SetActive(false);
            Debug.Log("Buy!");
        }
    }

    void MovementUpdate()
    {
        Vector3 relativePosition = tr.position - PlayerController.instance.tr.position;
        float distance = relativePosition.magnitude;
        float angle = Vector3.Angle(PlayerController.instance.direction, relativePosition.normalized);
        //int barDistance = (int)(distance / metr.scale);

        if (distance > ignore_distance && angle > ignore_angle)
            state = ArcState.Ignored;
            
        if (state == ArcState.Snap)
        {
            SnapUpdate(1);

            if (distance < 0.2 * metr.scale && angle < 10)
            {
                state = ArcState.Close;
            }
        }
        if (state == ArcState.Normal && distance < 3 * metr.scale && angle < 40)
        {
            if (!PlayerController.instance.targetArc)
            {
                state = ArcState.Snap;
                PlayerController.instance.targetArc = tr;
            }
        }    
    }

    void SnapUpdate(int bar)
    {
        Vector3 target = PlayerController.instance.trajectExtrapolation[bar];
        tr.position = Vector3.SmoothDamp(tr.position, target, ref vel, positionSnapTime, maxSnapSpeed);

        Quaternion targetRotation = Quaternion.Euler(0, 0, PlayerController.instance.rb.rotation + inRotation);
        tr.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.fixedDeltaTime / rotationSnapTime);
        
    }

    void CheckPointsDistance()
    {
        if (nextIndex > points.Count - 1)
        {
            state = ArcState.Gathered;
            PlayerController.instance.targetArc = null;
            return;
        }

        Vector3 nextPointLocalPosition = points[nextIndex].GetLocalPosition();
        Vector3 playerLocalPosition = tr.InverseTransformPoint(PlayerController.instance.tr.position);
        float distance = (nextPointLocalPosition - playerLocalPosition).magnitude;

        if (distance > fail_threshold)
        {
            state = ArcState.Failed;
            PlayerController.instance.targetArc = null;
            return;
        }

        if (distance < magnet_threshold)
        {
            points[nextIndex].SetLocalPosition(nextPointLocalPosition + 
                (playerLocalPosition - nextPointLocalPosition).normalized * Time.deltaTime * magnet_velocity);
        }
        else
        {
            points[nextIndex].SetLocalPosition(nextPointLocalPosition + 
                (initialPointsPositions[nextIndex] - nextPointLocalPosition).normalized * Time.deltaTime * magnet_velocity);
        }

        if (distance < gather_threshold)
        {
            points[nextIndex].Diactivate();
            nextIndex++;
                
        }  
    }


    public List<Vector3> GetBarsPointsPositions(BarType type, float scale, bool reverse = false, int signature = 3)
    {
        List<Vector3> ret = new List<Vector3>();
        float r = scale * 2 / Mathf.PI;

        switch (type)
        {
            case BarType.Circle0:
                for (int i = 0; i < signature + 1; i++)
                {
                    ret.Add(new Vector3(r * Mathf.Cos((float)i / signature * 0.5f * Mathf.PI) - r,
                                        r * Mathf.Sin((float)i / signature * 0.5f * Mathf.PI)));
                }
                break;
            case BarType.Circle1:
                for (int i = 0; i < signature + 1; i++)
                {
                    ret.Add(new Vector3(r * Mathf.Cos((float)i / signature * 0.5f * Mathf.PI + 0.5f * Mathf.PI),
                                        r * Mathf.Sin((float)i / signature * 0.5f * Mathf.PI + 0.5f * Mathf.PI) - r));
                }
                break;
            case BarType.Circle2:
                for (int i = 0; i < signature + 1; i++)
                {
                    ret.Add(new Vector3(r * Mathf.Cos((float)i / signature * 0.5f * Mathf.PI + Mathf.PI) + r,
                                        r * Mathf.Sin((float)i / signature * 0.5f * Mathf.PI + Mathf.PI)));
                }
                break;
            case BarType.Circle3:
                for (int i = 0; i < signature + 1; i++)
                {
                    ret.Add(new Vector3(r * Mathf.Cos((float)i / signature * 0.5f * Mathf.PI + 1.5f * Mathf.PI),
                                        r * Mathf.Sin((float)i / signature * 0.5f * Mathf.PI + 1.5f * Mathf.PI) + r));
                }
                break;
            case BarType.Line0:
                for (int i = 0; i < signature + 1; i++)
                {
                    ret.Add(new Vector3(scale * (float)i / signature, 0));
                }
                break;
            case BarType.Line1:
                for (int i = 0; i < signature + 1; i++)
                {
                    ret.Add(new Vector3(0, scale * (float)i / signature));
                }
                break;
        }

        if (reverse)
        {
            for (int i = 0; i < ret.Count; i++)
            {
                ret[i] = new Vector3(-ret[i].x, ret[i].y, ret[i].z);
            }
        }

        return ret;
    }

    public Vector3 GetStartDirection(BarType type, bool reverse)
    {
        Vector3 dir = Vector3.right;
        switch (type)
        {
            case BarType.Circle0:
                dir = Vector3.up;
                break;
            case BarType.Circle1:
                dir = reverse ? Vector3.right : Vector3.left;
                break;
            case BarType.Circle2:
                dir = Vector3.down;
                break;
            case BarType.Circle3:
                dir = reverse ? Vector3.left : Vector3.right;
                break;
            case BarType.Line0:
                dir = reverse ? Vector3.left : Vector3.right;
                break;
            case BarType.Line1:
                dir = reverse ? Vector3.down : Vector3.up;
                break;
        }
        return dir;
    }

    enum ArcState { Gathered, Failed, Ignored, Close, Snap, Normal}
    public enum BarType { Circle0, Circle1, Circle2, Circle3, Line0, Line1 }

    [System.Serializable]
    public class Bar
    {
        public BarType type;
        public bool reverse;
    }
}
