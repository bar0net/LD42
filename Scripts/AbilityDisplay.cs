using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AbilityDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler  {

    public Ability h;

    Manager _m;
    Button _b;

	// Use this for initialization
	void Start () {
        _m = FindObjectOfType<Manager>();
        _b = this.GetComponent<Button>();
        
        _b.interactable = false;
        this.transform.Find("Ability Name").GetComponent<Text>().text = h.name;

        Transform t = this.transform.Find("Cost Layout");
        for (int j = 0; j < h.tokenCost.Length; j++) PlaceTokens(h.tokenCost[j], t);
    }

    // Places the cost tokens for an ability with the appropiate color
    void PlaceTokens(Collection.TokenValue tokenPair, Transform parent)
    {
        for (int j = 0; j < tokenPair.value; j++)
        {
            GameObject token = Instantiate(_m.costToken);
            token.transform.SetParent(parent);
            token.transform.localScale = Vector3.one;

            if (_m.tokenColorsDict.ContainsKey(tokenPair.type)) token.GetComponent<Image>().color = _m.tokenColorsDict[tokenPair.type];
            else token.GetComponent<Image>().color = Color.black;
        }
    }

    public void Refresh(bool interactable)
    {
        _b.interactable = interactable;
    }

    public void Activate()
    {
        _m.player.ActivateAbility(h, _m.target);
        _m.DiscardSelected();
    }

    public void OnPointerEnter(PointerEventData e)
    {
        _m.abilityTooltipDescription.text = h.description;

        _m.abilityTooltipDamage.gameObject.SetActive(h.damage > 0);
        _m.abilityTooltipDamage.text = "Damage: " + _m.player.GetDamage(h).ToString();

        _m.abilityTooltipMagic.gameObject.SetActive(h.magic > 0);
        _m.abilityTooltipMagic.text = "Magic: " + _m.player.GetMagic(h).ToString();

        _m.abilityTooltipShield.gameObject.SetActive(h.defense > 0);
        _m.abilityTooltipShield.text = "Shield: " + _m.player.GetShield(h).ToString();

        _m.abilityTooltipPanel.SetActive(true);
    }

    public void OnPointerExit(PointerEventData e)
    {
        _m.abilityTooltipPanel.SetActive(false);
    }
}
