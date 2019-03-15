import { CardNumber } from './../enams/card-number.enum';
import { CardLear } from './../enams/card-lear.enum';
import { Component, OnInit, Input } from '@angular/core';

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
    console.log(this.cardLear + '  ' + this.cardNumber);
    this.cardLearS = CardLear[this.cardLear];
    this.cardNumberS = CardNumber[this.cardNumber];
  }

}
