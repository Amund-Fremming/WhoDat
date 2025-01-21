import Play from '@/src/Game/GameRouter';
import { GameplayProvider } from '@/src/Shared/providers/GameplayProvider';
import { GameProvider } from '@/src/Shared/providers/GameProvider';

export default function PlayTab() {
  return (
    <GameProvider>
      <GameplayProvider>
        <Play />
      </GameplayProvider>
    </GameProvider>
  );
}
