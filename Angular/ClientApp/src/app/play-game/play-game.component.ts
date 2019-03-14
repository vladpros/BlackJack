import { PlayGameService } from './../service/play-game.service';
import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs/Observable';

@Component({
  selector: 'app-play-game',
  templateUrl: './play-game.component.html',
  styleUrls: ['./play-game.component.css'],
  providers: [PlayGameService]
})
export class PlayGameComponent implements OnInit {

  names;

  constructor(private playGameService: PlayGameService) {
  }

  ngOnInit() {
    this.names = this.playGameService.getNames().subscribe( result => {
      console.log(result);
      this.names = result;
    },
    error => {
      console.error(error);
    });
    console.log(this.names);
  }


}
