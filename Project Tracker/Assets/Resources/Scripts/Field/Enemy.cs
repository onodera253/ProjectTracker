using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(GameObject))]
[RequireComponent(typeof(AudioSource))]


public class Enemy : MonoBehaviour
{
  // 状態定数
  public const string STATE_ATTACK  = "stateAttack";  // 攻撃
  public const string STATE_IDLE    = "stateIdle";    // 待機
  public const string STATE_PURSUIT = "statePursuit"; // 追跡
  public const string STATE_SEARCH  = "stateSearch";  // 索敵
  public const string STATE_SLEEP   = "stateSleep";   // 睡眠
  public const string STATE_TRANCE  = "stateTrance";  // 夢中

  // オブジェクト名定数
  private const string PLAYER     = "Utc";        // プレイヤー
  private const string FLASHLIGHT = "Flashlight"; // 懐中電灯
  private const string RESPAWN    = "Respawn";    // リスポーン

  // 時間定数
  private const float RESPAWN_SEC = 5.0f;

  // 距離
  public float attackDistance   = 1.0f;   // 攻撃
  public float pursuitDistance  = 5.0f;   // 追跡
  public float viewDistance     = 8.0f;  // 視覚
  public float hearDistance     = 8.0f;  // 聴覚

  // 視野角
  public float viewAngle = 45.0f;

  // 聴力
  public float earLevel = 3.0f;

  // 物理攻撃力
  public float phiysicalPower = 5.0f;

  // 精神攻撃力
  public float mentalPower = 5.0f;

  // 移動速度
  public float moveSpeed = 4.0f;

  // 夢中時間
  public float tranceSec = 5.0f;

  // 睡眠時間
  public float sleepSec = 3.0f;

  // 硬直状態
  public bool IsFreeze
  {
    get
    {
      // 硬直状態 設定
      bool isFreeze = false;

      // 夢中状態 or 睡眠状態
      if (isTrance || isSleep)
      {
        // 硬直状態 更新
        isFreeze = true;
      }

      return isFreeze;
    }
  }

  // 発見状態
  public bool IsDiscover
  {
    get
    {
      // 発見状態 設定
      bool isDiscover = false;

      // 現在状態
      switch (currentState)
      {
        // 索敵
        case STATE_SEARCH:
          // プレイヤー & プレイヤーScriptあり
          if (player && playerScr)
          {
            // プレイヤー座標 取得
            Vector3 playerPos = PlayerPos;

            // 自座標 取得
            Vector3 selfPos = transform.position;

            // プレイヤーとの距離 取得
            float distance = Vector3.Distance(playerPos, selfPos);

            // 距離が視覚距離 以下
            if (distance <= viewDistance)
            {
              // 自視線 取得
              Vector3 selfDir = transform.forward;

              // プレイヤーが視野角 以内 & 非潜伏状態
              if (Vector3.Angle((playerPos - selfPos).normalized, selfDir) <= viewAngle && !playerScr.IsHide)
              {
                // 発見状態 更新
                isDiscover = true;
              }
            }

            // 距離が聴覚距離 以下
            if (distance <= hearDistance)
            {
              // 対象の騒音が聴力 以上
              if (earLevel <= playerScr.NoiseLevel)
              {
                // 発見状態 更新
                isDiscover = true;
              }
            }
          }
          break;

        // 追跡 or 攻撃
        case STATE_PURSUIT:
        case STATE_ATTACK:
          // 発見状態 更新
          isDiscover = true;
          break;
      }

      return isDiscover;
    }
  }

  // プレイヤー座標
  public Vector3 PlayerPos
  {
    get
    {
      // プレイヤー座標
      Vector3 playerPos = Vector3.zero;

      // プレイヤーあり
      if (player)
      {
        // プレイヤー座標 更新
        playerPos = player.transform.position;
      }

      return playerPos;
    }
  }

  // プレイヤー
  private GameObject player;

  // 懐中電灯
  private GameObject flashlight;

  // リスポーン
  private GameObject respawn;

  // NavMeshAgent
  private NavMeshAgent agent;

  // プレイヤーScript
  private Player playerScr;

  // 現在状態
  private string currentState;

  // 夢中状態
  private bool isTrance = false;

  // 睡眠状態
  private bool isSleep = false;


  // Use this for initialization
  private void Start ()
  {

	}

	
	// Update is called once per frame
	private void Update ()
  {
    
	}


  // 初期化
  public void Init()
  {
    // 対象 取得
    player = GameObject.Find(PLAYER);

    // 懐中電灯 取得
    flashlight = GameObject.Find(FLASHLIGHT);

    // リスポーン 取得
    respawn = GameObject.Find(RESPAWN);

    // リスポーンあり
    if (respawn)
    {
      // リスポーン 表示切替
      respawn.SetActive(false);
    }

    // NavMeshAgent 取得
    agent = GetComponent<NavMeshAgent>();

    // NavMeshAgentあり
    if (agent)
    {
      // 速度 更新
      agent.speed = moveSpeed;
    }

    // プレイヤーScript 取得
    playerScr = (player) ? player.GetComponent<Player>() : null;
  }


