using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item", order = 1)]
public class ItemSO : ScriptableObject
{
    [field: HideInInspector] 
    [field: SerializeField]
    public int Id { get; private set; }

    [field: SerializeField]
    public string ItemName { get; private set; }

    [field: SerializeField]
    public Sprite ItemSprite { get; private set; }


    private void OnValidate()
    {
        var items = Resources.FindObjectsOfTypeAll<ItemSO>()
           .ToList();

        //Check id exist
        if (Id != 0)
        {
            //Copy-Paste duplicate check
            var duplicateCount = items.Count(item => item.Id == Id);

            if (duplicateCount <= 1)
            {
                return;
            }

            var duplicates = items.FindAll(item => item.Id == Id);
            if (this == duplicates[0])
            {
                return;
            }
        }

        //assign new ID
        var uniqueItems = new HashSet<int>(items.Select(item => item.Id));

        int randId;
        do
        {
            randId = Random.Range(int.MinValue, int.MaxValue);
        } while (randId == 0 || uniqueItems.Contains(randId));

        Id = randId;
    }
}
