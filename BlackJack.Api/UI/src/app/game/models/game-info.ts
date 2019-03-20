import { Card } from './card';

export interface GameInfo {
  PlayerName: string;
  Cards: Card;
  Point: number;
  PlayerStatus: number;
  GameId: number;
}
