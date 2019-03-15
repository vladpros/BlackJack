import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-card',
  templateUrl: './card.component.html',
  styleUrls: ['./card.component.css']
})
export class CardComponent implements OnInit {

  cardLear = 0;
  cardNumber = 0;

  constructor() { }

  ngOnInit() {
  }

}
