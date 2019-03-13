import { PlayGameService } from './../service/play-game.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-play-game',
  templateUrl: './play-game.component.html',
  styleUrls: ['./play-game.component.css'],
  providers: [PlayGameService]
})
export class PlayGameComponent implements OnInit {

  names: string[] = [];

  constructor(private playGameService: PlayGameService) {

  }

  ngOnInit() {
    this.names = this.playGameService.getName();
  }


}
