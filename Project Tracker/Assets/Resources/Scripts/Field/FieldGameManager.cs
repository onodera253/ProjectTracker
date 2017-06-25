using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;
using Novel;


public class FieldGameManager : MonoBehaviour
{

  // GameObject定数
  const string GOAL = "Misaki";
  const string PLAYER = "Utc";
  const string LIGHT = "SunLight";

  // シーン定数
  const string GAME_CLEAR = "GameClear";
  const string GAME_OVER = "GameOver";

  // 操作定数
  const string HORIZONTAL = "Horizontal";
  const string VERTICAL = "Vertical";
  const string RUN = "Run";
  const string FLASH = "Flash";

  // 制限時間
  public float limitHour = 24.0f;

  // Text
  public Text textTime;

  // script
  private Goal goalScr;
  private Player playerScr;
  private SkyboxField skyboxScr;

  // 移動座標
  private float h = 0.0f;
  private float v = 0.0f;


	// Use this for initialization
	private void Start ()
  {
    // GameObject 取得
    GameObject goal = GameObject.Find(GOAL);
    GameObject player = GameObject.Find(PLAYER);
    GameObject light = GameObject.Find(LIGHT);

    // script 取得
    goalScr = (goal) ? goal.GetComponent<Goal>() : null;
    playerScr = (player) ? player.GetComponent<Player>() : null;
    skyboxScr = (light) ? light.GetComponent<SkyboxField>() : null;
	}

	
	// Update is called once per frame
	private void Update ()
  {
    if (!goalScr || !playerScr || !skyboxScr)
      return;

    // 移動座標 取得
    h = Input.GetAxisRaw(HORIZONTAL);
    v = Input.GetAxisRaw(VERTICAL);

    // 移動座標なし
    if (h == 0 && v == 0)
    {
      // 移動座標 更新
      h = CrossPlatformInputManager.GetAxisRaw(HORIZONTAL);
      v = CrossPlatformInputManager.GetAxisRaw(VERTICAL);
    }

    // 入力移動座標 設定
    playerScr.SetInputAxisRaw(h, v);

    // 走行 ボタン入力
    if (CrossPlatformInputManager.GetButtonDown(RUN) || Input.GetButtonDown("Jump"))
    {
      // 入力走行状態 設定
      playerScr.SetInputIsRun();
    }

    // 懐中電灯 ボタン入力
    if (CrossPlatformInputManager.GetButtonDown(FLASH) || Input.GetMouseButtonDown(2))
    {
      // 入力懐中電灯状態 設定
      playerScr.SetInputIsFlashlight();
    }

    // ゴール
    if (goalScr.IsGoal)
    {
      // ノベル「エピローグ」 遷移
      NovelSingleton.StatusManager.callJoker("wide/epilogue", "");
    }
    // 死亡 or 制限超過
    else if (playerScr.IsDead() || skyboxScr.IsOverLimit(limitHour))
    {
      // シーン遷移
      SceneManager.LoadScene(GAME_OVER);
    }
	}


  private void FixedUpdate()
  {
    if (!textTime || !skyboxScr)
      return;

    // 残り秒 取得
    int remainingSec = skyboxScr.GetRemainingSec(limitHour);

    // 分 取得
    int min = Mathf.FloorToInt(remainingSec / 60);

    // 秒 取得
    int sec = remainingSec % 60;

    // Text 更新
    textTime.text = GetTimeText(min, sec);
  }


  // 時間テキスト 取得
  private string GetTimeText(int min, int sec)
  {
    // 分テキスト 設定
    string minText = GetDigitAlignmentNumText(min, 2);

    // 秒テキスト 設定
    string secText = GetDigitAlignmentNumText(sec, 2);

    return minText + "." + secText;
  }


  // 桁合わせ数値テキスト 取得
  private string GetDigitAlignmentNumText(int num, int digit)
  {
    // 数値テキスト 設定
    string numText = num.ToString();

    // 不足桁数 取得
    int lackDigit = digit - numText.Length;

    // 不足桁数が0 超過
    if (0 < lackDigit)
    {
      for (int i = 0; i < lackDigit; i ++)
      {
        // 数値テキスト 更新
        numText = "0" + numText;
      }
    }

    return numText;
  }

}
