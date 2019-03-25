import { StartGameComponent } from './start-game/start-game.component';
import { NgModule, } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

const routes: Routes = [
  {path: '', redirectTo: 'startgame', pathMatch: 'full'},
  {path: 'startgame',  loadChildren: './start-game/start-game.module#StartGameModule'},
  {path: 'playgame', loadChildren: './game-show/game-show.module#GameShowModule'}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class GameRoutingModule { }
