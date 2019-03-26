import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ShowGameView } from '../models/show-game-view';
import { environment } from 'src/environments/environment';

@Injectable()
export class GameShowService {

  constructor(private httpClient: HttpClient) {
  }

  private controllerPath = '/api/Game/';

  getGameInfo(id, choos): Observable<ShowGameView> {
    return this.httpClient
    .get<ShowGameView>(environment.webApiAdress + this.controllerPath + 'ShowGame?gameId=' + id + '&choos=' + choos);
  }

  getGameResult(id): Observable<ShowGameView> {
    return this.httpClient.get<ShowGameView>(environment.webApiAdress + this.controllerPath + 'GameResult?gameId=' + id);
  }
}
