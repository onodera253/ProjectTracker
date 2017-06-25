using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(GameObject))]


public class Item : MonoBehaviour
{
  // 対象
  public GameObject target;

  // バッテリー状態
  public bool isBattery = false;

  // 食べ物状態
  public bool isFood = false;

  // 追加ポイント
  public float addPoint = 15.0f;

  // スポーン時間
  public float spawnSec = 180.0f;

  // Renderer
  private Renderer ren;

  // プレイヤースクリプト
  private Player playerScript;

  // 懐中電灯スクリプト
  private Flashlight flashlightScript;


	// Use this for initialization
	private void Start ()
  {
    if (!target)
      return;

    // Renderer 取得
    ren = GetComponent<Renderer>();

    // プレイヤースクリプト 取得
    playerScript = target.GetComponent<Player>();

    // 懐中電灯スクリプト 取得
    flashlightScript = target.GetComponent<Flashlight>();
	}
	

	// Update is called once per frame
	private void Update ()
  {
    
	}


  // 接触判定
  private void OnTriggerEnter(Collider other)
  {
    if (!target || !ren || !ren.enabled)
      return;

    // 対象と接触
    if (other.tag == target.transform.tag)
    {
      // リスポーン状態 設定
      bool isRespawn = false;

      // バッテリー状態
      if (isBattery && flashlightScript)
      {
        // バッテリー追加
        flashlightScript.AddBattery(addPoint);

        // リスポーン状態 更新
        isRespawn = true;
      }
      // 食べ物状態
      else if (isFood && playerScript)
      {
        // HP追加
        playerScript.AddHp(addPoint);

        // リスポーン状態 更新
        isRespawn = true;
      }

      // リスポーン状態
      if (isRespawn)
      {
        // リスポーン開始
        StartCoroutine(StartRespawn());
      }
    }
  }


  // リスポーン開始
  private IEnumerator StartRespawn()
  {
    if (!ren)
      yield break;

    // 表示切替
    ren.enabled = false;

    // スポーン時間 待機
    yield return new WaitForSeconds(spawnSec);

    // 表示切替
    ren.enabled = true;
  }
}
