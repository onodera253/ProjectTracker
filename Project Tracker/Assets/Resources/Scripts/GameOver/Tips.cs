using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Text))]


public class Tips : MonoBehaviour
{
  // Text定数
  const string TEXT_TITLE   = "TextTipsTitle";
  const string TEXT_MESSAGE = "TextTipsMessage";

  // 待機時間
  const float WAIT_TIME = 6.5f;

  // TIPSリスト
  private string[,] tipsList = new string[,]
  {
    { "キノコと体力", "キノコに接触すると、消耗している体力が回復する。\n上限を超えて回復しないので、必要に応じて取るといい。" },
    { "タンポポと電池", "タンポポに接触すると、消耗している電池が回復する。\n上限を超えて回復しないので、必要に応じて取るといい。" },
    { "草のしげみ", "草のしげみの中で動きを止めると、\n敵の注意や追跡が、解除されやすくなる。" },
    { "切り株", "通路を塞いでいる切り株は、\n連動する新芽を刈る事で、撤去できる。" },
    { "新芽", "新芽に接触して刈ると、\n連動する切り株を撤去して、道を拓ける。" },
    { "体力と体力ゲージ", "体力が0になると、ゲームオーバーになる。\n体力ゲージは、体力を上限として走ると減少する。" },
    { "体力ゲージと走り", "走ると、体力ゲージが減少する。\nゲージが0になると、一時的に移動できなくなる。" },
    { "体力ゲージと休息", "動きを止めて休息すると、体力ゲージが回復する。\n歩きでも、体力ゲージを微回復できる。" },
    { "体力ゲージとダメージ", "体力ゲージの減少量に比例して、\n敵から受けるダメージが増加してしまう。" },
    { "歩きと走り", "歩きは遅いが、足音が小さく体力ゲージが微回復する。\n走りは早いが、足音が大きく体力ゲージが減少する。" },
    { "懐中電灯と電池", "懐中電灯は、使用すると電池を消耗する。\n自動的には回復しないので注意が必要。" },
    { "懐中電灯と敵", "懐中電灯の光りを敵に当てる事で、\n敵の動きを一時的に止める事ができる。" },
    { "敵の追跡", "敵の一定距離内に入ると、追いかけられる。\n一定距離外まで逃げ切ると、追跡を解除できる。" },
    { "敵の視線", "敵の視野に入ると、追いかけられる。\n視線を外すか草のしげみに入ると、追跡を解除しやすくなる。" },
    { "敵の聴覚", "敵の一定距離内で、足音などの音を出すと気付かれる。\n草のしげみの中は、敵に音を感知されにくい。" },
    { "時間と暗闇", "経過した時間に比例して、暗闇が濃くなる。\n濃い暗闇の中でも、懐中電灯で周囲を照らせる。" }
  };

  // Text
  public Text textTitle;
  public Text textMessage;

  // TipsID
  private int tipsId = 0;

  // 表示状態
  private bool isShow = false;


  // Use this for initialization
  private void Start ()
  {

  }
	

	// Update is called once per frame
	private void Update ()
  {
		
	}


  private void FixedUpdate()
  {
    if (isShow)
      return;

    // Tips 表示
    StartCoroutine(ShowTips());
  }


  // Tips 表示
  private IEnumerator ShowTips()
  {
    if (!textTitle || !textMessage)
      yield break;

    // 表示状態 更新
    isShow = true;

    // Tipsリスト数 取得
    int tipsListLength = tipsList.GetLength(0);

    // 乱数ID 取得
    int randId = Random.Range(0, tipsListLength);

    // TipsIDと乱数ID 一致
    if (tipsId == randId)
    {
      // TipsID 更新
      tipsId = (tipsId + 1) % tipsListLength;
    }
    // その他
    else
    {
      // TipsID 更新
      tipsId = randId;
    }

    // Tipsリストあり
    if (tipsList != null && 2 <= tipsList.GetLength(1))
    {
      // Text 更新
      textTitle.text   = tipsList[tipsId, 0].ToString();
      textMessage.text = tipsList[tipsId, 1].ToString();
    }

    // 待機
    yield return new WaitForSeconds(WAIT_TIME);

    // 表示状態 更新
    isShow = false;
  }
}
