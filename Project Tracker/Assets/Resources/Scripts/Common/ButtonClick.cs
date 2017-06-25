using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Novel;


public class ButtonClick:MonoBehaviour
{


  // Use this for initialization
  private void Start()
  {

  }


  // Update is called once per frame
  private void Update()
  {

  }


  // タイトル クリック
  public void ClickToTitle()
  {
    // シーン遷移
    SceneManager.LoadScene("Title");
  }

  // フィールド クリック
  public void ClickToField()
  {
    // シーン遷移
    SceneManager.LoadScene("Field");
  }

  // プロローグ クリック
  public void ClickToPrologue()
  {
    // ノベル「プロローグ」 遷移
    NovelSingleton.StatusManager.callJoker("wide/prologue", "");
  }
}
