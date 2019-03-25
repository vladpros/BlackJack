import { CardNumber } from './../../../../sheared/enams/card-number.enum';
import { Component, OnInit, Input } from '@angular/core';
import { CardLear } from './../../../../sheared/enams/card-lear.enum';

@Component({
  selector: 'app-card',
  templateUrl: './card.component.html',
  styleUrls: ['./card.component.css']
})

export class CardComponent implements OnInit {

  cardLearS;
  cardNumberS;

  @Input() cardLear: CardLear;
  @Input() cardNumber: CardNumber;

  constructor() {
   }

  ngOnInit() {
    this.cardLearS = CardLear[this.cardLear];
    this.cardNumberS = CardNumber[this.cardNumber];
  }

}
