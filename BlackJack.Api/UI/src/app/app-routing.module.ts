import { StartGameComponent } from './game/start-game/start-game.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { GameShowComponent } from './game/game-show/game-show.component';

const routes: Routes = [
  {path: '', redirectTo: 'startGame', pathMatch: 'full'},
  {path: 'startGame', component: StartGameComponent},
  {path: 'playGame/:id', component: GameShowComponent},
  {path: 'playGame', redirectTo: 'startGame'},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
