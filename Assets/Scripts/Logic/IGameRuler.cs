using System;
using System.Collections.Generic;
using Chips;

namespace Logic
{
    // Defines contract for resolving links and notifying state changes and game end.
    public interface IGameRuler
    {
        event Action<int, int> LinkResolved;
        event Action<GameState> GameFinished;
        void ResolveLink(List<LinkableChip> link);
        void Reset(int moveAmount, int startScore);
    }
}