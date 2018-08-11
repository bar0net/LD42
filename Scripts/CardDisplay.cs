using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    const float _UP_DST_ = 16.0f;
    const float _MOVE_SPEED_ = 1000f;

    public Card card;
    public Vector3 targetPos;
    public bool moving = true;

    Vector3 pos;
    RectTransform _rt;

	// Use this for initialization
	void Start () {
        _rt = this.GetComponent<RectTransform>();
        this.GetComponent<Image>().sprite = card.artwork;

        pos = targetPos;
	}

    void Update()
    {
        // If tha card has to go somewhere, move towards the destination
        if (moving)
        {
            Vector3 v = (targetPos - _rt.localPosition);
            float dst = v.magnitude;

            // If the card overshoots the position, stay on position and end movement
            if (dst - _MOVE_SPEED_ * Time.deltaTime < 0)
            {
                _rt.localPosition = targetPos;
                moving = false;
            }
            else _rt.localPosition += v.normalized * _MOVE_SPEED_ * Time.deltaTime;
        }
    }

    // Card move up when hovered
    public void OnPointerEnter(PointerEventData e)
    {
        targetPos = pos + Vector3.up * _UP_DST_;
        moving = true;
    }
    
    public void OnPointerExit(PointerEventData e)
    {
        targetPos = pos;
        moving = true;
    }
}
