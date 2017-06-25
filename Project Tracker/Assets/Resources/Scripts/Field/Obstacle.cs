using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Obstacle: MonoBehaviour
{
  // 定数
  private const string GAME_MANAGER = "GameManager";

  private const string TYPE_ACTIVE  = "active";   // 能動タイプ
  private const string TYPE_AUTO    = "auto";     // 受動タイプ
  private const string TYPE_CONSUME = "consume";  // 消費タイプ


  // キー
  public string key;

  // アンロック数
  public int unlockCount = 0;

  // 対象
  public GameObject target;

  // 消費状態
  public bool isConsume = false;

  // フラグマネージャー
  private FlagManager flagManager;


	// Use this for initialization
	private void Start ()
  {
    // ゲームマネージャー 取得
    GameObject gameManager = GameObject.Find(GAME_MANAGER);

    // フラグマネージャー 取得
    flagManager = (gameManager) ? gameManager.GetComponent<FlagManager>() : null;
  }
	

	// Update is called once per frame
	private void Update ()
  {
    // タイプ別
    switch (GetFlagType())
    {
      // 受動タイプ
      case TYPE_AUTO:
        // アクティブ状態 更新
        IsActiveUpdate();
        break;
    }
  }


  // 接触 判定
  private void OnTriggerEnter(Collider other)
  {
    if (!target)
      return;

    // 対象と接触
    if (other.tag == target.tag)
    {
        // タイプ別
        switch (GetFlagType())
      {
        // 能動タイプ
        case TYPE_ACTIVE:
          // アクティブ状態 更新
          IsActiveUpdate();
          break;

        // 消費タイプ
        case TYPE_CONSUME:
          // アクティブ状態 更新
          IsActiveUpdate(unlockCount);
          break;
      }
    }
  }


  // フラグタイプ 取得
  private string GetFlagType()
  {
    // タイプ 設定
    string flagType = TYPE_AUTO;

    // 対象あり
    if (target)
    {
      // 消費状態
      if (isConsume)
      {
        // フラグタイプ 更新
        flagType = TYPE_CONSUME;
      }
      // その他
      else
      {
        // フラグタイプ 更新
        flagType = TYPE_ACTIVE;
      }
    }

    return flagType;
  }


  // アクティブ状態 更新
  private void IsActiveUpdate(int removeCount = 0)
  {
    if (!flagManager)
      return;

    // アンロック数 以上
    if (flagManager.IsCountOver(key, unlockCount))
    {
      // カウント 削減
      flagManager.RemoveCount(key, removeCount);

      // 表示 切替
      gameObject.SetActive(false);
    }
  }


}
