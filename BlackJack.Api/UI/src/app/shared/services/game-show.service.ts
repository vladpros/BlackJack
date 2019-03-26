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

  public getGameInfo(id, choos): Observable<ShowGameView> {
    return this.httpClient
    .get<ShowGameView>(environment.webApiAdress + this.controllerPath + 'Show?gameId=' + id + '&playerChoose=' + choos);
  }

  public getGameResult(id): Observable<ShowGameView> {
    return this.httpClient.get<ShowGameView>(environment.webApiAdress + this.controllerPath + 'GameResult?gameId=' + id);
  }
}
