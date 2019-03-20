import { CardComponent } from '../card/card.component';

export interface GameInfo {
  PlayerName: string;
  Cards: CardComponent;
  Point: number;
  PlayerStatus: number;
  GameId: number;
}
