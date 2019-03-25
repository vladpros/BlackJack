import { PlayerStatus } from '../../../sheared/enams/player-status.enum';
import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-player-status',
  templateUrl: './player-status.component.html',
  styleUrls: ['./player-status.component.css']
})
export class PlayerStatusComponent implements OnInit {

  playerStatusString;

  @Input() playerStatus: PlayerStatus;
  constructor() { }

  ngOnInit() {
    this.playerStatusString = PlayerStatus[this.playerStatus];
  }

}
