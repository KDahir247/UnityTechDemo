using System.Collections;
using System.Collections.Generic;
using Tech.Core;
using Tech.DB;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.ResourceManagement.ResourceProviders;

public class test : MonoBehaviour
{
    List<GameObject> g = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
      var possessedUnit =  TechDB.LoadDataBase(FileDestination.UserPath).UserTable.FindByLevel(0).PossessedUnit;
        AssetAddress.LoadByNameOrLabel<GameObject>(possessedUnit[0].Address, g,
            new InstantiationParameters(new Vector3(-1.45f,0, 10.27f), Quaternion.Euler(0,190,0), null)).Forget();

        foreach (var o in TechDB.LoadDataBase(FileDestination.UserPath).UserTable.All)
        {
            Debug.Log(o.PossessedUnit[0].Name);
        }
    }

}
