using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace EA.FIFA20.UI
{
	public class MainLoadingCanvas : MonoBehaviour, ILoadingUI
	{
		[SerializeField] Image progress1 = default;
		[SerializeField] Image progress2 = default;
		[SerializeField] Image progress3 = default;
		[SerializeField] float refreshDelay = 0.8f;
		WaitForSeconds delay;
		
		bool complete;
		
		void Awake()
		{
			progress1.gameObject.SetActive(true);
			progress2.gameObject.SetActive(false);
			progress3.gameObject.SetActive(false);
			
			delay = new WaitForSeconds(refreshDelay);
		}

		void Start()
		{
			StartCoroutine(Loading());
		}

		void OnEnable()
		{
			complete = false;
		}

		void OnDisable()
		{
			complete = true;
		}

		IEnumerator Loading()
		{
			int step = 0;
			while (!complete)
			{
				yield return delay;
				step += 1;
				step %= 3;

				if (step == 0)
				{
					progress1.gameObject.SetActive(true);
					progress2.gameObject.SetActive(false);
					progress3.gameObject.SetActive(false);
				}
				else if (step == 1)
				{
					progress1.gameObject.SetActive(true);
					progress2.gameObject.SetActive(true);
					progress3.gameObject.SetActive(false);
				}
				else if (step == 2)
				{
					progress1.gameObject.SetActive(true);
					progress2.gameObject.SetActive(true);
					progress3.gameObject.SetActive(true);
				}
			}
		}

		public void SetTimer(float time, Action callback = null)
		{
			StartCoroutine(TimerCoroutine(time, callback));
		}

		IEnumerator TimerCoroutine(float time, Action callback)
		{
			yield return new WaitForSeconds(time);
			complete = true;
			
			callback?.Invoke();
		}
	}
}