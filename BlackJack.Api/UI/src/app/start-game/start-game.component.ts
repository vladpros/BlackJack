import { Component, OnInit } from '@angular/core';
import { StartgameService } from '../services/startgame.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-start-game',
  templateUrl: './start-game.component.html',
  styleUrls: ['./start-game.component.css'],
  providers: [StartgameService]
})
export class StartGameComponent implements OnInit {

  myForm: FormGroup;
  names: string[];
  isValid = true;

  constructor(
    private startgameService: StartgameService,
    private formBuilder: FormBuilder,
    private router: Router,
    ) {
  }

  ngOnInit() {
    this.startgameService.getNames().subscribe(result => {
      this.names = result;
    },
    error => {
      console.error(error);
    });
    this.initForm();
  }

  initForm() {
    this.myForm = this.formBuilder.group({
      name: ['', Validators.required],
      botsNumber: [1]
    });
  }

  onSubmit() {
    if (this.myForm.invalid) {
      this.isValid = false;
      return;
    }
    this.startgameService.StartGame(this.myForm.value).subscribe(result => {
    this.router.navigate(['/playGame', result]);
    },
    error => {
      console.error(error);
    });
  }
}
