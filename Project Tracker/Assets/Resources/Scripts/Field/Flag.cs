using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Flag:MonoBehaviour
{
  // 定数
  private const string GAME_MANAGER = "GameManager";

  // キー
  public string key;

  // 数量
  public int count = 0;

  // 対象
  public GameObject target;

  // フラグマネージャー
  private FlagManager flagManager;


  // Use this for initialization
  private void Start()
  {
    // ゲームマネージャー 取得
    GameObject gameManager = GameObject.Find(GAME_MANAGER);

    // フラグマネージャー 取得
    flagManager = (gameManager) ? gameManager.GetComponent<FlagManager>() : null;

    // フラグ 初期化
    InitFlag();
  }


  // Update is called once per frame
  private void Update()
  {

  }


  // 接触 判定
  private void OnTriggerEnter(Collider other)
  {
    if (!target || !flagManager || key == null)
      return;

    // タグ 一致
    if (other.tag == target.tag)
    {
      // カウント 追加
      flagManager.AddCount(key, count);

      gameObject.SetActive(false);
    }
  }


  // フラグ 初期化
  private void InitFlag()
  {
    if (!flagManager || key == null)
      return;

    // フラグ 初期化
    flagManager.InitFlag(key);
  }


}