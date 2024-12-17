using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class BroadsideButtonDamagePreview : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //Verantwortlich für die Schadensvorschau der Breitseite
    
    [Header("General")]
    [SerializeField] private GameObject damagePreviewTokenPrefab;
    [SerializeField] private DamageCounterFolder damageCounterFolder;

    //Pritavte Variablen
    private List<CardManager> playerArtyCards = new List<CardManager>();
    private List<GameObject> damagePreviewTokens = new List<GameObject>();
    private List<GameObject> directShipAttacker = new List<GameObject>();
    private GameObject damagePreviewToken;
    private CardManager cannoneerAttacked = null;
    private EnemyManager enemyManager;
    private int currentCannonLevel;

    private void Start()
    {
        damageCounterFolder = FindObjectOfType<DamageCounterFolder>();
        enemyManager = FindObjectOfType<EnemyManager>();
    }
    
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        SearchCannoneersInPlay();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HideBroadsidePreview();
    }

    private void SearchCannoneersInPlay()
    {
        damagePreviewTokens.Clear();
        directShipAttacker.Clear();
        playerArtyCards.Clear();
        
        currentCannonLevel = GameManager.instance.shipCannonLevel + 1;
        
        foreach (CardManager card in FindObjectsOfType<CardManager>())
        {
            if (card.owner == Owner.PLAYER && card.currentCardMode == CardMode.INPLAY && !card.cardActed && card.cardStats.keyWordCannoneer)
            {
                playerArtyCards.Add(card);
            }
        }

        if (playerArtyCards.Count > 0)
        {
            foreach (CardManager card in playerArtyCards)
            { 
                CalculateBroadsideDamage(card);
            }
        }
    }

    private void CalculateBroadsideDamage(CardManager card)
    {
        cannoneerAttacked = null;
        
        if (card.cardIngameSlot.enemyArtilleryLine.currentCard != null)
        { 
            cannoneerAttacked = card.cardIngameSlot.enemyArtilleryLine.currentCard;
        }

        if (cannoneerAttacked != null)
        {
            damagePreviewToken = Instantiate(damagePreviewTokenPrefab, cannoneerAttacked.cardDisplay.inGameArtworkImage.rectTransform.position + new Vector3(0,-40,0), Quaternion.identity, damageCounterFolder.transform);
        }
        else
        {
            damagePreviewToken = Instantiate(damagePreviewTokenPrefab, enemyManager.enemyHealthText.rectTransform.position + new Vector3(75,-75,0), Quaternion.identity, damageCounterFolder.transform);
            directShipAttacker.Add(damagePreviewToken);
            currentCannonLevel *= directShipAttacker.Count;
        }

        damagePreviewToken.GetComponentInChildren<TextMeshProUGUI>().text = currentCannonLevel.ToString();
        damagePreviewTokens.Add(damagePreviewToken);
        currentCannonLevel = GameManager.instance.shipCannonLevel + 1;
    }

    public void HideBroadsidePreview()
    {
        foreach (GameObject token in damagePreviewTokens)
        {
            Destroy(token);
        }
        damagePreviewTokens.Clear();
        directShipAttacker.Clear();
        playerArtyCards.Clear();
    }
}
