using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FlagManager : MonoBehaviour
{
  // フラグ書庫
  public Dictionary<string, int> flagDic = new Dictionary<string, int>();


	// Use this for initialization
	private void Start ()
  {
		
	}
	

	// Update is called once per frame
	private void Update ()
  {

  }


  // フラグ 初期化
  public void InitFlag(string key)
  {
    if (flagDic == null || key == null)
      return;

    // キーなし
    if (!flagDic.ContainsKey(key))
    {
      // フラグ書庫 追加
      flagDic.Add(key, 0);
    }
  }


  // カウント 追加
  public void AddCount(string key, int value)
  {
    if (flagDic == null || key == null)
      return;

    // フラグ 初期化
    InitFlag(key);

    // フラグ書庫 更新
    flagDic[key] += value;
  }


  // カウント 削減
  public void RemoveCount(string key, int value)
  {
    if (flagDic == null || key == null)
      return;

    // フラグ 初期化
    InitFlag(key);

    // フラグ書庫 更新
    flagDic[key] -= value;
  }


  // カウント 取得
  public int GetCount(string key)
  {
    int count = 0;

    // フラグ書庫 & キー あり
    if (flagDic != null && key != null)
    {
      // キーあり
      if (flagDic.ContainsKey(key))
      {
        // カウント 更新
        count = flagDic[key];
      }
    }

    return count;
  }


  // カウント超過 判定
  public bool IsCountOver(string key, int limit)
  {
    bool isOver = false;

    // カウント 取得
    int count = GetCount(key);

    // カウントが限度 以上
    if (limit <= count)
    {
      // 超過状態 更新
      isOver = true;
    }

    return isOver;
  }


}
