using System.Collections.Generic;
using System.Linq;
using Logic;
using UnityEngine;

namespace Managers
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField]
        private List<LevelDataSO> _levels;

        public LevelDataSO GetLevelData(int levelNo)
        {
            if (_levels.Count >= levelNo)
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