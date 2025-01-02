import Play from '@/src/Game/GameRouter';
import { GameProvider } from '@/src/Shared/providers/GameProvider';

export default function PlayTab() {
  return (
    <GameProvider>
      <Play />
    </GameProvider>
  );
}
