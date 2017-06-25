using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
// using SeManager;


public class Player : MonoBehaviour
{
  // 振動時間
  private const float SHAKE_TIME = 0.5f;

  // 最大HP
  public float maxHp = 30.0f;

  // 自然回復
  public float recovery = 1.5f;

  // 歩行速度
  public float walkSpeed = 3.0f;

  // 走行速度
  public float runSpeed = 5.0f;

  // カメラ
  private GameObject cam;

  // Animator
  private Animator anime;

  // NavMeshAgent
  private NavMeshAgent agent;

  // 懐中電灯
  private Flashlight flashlight;

  // SEマネージャー
  private SeManager seManager;

  // HP
  private float hp = 0.0f;
  public float Hp
  {
    get { return hp; }
    private set
    {
      if (hp == value)
        return;

      // 値が最大HP 超過
      if (maxHp < value)
      {
        hp = maxHp;
      }
      // 値が0 未満
      else if (value < 0)
      {
        hp = 0;
      }
      // その他
      else
      {
        hp = value;
      }
    }
  }

  // 現在HP
  private float currentHp = 0.0f;
  public float CurrentHp
  {
    get { return currentHp; }
    private set
    {
      if (currentHp == value)
        return;

      // 値がHP 超過
      if (hp < value)
      {
        currentHp = hp;
      }
      // 値が0 未満
      else if (value < 0)
      {
        currentHp = 0;
      }
      // その他
      else
      {
        currentHp = value;
      }
    }
  }

  // 疲労状態
  private bool isTired = false;

  // 潜伏状態
  private bool isHide = false;
  public bool IsHide
  {
    get { return isHide; }
    private set
    {
      if (isHide == value)
        return;

      isHide = value;
    }
  }

  // 移動座標
  private float h = 0.0f;
  private float v = 0.0f;

  // 走行状態
  private bool isRun = false;

  // 懐中電灯状態
  private bool isFlashlight = false;


  private FollowCamera camScr;



  private float noiseLevel = 2.0f;
  public float NoiseLevel
  {
    get { return noiseLevel; }
    private set
    {
      if (noiseLevel == value)
        return;

      noiseLevel = value;
    }
  }
  


	// Use this for initialization
	private void Start ()
  {
    // HP 取得
    CurrentHp = Hp = maxHp;

    // カメラ 取得
    cam = GameObject.FindWithTag("MainCamera");

    // Animator 取得
    anime = GetComponent<Animator>();

    // NavMeshAgent 取得
    agent = GetComponent<NavMeshAgent>();

    // 懐中電灯
    flashlight = GetComponent<Flashlight>();

    // Script 取得
    camScr = (cam) ? cam.GetComponent<FollowCamera>() : null;

    // SEマネージャー 初期化
    seManager = GetComponent<SeManager>();  // new SeManager();

    AudioSource[] seList = GetComponents<AudioSource>();
    if (seList[0]) { seManager.AddSe("walk", seList[0]); }
    if (seList[1]) { seManager.AddSe("run", seList[1]); }
    if (seList[2]) { seManager.AddSe("damage", seList[2]); }
  }
	

	// Update is called once per frame
	private void Update ()
  {

  }


