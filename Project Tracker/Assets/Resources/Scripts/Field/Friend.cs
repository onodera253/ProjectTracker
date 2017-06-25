using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


[RequireComponent(typeof(GameObject))]
[RequireComponent(typeof(Text))]


public class Friend : MonoBehaviour
{
  // 対象
  public GameObject target;

  // 探索距離
  public float searchDistance = 8.0f;

  // Text
  public Text textGuide;

  // Animator
  private Animator anime;

  // NavMeshAgent
  private NavMeshAgent agent;

  // 救助状態
  private bool isRescue = false;


  // Use this for initialization
  private void Start ()
  {
    // Animator 取得
    anime = GetComponent<Animator>();

    // NavMeshAgent 取得
    agent = GetComponent<NavMeshAgent>();
  }


  // Update is called once per frame
  private void Update()
  {
    if (!target)
      return;

    // 救助状態
    if (isRescue)
    {
      // 走行状態 設定
      bool isRun = false;

      // 対象との距離 取得
      float distance = Vector3.Distance(target.transform.position, transform.position);

      // 距離が探索距離以下
      if (distance <= searchDistance)
      {
        // 走行状態 更新
        isRun = true;

        // 追跡座標 更新
        agent.SetDestination(target.transform.position);
      }

      // Animator 更新
      anime.SetBool("is_run", isRun);
    }
    // 未救助状態
    else
    {
      // 対象方向に角度 更新
      transform.LookAt(target.transform);
    }
  }


  // 衝突判定
  private void OnTriggerEnter(Collider other)
  {
    if (!target)
      return;

    // 対象に接触
    if (other.gameObject.tag == target.tag)
    {
      // 未救助状態
      if (!isRescue)
      {
        // 救助状態 更新
        isRescue = true;
        
        // ガイドテキスト 変更
        ChangeTextGuide();
      }
    }
  }


  // ガイドテキスト 変更
  private void ChangeTextGuide()
  {
    if (!textGuide)
      return;

    // ガイドテキスト 更新
    textGuide.text = "目的：「藤原みさき」の待つ開始地点に帰る";
  }
}
