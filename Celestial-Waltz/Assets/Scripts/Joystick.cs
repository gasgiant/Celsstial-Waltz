using UnityEngine;
using UnityEngine.UI;

public class Joystick : MonoBehaviour {

    public RectTransform basePanel;
    public RectTransform stick;
    public Canvas scaler;

    public float dampTime = 0.1f;
    Vector3 vel;

    public Vector2 direction;
    public bool touched;

    float size;

    void Start()
    {
        size = basePanel.rect.height * 0.95f * scaler.scaleFactor / 2;
    }

    void Update()
    {
        if (Options.instance.touchControl)
        {
            if (Input.touchCount > 0)
            {
                Vector2 touchPos = Input.touches[0].position;
                direction = touchPos - (Vector2)basePanel.position;
                stick.position = basePanel.position + Vector3.ClampMagnitude(direction, size);
                direction.Normalize();
                touched = true;
            }
            else
            {
                touched = false;
                stick.position = Vector3.SmoothDamp(stick.position, basePanel.position, ref vel, dampTime);
            }
                
        }
        else
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                Vector2 mousePos = Input.mousePosition;
                direction = mousePos - (Vector2)basePanel.position;
                stick.position = basePanel.position + Vector3.ClampMagnitude(direction, size);
                direction.Normalize();
                touched = true;
            }
            else
            {
                touched = false;
                stick.position = Vector3.SmoothDamp(stick.position, basePanel.position, ref vel, dampTime);
            }
                
            
        }
    }
}
