import { CardNumber } from './../enams/card-number.enum';
import { CardLear } from './../enams/card-lear.enum';

export interface Card {
  cardLeard: CardLear;
  cardNumber: CardNumber;
}
