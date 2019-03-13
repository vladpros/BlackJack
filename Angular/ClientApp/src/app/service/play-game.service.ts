import { HttpClient } from '@angular/common/http';
import { Injectable, Inject } from '@angular/core';

@Injectable()
export class PlayGameService {

  name: string[];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<string[]>(baseUrl + 'api/SampleData/GetName').subscribe(result => {
      this.name = result;
    }, error => console.error(error));
   }

  getName(): string[] {
    return this.name;
  }
}
