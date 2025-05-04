using System;
using System.Collections.Generic;
using Chips;
using Managers;

namespace Logic
{
    public interface IGameRuler
    {
        event Action<int, int> LinkResolved;
        event Action<GameState> GameFinished;
        void ResolveLink(List<LinkableChip> link);
        void Reset(int moveAmount, int startScore);
    }
}