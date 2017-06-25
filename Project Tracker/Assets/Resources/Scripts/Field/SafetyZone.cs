using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SafetyZone : MonoBehaviour
{
  // 対象
  public GameObject target;

  // プレイヤースクリプト
  private Player playerScr;


	// Use this for initialization
	private void Start ()
  {
    if (!target)
      return;

    // プレイヤースクリプト 更新
    playerScr = target.GetComponent<Player>();
	}
	
	// Update is called once per frame
	private void Update ()
  {
		
	}


  // 接触 判定
  private void OnTriggerEnter(Collider other)
  {
    if (!target || !playerScr)
      return;

    // 対象と接触
    if (other.tag == target.tag)
    {
      // 潜伏 切替
      playerScr.ChangeHide(true);
    }
  }


  // 接触解除 判定
  private void OnTriggerExit(Collider other)
  {
    if (!target || !playerScr)
      return;

    // 対象と接触解除
    if (other.tag == target.tag)
    {
      // 潜伏 切替
      playerScr.ChangeHide(false);
    }
  }
}
