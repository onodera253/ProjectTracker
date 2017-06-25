using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(GameObject))]


public class FollowCamera : MonoBehaviour
{
  // 対象
  public GameObject target;

  // 対象距離
  private Vector3 targetDistance;

  // 振動時間
  private float shakeSec = 0.0f;

  // 振動幅
  private float shakeRange = 0.0f;

  // 開始座標
  private Vector3 startPos;


	// Use this for initialization
	private void Start ()
  {
    if (!target)
      return;

    // 対象座標 取得
    targetDistance = transform.position - target.transform.position;
  }
	

	// Update is called once per frame
	private void Update ()
  {
    if (!target || 0 < shakeSec)
      return;

    // カメラ座標 設定
    Vector3 camPos = target.transform.position + targetDistance;

    // カメラ座標と座標 不一致
    if (camPos != transform.position)
    {
      // 座標 更新
      transform.position = target.transform.position + targetDistance;
    }
  }


  private void FixedUpdate()
  {
    if (!target || shakeSec < 0)
      return;

    // 振動時間0 超過
    if (0 < shakeSec)
    {
      // 振動座標 取得
      float shakeX = Random.Range(shakeRange * -1, shakeRange);
      float shakeZ = Random.Range(shakeRange * -1, shakeRange);

      // 座標 更新
      transform.position = target.transform.position + targetDistance + new Vector3(shakeX, 0.0f, shakeZ);

      // 振動時間 更新
      shakeSec -= Time.deltaTime;
    }
  }


  // 振動開始
  public void StartShake(float sec, float range = 0.1f)
  {
    // 振動時間 更新
    shakeSec = sec;

    // 振動幅 更新
    shakeRange = range;

    // 開始座標 更新
    startPos = transform.position;
  }
}
