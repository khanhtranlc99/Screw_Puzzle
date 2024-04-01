using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MoneyEffectController : MonoBehaviour
{
    private Vector3 posDestination_Enegry;
    private Vector3 posDestination_Coin;
    private Vector3 posDestination_Gem;

    [SerializeField] private MoneyEffect moneyEffectPrefab;
    private List<MoneyEffect> poolEffect;
    [SerializeField] private Transform parentPool;
    private List<MoneyEffect> poolEffect_UI;

    public void InitState(Vector3 posDestination_Enegry, Vector3 posDestination_Coin, Vector3 posDestination_Gem)
    {
        this.posDestination_Enegry = posDestination_Enegry;
        this.posDestination_Coin = posDestination_Coin;
        this.posDestination_Gem = posDestination_Gem;
    }

    public void SpawnEffect_GoDestination(Vector3 posSpawn, GiftType itemType, int value, UnityAction actionCome, Vector3 posCome, bool isFollowObject = false, GameObject objectFollow = null)
    {

        StartCoroutine(SpawnEffect_GoDestination_Handle(posSpawn, itemType, value, actionCome, posCome, isFollowObject, objectFollow));
       
    }

    public void SpawnEffect_GoDestination2(Vector3 posSpawn, GiftType itemType, int value, UnityAction actionCome, Vector3 posCome, bool isFollowObject = false, GameObject objectFollow = null, int NumSpawn = 1)
    {

        StartCoroutine(SpawnEffect_GoDestination_Handle(posSpawn, itemType, value, actionCome, posCome, isFollowObject, objectFollow, NumSpawn));

    }

    private IEnumerator SpawnEffect_GoDestination_Handle(Vector3 posSpawn, GiftType itemType, int value, UnityAction actionCome, Vector3 posCome, bool isFollowObject = false, GameObject objectFollow = null)
    {
        int radomNumSpawn = Random.Range(7, 10);

        for (int i = 0; i < radomNumSpawn; i++)
        {
            Vector3 m_posSpawn = new Vector3(posSpawn.x + Random.Range(0, 0.3f), posSpawn.y + Random.Range(0, 0.3f), posSpawn.z);
            MoneyEffect effect = GetPool(MoneyEffect.TypeMoveEffect.MoveToCome);
            effect.gameObject.SetActive(true);
            effect.transform.position = Camera.main.WorldToScreenPoint(m_posSpawn);
            effect.SetSpriteIcon(itemType);
            effect.SetUpMoveCome(posCome, itemType, value, actionCome, isFollowObject, objectFollow);
            yield return new WaitForSecondsRealtime(0.04f);
        }
    }

    private IEnumerator SpawnEffect_GoDestination_Handle(Vector3 posSpawn, GiftType itemType, int value, UnityAction actionCome, Vector3 posCome, bool isFollowObject = false, GameObject objectFollow = null, int radomNumSpawn = 1)
    {
        for (int i = 0; i < radomNumSpawn; i++)
        {
            Vector3 m_posSpawn = new Vector3(posSpawn.x + Random.Range(0, 0.3f), posSpawn.y + Random.Range(0, 0.3f), posSpawn.z);
            MoneyEffect effect = GetPool(MoneyEffect.TypeMoveEffect.MoveToCome);
            effect.gameObject.SetActive(true);
            effect.transform.position = Camera.main.WorldToScreenPoint(m_posSpawn);
            effect.SetSpriteIcon(itemType);
            if (i == 0)
            {
                effect.SetUpMoveCome(posCome, itemType, value, actionCome, isFollowObject, objectFollow);
            }
            else
            {
                effect.SetUpMoveCome(posCome, itemType, value, ()=> { }, isFollowObject, objectFollow);
            }
            yield return new WaitForSecondsRealtime(0.04f);
        }
    }


    public void SpawnEffect_FlyUp(Vector3 posSpawn, GiftType itemType, int value, Color colorText, bool isSpawnItemPlayer = false, bool isFollowObject = false, GameObject objectFollow = null)
    {
        MoneyEffect effect = GetPool(MoneyEffect.TypeMoveEffect.FlyUp);
        effect.gameObject.SetActive(true);

        //Vector3 m_posSpawn = new Vector3(posSpawn.x + Random.Range(0, 0.3f), posSpawn.y + Random.Range(0, 0.3f), posSpawn.z);
        effect.transform.position = Camera.main.WorldToScreenPoint(posSpawn);

        if (isSpawnItemPlayer)
        {
            effect.SetMoveFlyUpCoinPlayer(itemType, value, colorText, isFollowObject, objectFollow);
        }
        else
        {
            effect.SetMoveFlyUp(itemType, value, colorText, isFollowObject, objectFollow);
        }
    }

    public void SpawnEffectText_FlyUp(Vector3 posSpawn,  string value, Color colorText, bool isSpawnItemPlayer = false, bool isFollowObject = false, GameObject objectFollow = null)
    {
        MoneyEffect effect = GetPool(MoneyEffect.TypeMoveEffect.FlyUp);
        effect.gameObject.SetActive(true);

        //Vector3 m_posSpawn = new Vector3(posSpawn.x + Random.Range(0, 0.3f), posSpawn.y + Random.Range(0, 0.3f), posSpawn.z);
        effect.transform.position = Camera.main.WorldToScreenPoint(posSpawn);

        if (isSpawnItemPlayer)
        {
            effect.SetMoveTextFlyUpTypePlayer(value, colorText, isFollowObject, objectFollow);
        }
        else
        {
            effect.SetMoveTextFlyUp(value, colorText, isFollowObject, objectFollow);
        }
    }

    private MoneyEffect GetPool(MoneyEffect.TypeMoveEffect type)
    {
        if (poolEffect == null)
            poolEffect = new List<MoneyEffect>();
        for (int i = 0; i < poolEffect.Count; i++)
        {
            if (poolEffect[i].typeMoveEffect == type && !poolEffect[i].gameObject.activeSelf)
            {
                return poolEffect[i];
            }
        }

        var effect = Instantiate(moneyEffectPrefab, parentPool);
        effect.typeMoveEffect = type;
        poolEffect.Add(effect);
        return effect;
    }
    public void SpawnEffectText_FlyUp_UI(Transform paramParent, Vector3 posSpawn, string value, Color colorText, bool isSpawnItemPlayer = false, bool isFollowObject = false, GameObject objectFollow = null)
    {
        MoneyEffect effect = GetPool_UI(MoneyEffect.TypeMoveEffect.FlyUp);
        effect.gameObject.SetActive(true);

        //Vector3 m_posSpawn = new Vector3(posSpawn.x + Random.Range(0, 0.3f), posSpawn.y + Random.Range(0, 0.3f), posSpawn.z);
        //effect.transform.position = Camera.main.WorldToScreenPoint(posSpawn);
        effect.transform.parent = paramParent;
        effect.transform.position = posSpawn;
        if (isSpawnItemPlayer)
        {
            effect.SetMoveTextFlyUpTypePlayer(value, colorText, isFollowObject, objectFollow);
        }
        else
        {
            effect.SetMoveTextFlyUp(value, colorText, isFollowObject, objectFollow);
        }
    }
    private MoneyEffect GetPool_UI(MoneyEffect.TypeMoveEffect type)
    {
        if (poolEffect_UI == null)
            poolEffect_UI = new List<MoneyEffect>();
        try
        {
            for (int i = 0; i < poolEffect_UI.Count; i++)
            {
                if (poolEffect_UI[i].typeMoveEffect == type && !poolEffect_UI[i].gameObject.activeSelf)
                {
                    return poolEffect_UI[i];
                }
            }
        }
        catch
        {

            var effect1 = Instantiate(moneyEffectPrefab, parentPool);
            effect1.typeMoveEffect = type;
            poolEffect_UI.Add(effect1);
            return effect1;

        }



        var effect = Instantiate(moneyEffectPrefab, parentPool);
        effect.typeMoveEffect = type;
        poolEffect_UI.Add(effect);
        return effect;
    }
}
