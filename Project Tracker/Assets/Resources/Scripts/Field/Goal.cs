using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(GameObject))]


public class Goal:MonoBehaviour
{
  // 対象
  public GameObject target;

  // ゴール状態
  private bool isGoal = false;
  public bool IsGoal
  {
    get { return isGoal; }
    private set
    {
      if (value != isGoal)
      {
        isGoal = value;
      }
    }
  }


  // Use this for initialization
  private void Start()
  {

  }


  // Update is called once per frame
  private void Update()
  {

  }


  // 衝突判定
  private void OnTriggerEnter(Collider other)
  {
    if (!target)
      return;

    // 対象に接触
    if (other.gameObject.tag == target.tag)
    {
      // ゴール状態 更新
      isGoal = true;
    }
  }


}