  // 状態 取得
  public string GetState(bool isReturnToChange = false)
  {
    // 状態 設定
    string state = STATE_IDLE;

    // プレイヤー & プレイヤーScript あり
    if (player && playerScr)
    {
      // プレイヤー座標 取得
      Vector3 playerPos = PlayerPos;

      // 座標 取得
      Vector3 selfPos = transform.position;

      // プレイヤーとの距離 取得
      float distance = Vector3.Distance(playerPos, selfPos);

      // 夢中状態
      if (isTrance)
      {
        // 状態 更新
        state = STATE_TRANCE;
      }
      // 睡眠状態
      else if (isSleep)
      {
        // 状態 更新
        state = STATE_SLEEP;
      }
      // 距離が攻撃距離 以下
      else if (distance <= attackDistance)
      {
        // 状態 更新
        state = STATE_ATTACK;
      }
      // 距離が追跡距離 以下
      else if (distance <= pursuitDistance)
      {
        // 状態 更新
        state = STATE_PURSUIT;
      }
      // 距離が視覚距離 or 聴覚距離 以下
      else if (distance <= viewDistance || distance <= hearDistance)
      {
        // 状態 更新
        state = STATE_SEARCH;
      }
    }

    // 変更時に戻り値 & 状態と現在状態 一致
    if (isReturnToChange && state == currentState)
    {
      // 状態 初期化
      state = null;
    }
    // その他
    else
    {
      // 現在状態 更新
      currentState = state;
    }

    return state;
  }


  // 移動
  public void Move(Vector3 targetPos)
  {
    if (targetPos == null || targetPos == Vector3.zero)
      return;

    // 現在状態
    switch (currentState)
    {
      // 攻撃
      case STATE_ATTACK:
        // 方向 更新
        transform.LookAt(targetPos);

        // 移動
        agent.Move(transform.forward * Time.deltaTime * moveSpeed);
        break;

      // その他
      default:
        // 移動
        agent.SetDestination(targetPos);
        break;
    }
  }


  // 衝突判定
  private void OnTriggerEnter(Collider other)
  {
    if (!player || !flashlight)
      return;

    // プレイヤーと一致
    if (other.gameObject.tag == player.tag)
    {
      // プレイヤーに接触
      HitPlayer();

      // 夢中 開始
      StartCoroutine(StartTrance());
    }
    // 懐中電灯と一致
    else if (other.gameObject.tag == flashlight.tag)
    {
      // 懐中電灯に接触
      HitFlashlight();

      // 睡眠 開始
      StartCoroutine(StartSleep());
    }
  }


  // プレイヤーに接触
  public virtual void HitPlayer()
  {
    Debug.Log("Enemy => void HitPlayer()");

    // HP 削減
    playerScr.RemoveHp(phiysicalPower);
  }


  // 懐中電灯に接触
  public virtual void HitFlashlight()
  {
    Debug.Log("Enemy => void HitFlashlight()");
  }


  // 夢中 開始
  private IEnumerator StartTrance()
  {
    if (!agent)
      yield break;

    // 追跡座標 削除
    agent.ResetPath();

    // 夢中状態 更新
    isTrance = true;

    // 待機
    yield return new WaitForSeconds(tranceSec);

    // 夢中状態 更新
    isTrance = false;

    // 夢中 終了
    EndTrance();
  }


  // 睡眠 開始
  private IEnumerator StartSleep()
  {
    if (!agent)
      yield break;

    // 追跡座標 削除
    agent.ResetPath();

    // 睡眠状態 更新
    isSleep = true;

    // 待機
    yield return new WaitForSeconds(sleepSec);

    // 睡眠状態 更新
    isSleep = false;

    // 睡眠 終了
    EndSleep();
  }


  // 夢中 終了
  public virtual void EndTrance()
  {
    Debug.Log("Enemy => void EndTrance()");
  }


  // 睡眠 終了
  public virtual void EndSleep()
  {
    Debug.Log("Enemy => void HitFlashlight()");
  }


  // ワープ開始
  public IEnumerator StartWarp()
  {
    if (!player || !agent || !respawn)
      yield break;

    // 夢中状態 更新
    isTrance = true;

    // ワープ座標 取得
    Vector3 warpVec = player.transform.position + (player.transform.forward * 1.0f);

    // リスポーン座標 更新
    respawn.transform.position = warpVec;

    // リスポーン 表示切替
    respawn.SetActive(true);

    // リスポーン時間 待機
    yield return new WaitForSeconds(RESPAWN_SEC);

    // ワープ
    agent.Warp(warpVec);

    // リスポーン 表示切替
    respawn.SetActive(false);

    // 夢中状態 更新
    isTrance = false;
  }


}
