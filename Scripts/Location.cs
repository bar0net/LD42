using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Location : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler{

    public Overlord.EventTypes locationEvent = Overlord.EventTypes.NONE;
    public GameObject tooltipPanel;
    public RectTransform tooltipPosition;
    public string description;

    private void Start()
    {
        tooltipPanel.SetActive(false);
    }

    public void SetEvent(Overlord.EventTypes type)
    {
        if (type == Overlord.EventTypes.NONE) return;

        locationEvent = type;
        GetComponent<Image>().color = new Color(1,1,0.4f);
        GetComponent<Shadow>().effectColor = Color.yellow;
    }

    public void OnPointerClick (PointerEventData e)
    {
        if (locationEvent == Overlord.EventTypes.NONE) return;

        FindObjectOfType<Overlord>().LoadEvent(locationEvent);

    }

    public void OnPointerEnter(PointerEventData e)
    {


        if (locationEvent == Overlord.EventTypes.NONE) return;

        tooltipPanel.SetActive(true);
        tooltipPanel.GetComponent<RectTransform>().position = tooltipPosition.position;
        tooltipPanel.GetComponentInChildren<Text>().text = description + "\n" + GetText();
        GetComponent<Image>().color = new Color(1, 1, 0);
    }

    public void OnPointerExit(PointerEventData e)
    {
        if (locationEvent == Overlord.EventTypes.NONE) return;
        tooltipPanel.SetActive(false);
        GetComponent<Image>().color = new Color(1, 1, 0.4f);
    }

    string GetText()
    {

        switch(locationEvent)
        {
            case Overlord.EventTypes.BOSS_FIGHT:
                return "There is an exceptionally strong opponent here. You better be ready!";
            case Overlord.EventTypes.MERCHANT:
                return "A traveler is resting here. They have some wares they are willing to exchange.";
            case Overlord.EventTypes.MINIBOSS_FIGHT:
                return "A lieutenant has come out! You can try to challenge him if you dare!";
            case Overlord.EventTypes.MISTERY:
                return "You have an odd feeling about this place.";
            case Overlord.EventTypes.SENTINEL_FIGHT:
                return "Some monsters are wandering around this location.";
            case Overlord.EventTypes.TREASURE:
                return "You recieve news about some valuables stashed here.";
        }

        return "";
    }
}
