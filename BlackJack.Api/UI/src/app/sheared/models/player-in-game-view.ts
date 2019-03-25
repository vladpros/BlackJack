import { CardView } from './card-view';

export interface PlayerInGameView {
  playerName: string;
  cards: CardView[];
  points: number;
  playerStatus: number;
  gameId: number;
}
