import { TestBed } from '@angular/core/testing';

import { GameShowService } from './game-show.service';

describe('GameShowService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: GameShowService = TestBed.get(GameShowService);
    expect(service).toBeTruthy();
  });
});
