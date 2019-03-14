import { Component, OnInit } from '@angular/core';
import { StartgameService } from '../services/startgame.service';
import { FormGroup, FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-start-game',
  templateUrl: './start-game.component.html',
  styleUrls: ['./start-game.component.css'],
  providers: [StartgameService]
})
export class StartGameComponent implements OnInit {

  myForm: FormGroup;
  names: string[];

  constructor(private startgameService: StartgameService, private formBuilder: FormBuilder) {
    console.log(this.names);
  }

  ngOnInit() {
    this.startgameService.getNames().subscribe(result => {
      this.names = result;
      console.log(result);
    },
    error => {
      console.error(error);
    });
    this.initForm();
  }

  initForm() {
    this.myForm = this.formBuilder.group({
      name: [''],
      botsNumber: [1]
    });
  }

  onSubmit() {
    console.log(this.myForm.value);
    this.startgameService.StartGame(this.myForm.value).subscribe(result => {
      console.log(result);
    },
    error => {
      console.error(error);
    });
  }
}
