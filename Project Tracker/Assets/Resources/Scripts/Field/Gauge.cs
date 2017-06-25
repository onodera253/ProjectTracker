using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Gauge : MonoBehaviour
{
  // HP状態
  public bool isHp = false;

  // 現在HP状態
  public bool isCurrentHp = false;

  // バッテリー状態
  public bool isBattery = false;

  // Slider
  private Slider slider;

  // playerScript
  private Player playerScr;

  // flashlightScript
  private Flashlight flashlightScr;


  // Use this for initialization
  private void Start ()
  {
    // Slider 取得
    slider = GetComponent<Slider>();

    // プレイヤー 取得
    GameObject player = GameObject.Find("Utc");

    // プレイヤーあり
    if (player)
    {
      // HP状態 or 現在HP状態
      if (isHp || isCurrentHp)
      {
        // playerScript 取得
        playerScr = player.GetComponent<Player>();
      }
      // バッテリー状態
      else if (isBattery)
      {
        // flashlightScript 取得
        flashlightScr = player.GetComponent<Flashlight>();
      }
    }
	}
	

	// Update is called once per frame
	private void Update ()
  {
    if (!slider)
      return;

    // スライダー 更新
    slider.value = GetGaugeValue();
  }


  // ゲージ値 取得
  private float GetGaugeValue()
  {
    // 値 設定
    float value = 0.0f;

    bool isHpType = (isHp || isCurrentHp) ? true : false;

    // HP関連状態
    if (isHpType && playerScr)
    {
      // HP状態
      if (isHp)
      {
        // ゲージ値 取得
        value = playerScr.GetHpRatio();
      }
      // 現在HP状態
      else if (isCurrentHp)
      {
        // ゲージ値 取得
        value = playerScr.GetCurrentHpRatio();
      }
    }
    // バッテリー状態
    else if (isBattery && flashlightScr)
    {
      // ゲージ値 取得
      value = flashlightScr.GetBatteryLevelRatio();
    }

    return value;
  }
}
