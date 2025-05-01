using Cores;
using UnityEngine;

namespace Particles
{
    public class ParticleManager : Singleton<ParticleManager>
    {
        [Header("Chip Particle")]
        [SerializeField]
        private ParticlePool _chipParticlePool;

        public ParticlePool ChipParticlePool => _chipParticlePool;

        [SerializeField]
        private int _chipParticlePoolSize;

        public void Init()
        {
            _chipParticlePool.CreatePool(_chipParticlePoolSize);
        }

        public Particle GetChipParticle()
        {
            var particle = _chipParticlePool.GetObject();
            return particle;
        }
    }
}