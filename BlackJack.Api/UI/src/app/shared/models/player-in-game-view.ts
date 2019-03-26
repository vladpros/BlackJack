import { CardView } from './card-view';

export interface PlayerInGameView {
  playerInGameViewItems: PlayerInGameViewItem[];
}

export interface PlayerInGameViewItem {
  playerName: string;
  cards: CardView[];
  points: number;
  playerStatus: string;
  gameId: number;
}
