import { StartGameComponent } from './start-game.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { ToastrModule } from 'ngx-toastr';
import { StartGameRoutingModule } from './start-game-routing.module';

@NgModule({
  declarations: [
    StartGameComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    ToastrModule,
    StartGameRoutingModule
  ]
})
export class StartGameModule { }
