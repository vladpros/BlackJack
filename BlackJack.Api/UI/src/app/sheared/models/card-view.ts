import { CardNumber } from '../enams/card-number.enum';
import { CardLear } from '../enams/card-lear.enum';

export interface CardView {
  cardLeard: CardLear;
  cardNumber: CardNumber;
}
