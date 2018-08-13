using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Location : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler{

    public Overlord.EventData locationEvent;
    public GameObject tooltipPanel;
    public RectTransform tooltipPosition;
    public string description;

    private void Awake()
    {
        locationEvent = null;
    }

    private void Start()
    {
        tooltipPanel.SetActive(false);
    }

    public void SetEvent(Overlord.EventData data)
    {
        if (data == null) return;

        locationEvent = data;
        GetComponent<Image>().color = new Color(1,1,0.4f);
        GetComponent<Shadow>().effectColor = Color.yellow;
    }

    public void OnPointerClick (PointerEventData e)
    {
        if (locationEvent == null) return;

        FindObjectOfType<Overlord>().LoadEvent(locationEvent);

    }

    public void OnPointerEnter(PointerEventData e)
    {
        if (locationEvent == null) return;

        tooltipPanel.SetActive(true);
        tooltipPanel.GetComponent<RectTransform>().position = tooltipPosition.position;
        tooltipPanel.GetComponentInChildren<Text>().text = description + "\n" + GetText();
        GetComponent<Image>().color = new Color(1, 1, 0);

        RectTransform rt = tooltipPanel.transform.Find("Rewards").GetComponent<RectTransform>();
        if (rt == null) return;

        for (int i = rt.childCount - 1; i >= 0; i--)
        {
            Transform t = rt.GetChild(i);
            t.SetParent(null);
            Destroy(t.gameObject);
        }

        foreach (Item item in locationEvent.rewards) 
        {
            GameObject go = new GameObject();
            go.transform.SetParent(rt);
            go.transform.localScale = Vector3.one;

            Image img = go.AddComponent<Image>();
            img.sprite = item.artwork;
            img.preserveAspect = true;
        }
    }

    public void OnPointerExit(PointerEventData e)
    {
        if (locationEvent == null) return;
        tooltipPanel.SetActive(false);
        GetComponent<Image>().color = new Color(1, 1, 0.4f);
    }

    string GetText()
    {

        switch(locationEvent.type)
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
