import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { apiWebAdress } from 'src/environments/environment';

@Injectable()

export class StartGameService {

  constructor(private httpClient: HttpClient) {
   }

   private controllerName = '/api/Game/';
  getNames(): Observable<string[]> {
    return this.httpClient.get<string[]>(apiWebAdress + this.controllerName + 'GetName');
  }

  StartGame(input): Observable<number> {
    return this.httpClient.get<number>(apiWebAdress + this.controllerName + 'StartGame?name=' + input.name + '&botsNumber=' + input.botsNumber);
  }


}
