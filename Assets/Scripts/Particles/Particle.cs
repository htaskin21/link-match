using System.Collections;
using UnityEngine;

namespace Particles
{
    [RequireComponent(typeof(ParticleSystem))]
    public class Particle : MonoBehaviour
    {
        [SerializeField]
        private ParticleSystem _particleSystem;

        public void PlayAndRelease(ParticlePool pool, Color color, Vector3 position)
        {
            transform.position = position;
            SetColor(color);
            gameObject.SetActive(true);

            var ps = _particleSystem;
            ps.Play();

            StartCoroutine(ReleaseAfter(ps.main.duration + 0.1f, pool));
        }

        private IEnumerator ReleaseAfter(float delay, ParticlePool pool)
        {
            yield return new WaitForSeconds(delay);
            gameObject.SetActive(false);
            pool.ReturnToPool(this);
        }

        private void SetColor(Color color)
        {
            var main = _particleSystem.main;
            main.startColor = color;
        }
    }
}