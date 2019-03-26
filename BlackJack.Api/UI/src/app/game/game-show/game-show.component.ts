import { PlayerInGameView } from '../../sheared/models/player-in-game-view';
import { GameShowService } from './../../sheared/services/game-show.service';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-game-show',
  templateUrl: './game-show.component.html',
  styleUrls: ['./game-show.component.css'],
  providers: [
    GameShowService
  ]
})
export class GameShowComponent implements OnInit {

  private id: number;
  private choose: number;
  public gameInfo: PlayerInGameView;
  public isEnd = false;

  constructor(
    private activstRout: ActivatedRoute,
    private router: Router,
    private gameShowService: GameShowService,
    ) {

   }

  ngOnInit() {
    this.id = +this.activstRout.snapshot.paramMap.get('id');
    if (this.id === null) {
      this.router.navigate(['/startGame']);
    }
    this.showGame();
  }

  showGame() {
    this.gameShowService.getGameInfo(this.id, this.choose).subscribe(result => {
      this.gameInfo = result;
      console.log(this.gameInfo);
      this.findEndGame();
    },
    error => {
      this.router.navigate(['/startGame']);
    }
    );
  }

  resumePlay() {
    this.choose = 1;
    this.showGame();
  }

  stopPlay() {
    this.choose = 2;
    this.showGame();
  }

  findEndGame(): boolean {
    console.log(this.gameInfo);
    this.gameInfo.playerInGameViewItems.forEach(element => {
      if (element.playerStatus === 'Won') {
        this.isEnd = true;
      }
    });

    return false;
  }
}
