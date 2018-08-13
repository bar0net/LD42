using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Treasure : MonoBehaviour {
    const int _SHAKES_ = 3;
    const float _SHAKE_TIME_ = 0.6f;
    const float _MAX_ANGLE_SHAKE_ = 5f;
    const float _OPEN_TIME_ = 0.2f;
    const string _TREASURE_TEXT_ = "You found {0}\n{1}";

    public GameObject item;
    public Button backButton;

    Overlord _o;
    bool animate = false;
    float shake_timer = 0;
    float open_timer = 0;

    private void Start()
    {
        item.SetActive(false);
    }

    private void Update()
    {
        if (!animate) return;

        if (shake_timer > 0)
        {
            shake_timer -= Time.deltaTime;

            if (shake_timer < 0)
            {
                shake_timer = 0;
                open_timer = _OPEN_TIME_;
            }
            this.transform.rotation = Quaternion.Euler(0,0,_MAX_ANGLE_SHAKE_ * Mathf.Sin(2 * Mathf.PI * shake_timer * _SHAKES_ / _SHAKE_TIME_) );
        }

        if (open_timer > 0)
        {
            open_timer -= Time.deltaTime;

            if (open_timer < 0)
            {
                item.SetActive(true);
                animate = false;
            }
        }

    }

    public void GetReward()
    {
        _o = Overlord._instance;

        if (_o.currentEvent == null)
        {
            _o.currentEvent = _o.GetRandomEvent(Overlord.EventTypes.TREASURE);
        }

        animate = true;
        shake_timer = _SHAKE_TIME_;
        GetComponent<Button>().interactable = false;

        Item i = _o.currentEvent.rewards[Random.Range(0, _o.currentEvent.rewards.Length)];
        Debug.Log(i.name);

        item.GetComponent<Image>().sprite = i.artwork;
        Text t = item.transform.Find("Text").GetComponent<Text>();

        t.text = string.Format(_TREASURE_TEXT_, i.name, i.description);
        if (_o.inventorySlots <= _o.inventory.Count) t.text += "\nYou can't carry more objects. Leaving this treasure behind drives you more insane.";

        _o.InventoryAdd(i);

    }

    public void LoadScene()
    {
        Overlord._instance.eventsCompleted += 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Map");
    }
}