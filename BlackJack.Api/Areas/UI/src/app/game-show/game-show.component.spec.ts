import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GameShowComponent } from './game-show.component';

describe('GameShowComponent', () => {
  let component: GameShowComponent;
  let fixture: ComponentFixture<GameShowComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GameShowComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GameShowComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
