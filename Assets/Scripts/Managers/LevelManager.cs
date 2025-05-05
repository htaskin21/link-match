using System.Collections.Generic;
using System.Linq;
using Logic;
using UnityEngine;

namespace Managers
{
    // Provides access to level configuration assets.
    public class LevelManager : MonoBehaviour
    {
        [SerializeField]
        private List<LevelDataSO> _levels;

        public LevelDataSO GetLevelData(int levelNo)
        {
            if (levelNo >= _levels.Count)
            {
                Debug.Log("Invalid Level No ");
                return _levels.First();
            }

            return _levels[levelNo];
        }

        public LevelDataSO GetRandomLevel()
        {
            var rnd = Random.Range(0, _levels.Count);
            return _levels[rnd];
        }
    }
}