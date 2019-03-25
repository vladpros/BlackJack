import { ReactiveFormsModule } from '@angular/forms';
import { CardsComponent } from './game-show/cards/cards.component';
import { CardComponent } from './game-show/cards/card/card.component';
import { StartGameComponent } from './start-game/start-game.component';
import { NgModule, ErrorHandler } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GameShowComponent } from './game-show/game-show.component';
import { PlayerStatusComponent } from './player-status/player-status.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';

@NgModule({
  declarations: [
    StartGameComponent,
    GameShowComponent,
    PlayerStatusComponent,
    CardComponent,
    CardsComponent,
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot(),
  ]
})
export class GameModule { }
