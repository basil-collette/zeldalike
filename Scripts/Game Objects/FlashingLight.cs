using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Assets.Scripts
{
    public class FlashingLight : MonoBehaviour
	{
		[SerializeField] private Light2D _light;
		private float _baseSize;

		void Start()
		{
			if (_light == null)
				_light = GetComponent<Light2D>();

			_baseSize = _light.pointLightOuterRadius;

			StartCoroutine(FlashCo());
		}

		IEnumerator FlashCo()
        {
			while (true)
            {
				while (_light.pointLightOuterRadius > _baseSize * 0.9f)
				{
					_light.pointLightOuterRadius -= (2f * Time.deltaTime);
					yield return null;
				}

				while (_light.pointLightOuterRadius < _baseSize * 1.1f)
				{
					_light.pointLightOuterRadius += (2f * Time.deltaTime);
					yield return null;
				}
			}
		}

	}
}
