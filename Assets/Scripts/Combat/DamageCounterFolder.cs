
using UnityEngine;

public class DamageCounterFolder : MonoBehaviour
{
    //Verantwortlich für das Erschaffen von Damagecounters
    
    public GameObject damageCounterPrefab;
    
    public void SpawnDamageCounter(Vector3 position, int amount)
    {
        GameObject createdDamageCounter = Instantiate(damageCounterPrefab, position, Quaternion.identity,
            transform);
        createdDamageCounter.GetComponent<DamageCounter>().numberText.text = "-"+ amount.ToString();
    }
    
}
