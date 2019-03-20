import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { CardsComponent } from './game-show/cards/cards.component';
import { CardComponent } from './game-show/cards/card/card.component';
import { StartGameComponent } from './start-game/start-game.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GameShowComponent } from './game-show/game-show.component';
import { PlayerStatusComponent } from './player-status/player-status.component';

@NgModule({
  declarations: [
    StartGameComponent,
    GameShowComponent,
    PlayerStatusComponent,
    CardComponent,
    CardsComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule
  ]
})
export class GameModule { }
