using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UtcAnimator : MonoBehaviour
{
  // 歩き状態
  public bool isWalk = false;

  // 走り状態
  public bool isRun = false;

  // 疲労状態
  public bool isTired = false;

  // ダメージ状態
  public bool isDamage = false;

  // Animator
  private Animator anim;


	// Use this for initialization
	void Start ()
  {
    // Animator 取得
    anim = GetComponent<Animator>();

    // Animatorあり
    if (anim)
    {
      // 歩き状態
      if (isWalk)
      {
        // Animator 更新
        anim.SetBool("is_walk", true);
      }
      // 走り状態
      else if (isRun)
      {
        // Animator 更新
        anim.SetBool("is_run", true);
      }
      // 疲労状態
      else if (isTired)
      {
        // Animator 更新
        anim.SetBool("is_tired", true);
      }
      // ダメージ状態
      else if (isDamage)
      {
        // Animator 更新
        anim.SetBool("is_damage", true);
      }
    }
	}
	
	// Update is called once per frame
	void Update ()
  {
		
	}
}
