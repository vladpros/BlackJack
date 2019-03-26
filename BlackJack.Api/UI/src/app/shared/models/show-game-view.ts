import { CardView } from './card-view';

export interface ShowGameView {
  showGameViewItems: ShowGameViewItem[];
}

export interface ShowGameViewItem {
  playerName: string;
  cards: CardView[];
  points: number;
  playerStatus: string;
  gameId: number;
}
