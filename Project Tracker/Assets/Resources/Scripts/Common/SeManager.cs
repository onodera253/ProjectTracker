using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SeManager : MonoBehaviour
{
  // SE書庫
  private Dictionary<string, AudioSource> seDic = new Dictionary<string, AudioSource>();

  // 現在SE
  private AudioSource currentSe;


	// Use this for initialization
	private void Start ()
  {

  }
	

	// Update is called once per frame
	private void Update ()
  {
		
	}


  // SE 追加
  public void AddSe(string key, AudioSource se)
  {
    if (seDic == null || !se)
      return;

    // SE
    AudioSource registSe = GetSe(key);

    // 登録済
    if (registSe)
    {
      // SE 上書き
      seDic[key] = se;
    }
    // 未登録
    else
    {
      // SE 追加
      seDic.Add(key, se);
    }
  }


  // SE書庫 追加
  public void AddSeDic(Dictionary<string, AudioSource> dic)
  {
    if (seDic == null || dic == null)
      return;

    foreach (KeyValuePair<string, AudioSource> pair in dic)
    {
      // SE 追加
      AddSe(pair.Key, pair.Value);
    }
  }


  // SE 再生
  public void PlaySe(string key)
  {
    // SE 取得
    AudioSource se = GetSe(key);

    if (!se)
      return;

    // 現在SEとSE 不一致 or ループでない
    if (se != currentSe || !se.loop)
    {
      // 現在SE 停止
      StopCurrentSe();

      // 現在SE 更新
      currentSe = se;

      // 現在SE 再生
      currentSe.Play();
    }
  }


  // 現在SE 停止
  public void StopCurrentSe()
  {
    if (seDic == null || !currentSe)
      return;

    // 現在SE 停止
    currentSe.Stop();

    // 現在SE 初期化
    currentSe = null;
  }


  // SE 全停止
  public void StopAllSe()
  {
    if (seDic == null)
      return;

    // 現在SE 停止
    StopCurrentSe();

    foreach (AudioSource value in seDic.Values)
    {
      // SE 停止
      value.Stop();
    }
  }


  // SE書庫 初期化
  public void InitSeDic()
  {
    // SE書庫 初期化
    seDic = new Dictionary<string, AudioSource>();
  }


  // SE 取得
  private AudioSource GetSe(string key)
  {
    // 戻り値
    AudioSource se = null;

    if (seDic != null && key != null)
    {
      // キーあり
      if (seDic.ContainsKey(key))
      {
        // SE 取得
        se = seDic[key];
      }
    }

    return se;
  }


}
