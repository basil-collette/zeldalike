using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;

public class FloatingJoyStick : MonoBehaviour
{
    public bool Floating = false;

    RectTransform RectTransform;
    [SerializeField] RectTransform Knob;
    public Vector2 MovementAmount;

    Finger MovementFinger;
    Vector2 startPos;
    float JoystickSize;
    float JoystickSizeHalf;
    float KnobSize;

    private void Start()
    {
        if (Floating)
        {
            RectTransform = GetComponent<RectTransform>();
            startPos = RectTransform.anchoredPosition;

            JoystickSize = RectTransform.rect.width;
            JoystickSizeHalf = JoystickSize / 2;
            KnobSize = Knob.rect.width;
        }
    }

    private void OnEnable()
    {
        if (Floating)
        {
            EnhancedTouchSupport.Enable();
            ETouch.Touch.onFingerDown += HandleFingerDown;
            ETouch.Touch.onFingerUp += HandleLoseFinger;
        }
    }

    private void OnDisable()
    {
        if (Floating)
        {
            ETouch.Touch.onFingerDown -= HandleFingerDown;
            ETouch.Touch.onFingerUp -= HandleLoseFinger;
            EnhancedTouchSupport.Disable();
        }
    }

    private void HandleFingerDown(Finger touchedFinger)
    {
        if (MovementFinger == null && touchedFinger.screenPosition.x < Screen.width / 3)
        {
            MovementFinger = touchedFinger;
            MovementAmount = Vector2.zero;

            GetComponent<Image>().color = new Color(1, 1, 1, 1);
            Knob.GetComponent<Image>().color = new Color(1, 1, 1, 1);

            RectTransform.anchoredPosition = ClampStartPosition(touchedFinger.screenPosition);
            //Knob.anchoredPosition = new Vector2(-(KnobSize / 2), -(KnobSize / 2));
        }
    }

    private void HandleLoseFinger(Finger lostFinger)
    {
        if (lostFinger == MovementFinger)
        {
            MovementFinger = null;
            MovementAmount = Vector2.zero;

            GetComponent<Image>().color = new Color(1, 1, 1, 0.7f);
            Knob.GetComponent<Image>().color = new Color(1, 1, 1, 0.6f);

            RectTransform.anchoredPosition = startPos;
            Knob.anchoredPosition = new Vector2(-(KnobSize / 2), -(KnobSize / 2));
        }
    }

    private Vector2 ClampStartPosition(Vector2 startPosition)
    {
        startPosition = new Vector2(startPosition.x - JoystickSizeHalf, startPosition.y - JoystickSizeHalf);

        if (startPosition.x < 0)
        {
            startPosition.x = 0;
        }

        if (startPosition.y < 0)
        {
            startPosition.y = 0;
        }
        else if (startPosition.y > Screen.height - JoystickSize)
        {
            startPosition.y = Screen.height - JoystickSize;
        }

        return startPosition;
    }

}