import { HttpClient } from '@angular/common/http';
import { Injectable, Inject } from '@angular/core';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class PlayGameService {

  name;

  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) { }

  getNames(): Observable<string> {
    return this.http.get<string>('http://localhost:55848/api/Game/GetName');
  }
}
