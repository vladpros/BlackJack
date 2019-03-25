import { CardNumber } from './../../../../sheared/enams/card-number.enum';
import { Component, OnInit, Input } from '@angular/core';
import { CardLear } from './../../../../sheared/enams/card-lear.enum';

@Component({
  selector: 'app-card',
  templateUrl: './card.component.html',
  styleUrls: ['./card.component.css']
})

export class CardComponent implements OnInit {

  cardLearString;
  cardNumberString;

  @Input() cardLear: CardLear;
  @Input() cardNumber: CardNumber;

  constructor() {
   }

  ngOnInit() {
    this.cardLearString = CardLear[this.cardLear];
    this.cardNumberString = CardNumber[this.cardNumber];
  }

}
