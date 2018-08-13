using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    const float _UP_DST_ = 16.0f;
    const float _MOVE_SPEED_ = 1000f;

    [Header("Functionality")]
    public Card card;
    public Vector3 targetPos;
    public bool moving = true;
    
    [Header("Appearance")]
    public Color regularColor = new Color(0.9f, 0.9f, 0.9f);
    public Color highlightColor = Color.white;
    public Color selectedColor = new Color(1, 1, 0.9f);

    [HideInInspector]
    public bool selected = false;

    Vector3 pos;
    RectTransform _rt;
    Manager _m;
    Image _img;

	// Use this for initialization
	void Start () {
        _m = FindObjectOfType<Manager>();
        _rt = this.GetComponent<RectTransform>();
        _img = this.GetComponent<Image>();

        _img.sprite = card.artwork;
        _img.color = regularColor;

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
        if (selected) return;

        _img.color = highlightColor;
        targetPos = pos + Vector3.up * _UP_DST_;
        moving = true;
    }
    
    public void OnPointerExit(PointerEventData e)
    {
        if (selected) return;

        _img.color = regularColor;
        targetPos = pos;
        moving = true;
    }

    public void OnPointerClick(PointerEventData e)
    {
        selected = !selected;

        if (selected)
        {
            _img.color = selectedColor;

            _m.SelectedTokenAdd(card.tokenValues);
        }
        else
        {
            _img.color = highlightColor;

            _m.SelectedTokenRemove(card.tokenValues);
        }
    }
}
