import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Injectable()

export class StartGameService {

  constructor(private httpClient: HttpClient) {
   }

  getNames(): Observable<string[]> {
    return this.httpClient.get<string[]>('http://localhost:49784/api/Game/GetName');
  }

  StartGame(input): Observable<number> {
    return this.httpClient.get<number>('http://localhost:49784/api/Game/StartGame?name=' + input.name + '&botsNumber=' + input.botsNumber);
  }


}
