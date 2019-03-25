import { Card } from './card';

export interface GameInfo {
  playerName: string;
  cards: Card;
  points: number;
  playerStatus: number;
  gameId: number;
}
