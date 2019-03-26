import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PlayerInGameView } from '../models/player-in-game-view';
import { apiWebAdress } from 'src/environments/environment';

@Injectable()
export class GameShowService {

  constructor(private httpClient: HttpClient) {
  }

  private controllerName = '/api/Game/';

  getGameInfo(id, choos): Observable<PlayerInGameView> {
    return this.httpClient.get<PlayerInGameView>(apiWebAdress + this.controllerName + 'ShowGame?gameId=' + id + '&choos=' + choos);
  }

  getGameResult(id): Observable<PlayerInGameView> {
    return this.httpClient.get<PlayerInGameView>(apiWebAdress + this.controllerName + 'GameResult?gameId=' + id);
  }
}
