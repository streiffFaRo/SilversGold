using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class AttackButtonPreview : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //Verantwortlich für die Schadensvorschau des Angriffs
    
    [Header("General")]
    [SerializeField] private CardManager myCardManager;
    [SerializeField] private GameObject damagePreviewTokenPrefab;
    [SerializeField] private DamageCounterFolder damageCounterFolder;

    //Pritavte Variablen
    private GameObject playerDamagePreviewToken;
    private GameObject enemyDamagePreviewToken;
    private CardManager cardAttacked = null;
    private int attackDamage;
    private EnemyManager enemyManager;

    private void Start()
    {
        attackDamage = myCardManager.cardStats.attack;
        damageCounterFolder = FindObjectOfType<DamageCounterFolder>();
        enemyManager = FindObjectOfType<EnemyManager>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        CalculateDamage();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HideAttackPreview();
    }

    private void CalculateDamage()
    {
        if (attackDamage > 0)
        {
            if (myCardManager.cardStats.position == "I")
            {
                if (myCardManager.cardIngameSlot.enemyInfantryLine.currentCard != null)
                {
                    cardAttacked = myCardManager.cardIngameSlot.enemyInfantryLine.currentCard;
                }
                else if (myCardManager.cardIngameSlot.enemyArtilleryLine.currentCard != null)
                {
                    cardAttacked = myCardManager.cardIngameSlot.enemyArtilleryLine.currentCard;
                }
            }
            else if (myCardManager.cardStats.position == "A")
            {
                if (myCardManager.cardIngameSlot.enemyArtilleryLine.currentCard != null)
                {
                    cardAttacked = myCardManager.cardIngameSlot.enemyArtilleryLine.currentCard;
                }
            }
            ShowAttackPreview();
        }
    }
    
    private void ShowAttackPreview()
    {

        if (cardAttacked != null)
        { 
            playerDamagePreviewToken = Instantiate(damagePreviewTokenPrefab, cardAttacked.cardDisplay.inGameArtworkImage.rectTransform.position + new Vector3(0,-40,0), Quaternion.identity, damageCounterFolder.transform);
            enemyDamagePreviewToken = Instantiate(damagePreviewTokenPrefab, myCardManager.cardDisplay.inGameArtworkImage.rectTransform.position + new Vector3(0,-40,0), Quaternion.identity, damageCounterFolder.transform);
            enemyDamagePreviewToken.GetComponentInChildren<TextMeshProUGUI>().text = cardAttacked.cardStats.attack.ToString();
        }
        else
        {
            playerDamagePreviewToken = Instantiate(damagePreviewTokenPrefab, enemyManager.enemyHealthText.rectTransform.position + new Vector3(75,-75,0), Quaternion.identity, damageCounterFolder.transform);
            enemyDamagePreviewToken = null;
        }
        
        playerDamagePreviewToken.GetComponentInChildren<TextMeshProUGUI>().text = attackDamage.ToString();
        cardAttacked = null;
        
        
    }

    public void HideAttackPreview()
    {
        Destroy(playerDamagePreviewToken);
        if (enemyDamagePreviewToken != null)
        {
            Destroy(enemyDamagePreviewToken);
        }
    }
}
