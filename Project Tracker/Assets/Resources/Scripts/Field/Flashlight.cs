using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(GameObject))]


public class Flashlight : MonoBehaviour
{
  // 懐中電灯
  public GameObject flashlight;

  // スポットライト
  public GameObject spotlight;

  // バッテリー最大時間
  public float batteryMaxSec = 50.0f;

  // 実行状態
  private bool isActive = false;

  // バッテリー時間
  private float batterySec = 0.0f;

  // 距離
  private Vector3 distance;


	// Use this for initialization
	private void Start ()
  {
    // バッテリー時間 取得
    batterySec = batteryMaxSec;

    // 懐中電灯 表示切替
    flashlight.gameObject.SetActive(isActive);

    // スポットライト 表示切替
    spotlight.gameObject.SetActive(isActive);

    // 距離 取得
    distance = flashlight.transform.position - transform.position;
  }


  // Update is called once per frame
  private void Update()
  {
    if (!flashlight || !flashlight || !isActive)
      return;

    // 懐中電灯 座標 更新
    flashlight.transform.position = transform.position + distance;

    // 懐中電灯 角度 更新
    flashlight.transform.rotation = transform.rotation;

    // バッテリー時間 更新
    batterySec -= Time.deltaTime;

    // バッテリー時間 0以下
    if (batterySec <= 0)
    {
      // バッテリー時間 初期化
      batterySec = 0.0f;

      // 実行状態 更新
      isActive = false;

      // 懐中電灯 表示切替
      flashlight.gameObject.SetActive(isActive);

      // スポットライト 表示切替
      spotlight.gameObject.SetActive(isActive);
    }
  }


  // スイッチ切替
  public void ChangeSwitch(bool active)
  {
    if (active == isActive)
      return;

    // 実行状態 更新
    isActive = (active && 0 < batterySec) ? true : false;
    
    // 懐中電灯 表示切替
    flashlight.gameObject.SetActive(isActive);

    // スポットライト 表示切替
    spotlight.gameObject.SetActive(isActive);
  }


  // バッテリー追加
  public void AddBattery(float addSec)
  {
    // 結果時間 取得
    float resultSec = batterySec + addSec;

    // 結果時間が最大時間 超過
    if (batteryMaxSec < resultSec)
    {
      // 結果時間 更新
      resultSec = batteryMaxSec;
    }

    // バッテリー時間 更新
    batterySec = resultSec;
  }


  // バッテリー残量比率 取得
  public float GetBatteryLevelRatio()
  {
    return batterySec / batteryMaxSec;
  }
}
