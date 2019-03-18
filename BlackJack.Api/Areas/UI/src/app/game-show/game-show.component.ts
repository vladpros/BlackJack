import { GameInfo } from './../model/game-info';
import { GameShowService } from './../services/game-show.service';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-game-show',
  templateUrl: './game-show.component.html',
  styleUrls: ['./game-show.component.css'],
  providers: [GameShowService]
})
export class GameShowComponent implements OnInit {

  id: number;
  choos: number;
  gameInfo: [GameInfo];
  isEnd = false;

  constructor(
    private activstRout: ActivatedRoute,
    private router: Router,
    private gameShowService: GameShowService
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
    this.gameShowService.getGameInfo(this.id, this.choos).subscribe(result => {
      this.gameInfo = result;
      console.log(this.gameInfo);
      this.findEndGame();
    },
    error => {
      console.error(error);
      this.router.navigate(['/startGame']);
    }
    );
  }

  resumePlay() {
    this.choos = 1;
    this.showGame();
  }

  stopPlay() {
    this.choos = 2;
    this.showGame();
  }

  findEndGame(): boolean {
    this.gameInfo.forEach(element => {
      if (element.PlayerStatus === 4) {
        const id = this.id;
        this.isEnd = true;
      }
    });

    return false;
  }
}