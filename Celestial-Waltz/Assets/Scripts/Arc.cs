using System.Collections.Generic;
using UnityEngine;
using MarchingBytes;

public class Arc : MonoBehaviour {

    public Metronome metr;
    public GameObject pointPrefab;

    float positionSnapTime = 0.4f;
    float rotationSnapTime = 0.4f;
    float maxSnapSpeed = 20;
    float close_threshold;
    float gather_threshold;
    float magnet_threshold;
    float magnet_velocity;
    float fail_threshold = 7;
    float ignore_distance = 30;
    float ignore_angle = 90;
    public bool closed;


    public List<RelativeDirection> bars_directions;

    [HideInInspector]
    public List<Bar> bars = new List<Bar>();

    ArcState state = ArcState.Normal;

    int nextIndex;

    [HideInInspector]
    public List<ArcPoint> points;

    [HideInInspector]
    public bool spawned;
    List<Vector3> initialPointsPositions = new List<Vector3>();
    Transform tr;

    Vector3 vel;
    float rotVel;

    void OnEnable()
    {
        if (spawned)
            Spawner.instance.arcs.Add(this);
        else
        {
            spawned = true;
            tr = transform;
            metr = Metronome.instance;

            Options op = Options.instance;
            positionSnapTime = op.positionSnapTime;
            rotationSnapTime = op.rotationSnapTime;
            maxSnapSpeed = op.maxSnapSpeed;
            close_threshold = op.close_threshold;
            gather_threshold = op.gather_threshold;
            magnet_threshold = op.magnet_threshold;
            magnet_velocity = op.magnet_velocity;
            fail_threshold = op.fail_threshold;
            ignore_distance = op.ignore_distance;
            ignore_angle = op.ignore_angle;
        }   

        state = ArcState.Normal;
        nextIndex = 0;

        foreach (var point in points)
        {
            point.Activate();
            initialPointsPositions.Add(point.GetLocalPosition());
        }
    }

    void FixedUpdate()
    {
        Vector3 relativePosition = tr.position - PlayerController.instance.tr.position;
        float distance = relativePosition.magnitude;
        float angle = Vector3.Angle(PlayerController.instance.direction, relativePosition.normalized);

        switch (state)
        {
            case ArcState.Gathered:
                GetBackToPool();
                break;
            case ArcState.Failed:
                GameManager.instance.GameOver();
                GetBackToPool();
                break;
            case ArcState.Ignored:
                GetBackToPool();
                break;
            case ArcState.Close:
                CheckPointsDistance();
                break;
            case ArcState.Snap:
                SnapUpdate(2);
                if (distance < close_threshold * metr.scale && angle < 10)
                {
                    state = ArcState.Close;
                }
                break;
            case ArcState.Normal:
                if (distance > ignore_distance && angle > ignore_angle)
                    state = ArcState.Ignored;
                if (!PlayerController.instance.targetArc && distance < 3 * metr.scale && angle < 40)
                {
                    state = ArcState.Snap;
                    PlayerController.instance.targetArc = this;
                }
                break;
        }
    }

    void GetBackToPool()
    {
        points[nextIndex].SetLocalPosition(initialPointsPositions[nextIndex]);
        Spawner.instance.arcs.Remove(this);
        EasyObjectPool.instance.ReturnObjectToPool(gameObject);
    }

    void SnapUpdate(int bar)
    {
        Vector3 target = PlayerController.instance.trajectExtrapolation[bar];
        tr.position = Vector3.SmoothDamp(tr.position, target, ref vel, positionSnapTime, maxSnapSpeed);

        Quaternion targetRotation = Quaternion.Euler(0, 0, PlayerController.instance.rb.rotation);
        tr.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.fixedDeltaTime / rotationSnapTime);
        
    }

    void CheckPointsDistance()
    {
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
            Score.instance.IncrementScore();
            points[nextIndex].SetLocalPosition(initialPointsPositions[nextIndex]);
            nextIndex++;
            if (nextIndex > points.Count - 1)
            {
                nextIndex = points.Count - 1;
                state = ArcState.Gathered;
                PlayerController.instance.targetArc = null;
            }
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

    public Bar GetBars(Direction direction, RelativeDirection rd, ref Direction endDirection)
    {
        Bar bar = new Bar();
        switch (direction)
        {
            case Direction.Right:
                switch (rd)
                {
                    case RelativeDirection.Forward:
                        bar.type = BarType.Line0;
                        bar.reverse = false;
                        endDirection = Direction.Right;
                        break;
                    case RelativeDirection.Clockwise:
                        bar.type = BarType.Circle1;
                        bar.reverse = true;
                        endDirection = Direction.Down;
                        break;
                    case RelativeDirection.Counter_Clockwise:
                        bar.type = BarType.Circle3;
                        bar.reverse = false;
                        endDirection = Direction.Up;
                        break;
                }
                break;
            case Direction.Left:
                switch (rd)
                {
                    case RelativeDirection.Forward:
                        bar.type = BarType.Line0;
                        bar.reverse = true;
                        endDirection = Direction.Left;
                        break;
                    case RelativeDirection.Clockwise:
                        bar.type = BarType.Circle3;
                        bar.reverse = true;
                        endDirection = Direction.Up;
                        break;
                    case RelativeDirection.Counter_Clockwise:
                        bar.type = BarType.Circle1;
                        bar.reverse = false;
                        endDirection = Direction.Down;
                        break;
                }
                break;
            case Direction.Up:
                switch (rd)
                {
                    case RelativeDirection.Forward:
                        bar.type = BarType.Line1;
                        bar.reverse = false;
                        endDirection = Direction.Up;
                        break;
                    case RelativeDirection.Clockwise:
                        bar.type = BarType.Circle0;
                        bar.reverse = true;
                        endDirection = Direction.Right;
                        break;
                    case RelativeDirection.Counter_Clockwise:
                        bar.type = BarType.Circle0;
                        bar.reverse = false;
                        endDirection = Direction.Left;
                        break;
                }
                break;
            case Direction.Down:
                switch (rd)
                {
                    case RelativeDirection.Forward:
                        bar.type = BarType.Line1;
                        bar.reverse = true;
                        endDirection = Direction.Down;
                        break;
                    case RelativeDirection.Clockwise:
                        bar.type = BarType.Circle2;
                        bar.reverse = true;
                        endDirection = Direction.Left;
                        break;
                    case RelativeDirection.Counter_Clockwise:
                        bar.type = BarType.Circle2;
                        bar.reverse = false;
                        endDirection = Direction.Right;
                        break;
                }
                break;
        }
        return bar;
    }

    enum ArcState { Gathered, Failed, Ignored, Close, Snap, Normal}
    public enum BarType { Circle0, Circle1, Circle2, Circle3, Line0, Line1 }

    public enum RelativeDirection { Forward, Clockwise, Counter_Clockwise }

    public enum Direction { Right, Left, Up, Down }

    [System.Serializable]
    public class Bar
    {
        public BarType type;
        public bool reverse;
    }
}
