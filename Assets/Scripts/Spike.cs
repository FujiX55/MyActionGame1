using UnityEngine;
using System.Collections;

public class Spike : Token
{
	public static TokenMgr<Spike> parent = null;

	public static Spike Add(float x, float y)
	{
		return parent.Add (x, y);
	}

	// 更新する
	void FixedUpdate()
	{
		// 回転する
		Angle += 2;
	}

	// 接触判定チェック
	void OnTriggerEnter2D(Collider2D other)
	{
		string name = LayerMask.LayerToName (other.gameObject.layer);

		if (name == "Player")
		{
			// プレイヤが衝突
			Player p = other.gameObject.GetComponent<Player>();

			// ゲームオーバー状態にする
			p.SetGameState(Player.eGameState.GameOver);

			// プレイヤ消滅
			p.Vanish();
		}
	}
}
