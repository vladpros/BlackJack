import { PlayerStatus } from '../enams/player-status.enum';
import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-player-status',
  templateUrl: './player-status.component.html',
  styleUrls: ['./player-status.component.css']
})
export class PlayerStatusComponent implements OnInit {

  playerStatusS;

  @Input() playerStatus: PlayerStatus;
  constructor() { }

  ngOnInit() {
    this.playerStatusS = PlayerStatus[this.playerStatus];
  }

}
