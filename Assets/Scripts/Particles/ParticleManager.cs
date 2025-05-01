using Cores;
using UnityEngine;

namespace Particles
{
    public class ParticleManager : Singleton<ParticleManager>
    {
        [SerializeField]
        private ParticlePool _chipParticlePool;

        public ParticlePool ChipParticlePool => _chipParticlePool;

        public Particle GetChipParticle()
        {
            var particle = _chipParticlePool.GetObject();
            return particle;
        }
    }
}