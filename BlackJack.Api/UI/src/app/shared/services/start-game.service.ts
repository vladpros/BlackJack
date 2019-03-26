import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { apiWebAdress } from 'src/environments/environment';
import { NameView} from '../models/name-view';

@Injectable()

export class StartGameService {

  constructor(private httpClient: HttpClient) {
   }

   private controllerName = '/api/Game/';

  getNames(): Observable<NameView> {
    return this.httpClient.get<NameView>(apiWebAdress + this.controllerName + 'GetName');
  }

  StartGame(input): Observable<number> {
    return this.httpClient.get<number>(apiWebAdress + this.controllerName + 'StartGame?name=' + input.name + '&botsNumber=' + input.botsNumber);
  }


}
