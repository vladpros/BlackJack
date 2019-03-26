import { PlayerChoose } from './../../shared/enum/player-choose.enum';
import { ShowGameView } from '../../shared/models/show-game-view';
import { GameShowService } from './../../shared/services/game-show.service';
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
  public gameInfo: ShowGameView;
  public isEnd = false;

  constructor(
    private activstRout: ActivatedRoute,
    private router: Router,
    private gameShowService: GameShowService,
    ) { }

  ngOnInit() {
    this.id = +this.activstRout.snapshot.paramMap.get('id');
    if (this.id === null) {
      this.router.navigate(['/game/startgame']);
    }
    this.showGame();
  }

  private showGame(): void {
    this.gameShowService.getGameInfo(this.id, this.choose).subscribe(result => {
      this.gameInfo = result;
      console.log(this.gameInfo);
      this.findEndGame();
    },
    error => {
      this.router.navigate(['/game/startgame']);
    }
    );
  }

  public resumePlay(): void {
    this.choose = PlayerChoose.ContinueGame;
    this.showGame();
  }

  public stopPlay(): void {
    this.choose = PlayerChoose.StopGame;
    this.showGame();
  }

  private findEndGame(): boolean {
    console.log(this.gameInfo);
    this.gameInfo.showGameViewItems.forEach(element => {
      if (element.playerStatus === 'Won') {
        this.isEnd = true;
      }
    });

    return false;
  }
}
