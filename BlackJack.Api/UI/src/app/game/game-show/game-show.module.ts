import { GameShowComponent } from './game-show.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GameShowRoutingModule } from './game-show-routing.module';

@NgModule({
  declarations: [
    GameShowComponent,
  ],
  imports: [
    CommonModule,
    GameShowRoutingModule,
  ]
})
export class GameShowModule { }
