import { StartGameModule } from './start-game/start-game.module';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { GameRoutingModule } from './game-routing.model';



@NgModule({
  declarations: [
  ],
  imports: [
    CommonModule,
    StartGameModule,
    ReactiveFormsModule,
    GameRoutingModule,
  ],
})
export class GameModule { }
