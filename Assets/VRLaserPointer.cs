using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRStandardAssets.Utils
{
	/// <summary>
	/// VRSample の VREyeRaycaster や Reticle（使用するのはUIの位置）を LaserPointer.target に入力する。
	/// </summary>
	public class VRLaserPointer : MonoBehaviour
	{
		public LaserPointer laserPointer;     //レーザーの描画
		public VREyeRaycaster eyeRaycaster;   //Rayを発射してオブジェクトを判定する
		public Transform guiReticle;          //レティクルなどのUI

		// Update is called once per frame
		void Update()
		{
			if (eyeRaycaster.CurrentInteractible != null)
				laserPointer.target = guiReticle;
			else
				laserPointer.target = null;
		}
	}
}