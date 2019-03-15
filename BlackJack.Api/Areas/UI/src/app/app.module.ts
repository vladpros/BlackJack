import { StartgameService } from './services/startgame.service';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { StartGameComponent } from './start-game/start-game.component';
import { HttpClientModule } from '@angular/common/http';
import { GameShowComponent } from './game-show/game-show.component';
import { CardComponent } from './card/card.component';

@NgModule({
  declarations: [
    AppComponent,
    StartGameComponent,
    GameShowComponent,
    CardComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    ReactiveFormsModule
  ],
  providers: [StartgameService],
  bootstrap: [AppComponent]
})
export class AppModule { }
