import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PlayerInGameView } from '../models/player-in-game-view';
import { environment } from 'src/environments/environment';

@Injectable()
export class GameShowService {

  constructor(private httpClient: HttpClient) {
  }

  private controllerPath = '/api/Game/';

  getGameInfo(id, choos): Observable<PlayerInGameView> {
    return this.httpClient
    .get<PlayerInGameView>(environment.webApiAdress + this.controllerPath + 'ShowGame?gameId=' + id + '&choos=' + choos);
  }

  getGameResult(id): Observable<PlayerInGameView> {
    return this.httpClient.get<PlayerInGameView>(environment.webApiAdress + this.controllerPath + 'GameResult?gameId=' + id);
  }
}