import { CardComponent } from './cards/card/card.component';
import { PlayerStatusComponent } from './player-status/player-status.component';
import { GameShowComponent } from './game-show.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CardsComponent } from './cards/cards.component';
import { GameShowRoutingModule } from './game-show-routing.module';

@NgModule({
  declarations: [
    GameShowComponent,
    PlayerStatusComponent,
    CardsComponent,
    CardComponent,
  ],
  imports: [
    CommonModule,
    GameShowRoutingModule,
  ]
})
export class GameShowModule { }
