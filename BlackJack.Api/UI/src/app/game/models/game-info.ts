import { Card } from './card';

export interface GameInfo {
  PlayerName: string;
  Cards: Card;
  Points: number;
  PlayerStatus: number;
  GameId: number;
}
