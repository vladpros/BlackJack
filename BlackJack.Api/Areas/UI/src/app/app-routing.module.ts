import { StartGameComponent } from './start-game/start-game.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { GameShowComponent } from './game-show/game-show.component';

const routes: Routes = [
  {path: '', redirectTo: 'startGame', pathMatch: 'full'},
  {path: 'startGame', component: StartGameComponent},
  {path: 'playGame/:id', component: GameShowComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
