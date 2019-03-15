import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class GameShowService {

  constructor(private httpClient: HttpClient) {
    console.log('I`m in services');
  }

  getGameInfo(id, choos): Observable<any> {
    return this.httpClient.get<any>('http://localhost:49784/api/Game/ShowGame?gameId=' + id + '&choos=' + choos);
  }
}
