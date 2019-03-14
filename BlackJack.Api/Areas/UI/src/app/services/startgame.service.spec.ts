import { TestBed } from '@angular/core/testing';

import { StartgameService } from './startgame.service';

describe('StartgameService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: StartgameService = TestBed.get(StartgameService);
    expect(service).toBeTruthy();
  });
});
