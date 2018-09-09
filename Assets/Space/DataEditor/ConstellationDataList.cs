using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstellationDataList : MonoBehaviour
{
    [SerializeField]
    private List<Transform> mDatas;

    public List<Transform> Datas { get { return mDatas; } }
    
}
