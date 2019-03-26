import { Component, OnInit } from '@angular/core';
import { StartGameService } from '../../shared/services/start-game.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { NameView } from 'src/app/shared/models/name-view';

@Component({
  selector: 'app-start-game',
  templateUrl: './start-game.component.html',
  styleUrls: ['./start-game.component.css'],
  providers: [StartGameService]
})
export class StartGameComponent implements OnInit {

  public myForm: FormGroup;
  public nameView: NameView;
  public isValid = true;

  constructor(
    private startGameService: StartGameService,
    private formBuilder: FormBuilder,
    private router: Router,
    ) {
      this.startGameService.getNames().subscribe(result => {
        this.nameView = result;
      },
      error => {
        console.error(error);
      });
      this.initForm();
  }

  ngOnInit() {
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
    this.startGameService.StartGame(this.myForm.value).subscribe(result => {
    this.router.navigate(['/game/playgame', result]);
    },
    error => {
      console.error(error);
    });
  }
}
