using UnityEngine;
using System.Collections;

public class FloorMove : Token
{
	public static TokenMgr<FloorMove> parent = null;
	public static FloorMove Add (float x, float y)
	{
		FloorMove floor = parent.Add (x, y);

		// 初期化
		floor.Init ();

		return floor;
	}

	// 前回の更新時のX座標
	float _xprevious = 0;

	// プレイヤーの参照
	Player _target = null;

	// 開始時の移動速度
	public float StartSpeed = 0.5f;

	// 初期化
	public void Init()
	{
		// 開始時は右に進むようにする
		SetVelocityXY (StartSpeed, 0);

		// 初期座標を保持
		_xprevious = X;
	}

	// トリガーイベント検出
	void OnTriggerEnter2D(Collider2D other)
	{
		string name = LayerMask.LayerToName (other.gameObject.layer);

		if (name == "Ground")
		{
			// 1フレーム前のX座標と一致していないときのみ反転する
			if (X != _xprevious)
			{
				// 他の壁・床と当たったら反転
				VX *= -1;

				// X座標を1フレーム前のX座標に戻す
				X = _xprevious;
			}

			// 反対方向に押し出す
			X += VX * Time.deltaTime;
		}
		else if (name == "Player")
		{
			// プレイヤに当たったので参照を保持
			_target = other.gameObject.GetComponent<Player>();
		}
	}

	// トリガーイベントから離脱
	void OnTriggerExit2D(Collider2D other)
	{
		string name = LayerMask.LayerToName (other.gameObject.layer);
		
		if (name == "Player")
		{
			// プレイヤがいなくなったので参照を消す
			_target = null;
		}
	}

	// 更新
	void Update()
	{
		// 前回の座標からの差分を求める
		float dx = X - _xprevious;

		if (_target != null)
		{
			// 上にプレイヤーが乗っていたら動かす
			_target.X += dx;
		}

		// 現在の座標を次に使うように保存
		_xprevious = X;
	}

	// 消滅
	public override void Vanish()
	{
		// 上にプレイヤーが乗ったまま死ぬときの対応
		_target = null;

		base.Vanish ();
	}
}
