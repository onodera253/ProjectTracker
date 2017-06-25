using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyTypeWarrior:Enemy
{
  // ワープ時間（秒）
  public float warpSec = 30.0f;

  // 走り状態
  private bool isRun = false;
  public bool IsRun
  {
    get { return isRun; }
    private set
    {
      if (value != isRun)
      {
        isRun = value;

        // Animatorあり
        if (anim)
        {
          // Animator 更新
          anim.SetBool("is_run", isRun);
        }

        // SEあり
        if (se)
        {
          // 走り状態
          if (isRun)
          {
            // SE 再生
            se.Play();
          }
          // その他
          else
          {
            // SE 停止
            se.Stop();
          }
        }
      }
    }
  }

  // SE
  private AudioSource se;

  // Animator
  private Animator anim;

  // 待機時間（秒）
  private float waitSec = 0.0f;


	// Use this for initialization
	private void Start ()
  {
    // 初期化
    base.Init();

    // SE 取得
    se = GetComponent<AudioSource>();

    // Animator 取得
    anim = GetComponent<Animator>();
  }
	

	// Update is called once per frame
	private void Update ()
  {
    if (!anim)
      return;

    // 状態 取得
    string state = base.GetState(true);

    // 発見状態 取得
    bool isDiscover = IsDiscover;

    // 状態別
    switch (state)
    {
      // 待機
      case STATE_IDLE:
        // 走り状態 更新
        IsRun = false;
        break;

      // 索敵
      case STATE_SEARCH:
        // 発見 状態
        if (isDiscover)
        {
          // プレイヤー 視認
          transform.LookAt(PlayerPos);

          // 走り状態 更新
          IsRun = true;
        }
        // 未発見 状態
        else
        {
          // 走り状態 更新
          IsRun = false;
        }
        break;

      // 追跡
      case STATE_PURSUIT:
        // プレイヤー 視認
        transform.LookAt(PlayerPos);

        // 走り状態 更新
        IsRun = true;
        break;

      // 攻撃
      case STATE_ATTACK:
        // 攻撃 開始
        // StartCoroutine(StartAttack());
        break;

      // 夢中
      case STATE_TRANCE:
        // Animator 更新
        anim.SetBool("is_freeze", true);
        break;

      // 睡眠
      case STATE_SLEEP:
        // Animator 更新
        anim.SetBool("is_freeze", true);
        break;
    }

    // 発見 状態
    if (isDiscover)
    {
      // 待機時間が0 超過
      if (0 < waitSec)
      {
        // 待機時間 初期化
        waitSec = 0.0f;
      }

      // 移動
      base.Move(PlayerPos);
    }
    // 未発見 状態
    else
    {
      // 待機時間 更新
      UpdateWaitSec();
    }
  }


  // 攻撃 開始
  private IEnumerator StartAttack()
  {
    if (!anim)
      yield break;

    // Animator 更新
    anim.SetBool("is_attack", true);

    // 待機
    yield return new WaitForSeconds(3.0f);

    // Animator 更新
    anim.SetBool("is_attack", false);
  }


  // 夢中 終了
  public override void EndTrance()
  {
    // Animator 更新
    anim.SetBool("is_freeze", false);
  }


  // 睡眠 終了
  public override void EndSleep()
  {
    // Animator 更新
    anim.SetBool("is_freeze", false);
  }


  // プレイヤーに接触
  public override void HitPlayer()
  {
    Debug.Log("EnemyTypeWarrior => HitPlayer");

    // プレイヤーに接触
    base.HitPlayer();

    // 攻撃 開始
    StartCoroutine(StartAttack());
  }


  // 懐中電灯に接触
  public override void HitFlashlight()
  {
    Debug.Log("EnemyTypeWarrior => HitFlashlight");
  }


  // 待機時間 更新
  private void UpdateWaitSec()
  {
    // 待機時間 更新
    waitSec += Time.deltaTime;

    // 待機時間がワープ時間 超過
    if (warpSec < waitSec)
    {
      // 待機時間 初期化
      waitSec = 0.0f;

      // ワープ 開始
      StartCoroutine(base.StartWarp());
    }
  }
}
