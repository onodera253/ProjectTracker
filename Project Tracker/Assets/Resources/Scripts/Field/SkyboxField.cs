using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


public class SkyboxField : MonoBehaviour
{
  // 1日(時)
  const int DAY_HOUR = 24;

  // 円角度
  const int CIRCLE_ANGLE = 360;
  // const int CIRCLE_ANGLE = 1;

  // 1日の時間(秒)
  public float dayAsSec = 180.0f;

  // 秒角度
  private float secAngle = 1.0f;
  
  // 総合時間(秒)
  private float totalSec = 0.0f;


	// Use this for initialization
	private void Start ()
  {
    // 秒の角度 取得
    secAngle = dayAsSec / CIRCLE_ANGLE;
  }
	

	// Update is called once per frame
	private void Update ()
  {
    // 総合時間 更新
    totalSec += Time.deltaTime;

    // デルタ角 取得
    float deltaAngle = Time.deltaTime / secAngle;

    // 回転
    transform.rotation = transform.rotation * Quaternion.Euler(deltaAngle, 0, 0);
	}


  // 残り秒 取得
  public int GetRemainingSec(float limitHour)
  {
    int remainingSec = 0;

    // 制限(秒) 取得
    float limitSec = (limitHour / DAY_HOUR) * dayAsSec;

    // 残り秒 更新
    remainingSec = Mathf.FloorToInt(limitSec - totalSec);

    return remainingSec;
  }


  // 制限超過 判定
  public bool IsOverLimit(float limitHour)
  {
    // 残り秒 取得
    int remainingSec = GetRemainingSec(limitHour);

    // 制限超過状態 取得
    bool isOverLimit = (remainingSec <= 0) ? true : false;

    return isOverLimit;
  }
}