  private void FixedUpdate()
  {
    if (!cam || !anime)
      return;

    // Anime歩行状態 設定
    bool isAnimeWalk = false;

    // Anime走行状態 設定
    bool isAnimeRun = false;

    // 移動状態 設定
    bool isMove = (h != 0 || v != 0) ? true : false;

    // 無疲労状態 & 移動状態
    if (!isTired && isMove)
    {
      // 走行状態
      if (isRun)
      {
        // 現在HP 更新
        CurrentHp -= runSpeed * Time.deltaTime;

        //  現在HPが0 以下
        if (currentHp <= 0)
        {
          // 走行状態 更新
          isRun = false;

          // 疲労状態 更新
          isTired = true;

          // Animator 更新
          anime.SetBool("is_tired", isTired);
        }
      }
      // 歩行状態
      else
      {
        // 現在HP 更新
        CurrentHp += recovery * walkSpeed * Time.deltaTime;
      }

      // 疲労状態
      if (isTired)
      {
        // 懐中電灯状態
        if (isFlashlight)
        {
          // 入力懐中電灯状態 設定
          SetInputIsFlashlight();
        }
      }
      // 無疲労状態
      else
      {
        // カメラ座標 取得
        Vector3 camVec = Vector3.Scale(cam.transform.forward, new Vector3(1, 0, 1)).normalized;

        // 移動座標 取得
        Vector3 moveVec = camVec * v + cam.transform.right * h;

        // 速度 取得
        float speed = (isRun) ? runSpeed : walkSpeed;

        // 移動速度 設定
        agent.Move(moveVec * Time.deltaTime * speed);

        // 移動あり
        if (moveVec != Vector3.zero)
        {
          // 座標 更新
          transform.rotation = Quaternion.LookRotation(moveVec);
        }
      }

      // Anime走行状態 更新
      isAnimeRun = isRun;

      // Anime歩行状態 更新
      isAnimeWalk = !isAnimeRun;
    }
    // 現在HPがHP 未満
    else if (CurrentHp < Hp)
    {
      // 現在HP 更新
      CurrentHp += recovery * runSpeed * Time.deltaTime;

      // 疲労状態 & 現在HPがHP 以上
      if (isTired && hp <= CurrentHp)
      {
        // 疲労状態 更新
        isTired = false;

        // Animator 更新
        anime.SetBool("is_tired", isTired);
      }
    }

    // Animator 更新
    anime.SetBool("is_walk", isAnimeWalk);
    anime.SetBool("is_run", isAnimeRun);

    if (isAnimeWalk)
    {
      seManager.PlaySe("walk");
    }
    else if (isAnimeRun)
    {
      seManager.PlaySe("run");
    }
    else
    {
      seManager.StopCurrentSe();
    }
  }


  // 入力移動座標 設定
  public void SetInputAxisRaw(float inputH, float inputV)
  {
    // 移動座標 更新
    h = inputH;
    v = inputV;
  }


  // 入力走行状態 設定
  public void SetInputIsRun()
  {
    // 無疲労状態
    if (!isTired)
    {
      // 走行状態 更新
      isRun = !isRun;
    }
  }


  // 入力懐中電灯状態 設定
  public void SetInputIsFlashlight()
  {
    if (!flashlight)
      return;

    // 懐中電灯状態 更新
    isFlashlight = !isFlashlight;

    // スイッチ切替
    flashlight.ChangeSwitch(isFlashlight);
  }


  // 死亡状態
  public bool IsDead()
  {
    // 死亡状態 取得
    bool isDead = (Hp <= 0) ? true : false;

    return isDead;
  }

  // HP比率 取得
  public float GetHpRatio()
  {
    return Hp / maxHp;
  }


  // 現在HP比率 取得
  public float GetCurrentHpRatio()
  {
    // return CurrentHp / Hp;
    return CurrentHp / maxHp;
  }


  // HP 追加
  public void AddHp(float addHp)
  {
    // HP 更新
    Hp += addHp;

    // 現在HP 更新
    CurrentHp = Hp;
  }


  // HP 削減
  public void RemoveHp(float removeHp)
  {
    if (!anime)
      return;

    // HP 更新
    Hp -= removeHp;

    // 現在HP 更新
    CurrentHp = Hp;

    // 疲労状態
    if (isTired)
    {
      // 疲労状態 更新
      isTired = false;

      // Animator 更新
      anime.SetBool("is_tired", isTired);
    }

    // ダメージ
    StartCoroutine(Damage());
  }

  // ダメージ
  private IEnumerator Damage()
  {
    if (!agent || !camScr)
       yield break;

    // レイヤー 切替
    gameObject.layer = LayerMask.NameToLayer("PlayerDamage");

    // ノックバック座標 取得
    Vector3 knockbackPos = transform.position + (transform.forward * -1.0f);

    // ノックバックY座標 更新
    knockbackPos.y = (knockbackPos.y < 0) ? 0.0f : knockbackPos.y;

    // 座標 ノックバック
    agent.Warp(knockbackPos);


    seManager.PlaySe("damage");

    // Animator 更新
    anime.SetBool("is_damage", true);

    // 振動開始
    camScr.StartShake(SHAKE_TIME, 0.1f);

    yield return new WaitForSeconds(0.5f);

    // Animator 更新
    anime.SetBool("is_damage", false);

    // レイヤー 切替
    gameObject.layer = LayerMask.NameToLayer("Player");
  }


  // 潜伏 切替
  public void ChangeHide(bool value)
  {
    // 潜伏状態 更新
    IsHide = value;
  }


}
