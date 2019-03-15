import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { GameInfo } from '../model/game-info';

@Injectable()
export class GameShowService {

  constructor(private httpClient: HttpClient) {
  }

  getGameInfo(id, choos): Observable<[GameInfo]> {
    return this.httpClient.get<[GameInfo]>('http://localhost:49784/api/Game/ShowGame?gameId=' + id + '&choos=' + choos);
  }

  getGameResult(id): Observable<[GameInfo]> {
    return this.httpClient.get<[GameInfo]>('http://localhost:49784/api/Game/GameResult?gameId=' + id);
  }
}
