import Play from '@/src/Game/Game';
import { GameProvider } from '@/src/Shared/providers/GameProvider';

export default function PlayTab() {
  return (
    <GameProvider>
      <Play />
    </GameProvider>
  );
}
